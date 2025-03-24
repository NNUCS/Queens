using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace SolverLibrary
{
    public class KHillClimbStrategy : SolutionStrategy
    {
        Byte _bytTemperature = 0;
        public KHillClimbStrategy(ChessBoard brd, Byte bytTemperature)
            : base(brd)
        {
            Temperature = bytTemperature;
            // initialize board if appropriate
            if (_Board.Status == "" && _Board.IndicatorMax == 0)
            {
                _Board.IndicatorMax = Temperature;     // we don't want to use until we hit a local minimum
                _Board.IndicatorCurrent = Temperature;
            }
            // in case somebody ups the threshold in the middle, restart
            if (_Board.IndicatorMax < Temperature)
            {
                _Board.IndicatorMax = Temperature;
                _Board.IndicatorCurrent = Temperature;
            }
        }
        public override void ApplyStrategy()
        {
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            Tile tilNextFree = null;

            if (_Board.Status == "" || _Board.Status == "I")
                tilNextFree = LowestFreeTile();
            else
                tilNextFree = NextFreeTile();
            if (tilNextFree != null)
            {
                Byte bytCol = tilNextFree.Column;
                Queen qn = _Board.Queens[bytCol];
                Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = tilNextFree.Row;
                this._Board.Queens[bytCol].BoardPosition = tilNextFree;
            }
            this._Board.UpdateConflicts();
            this.NewConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            SetBoardState();
            SetStrategyStatus();
        }
        public override Tile TestStrategy()
        {
            Tile tilNextFree = null;

            if (_bytTemperature == 0)
                tilNextFree = LowestFreeTile();
            else
                tilNextFree = NextFreeTile();
            return tilNextFree;
        }
        public override void SetBoardState()
        {
            // test goal state first
            if (NewConflicts == 0)
            {
                _Board.Status = "G";
                return;
            }

            if (_Board.Status == "" || _Board.Status == "I")
            {
                if (OldConflicts != NewConflicts)
                    _Board.Status = "I";        // keep going
                else  // we hit a local minima bounce queen or quit
                {
                    if (_Board.IndicatorCurrent > 0)
                        _Board.Status = "S";
                    else
                        _Board.Status = "F";    // we're done
                }
                return;
            }
            if (_Board.Status == "S")
            {
                if (OldConflicts != NewConflicts)
                {
                    _Board.Status = "I";
                    if(_Board.IndicatorCurrent > 0)
                        _Board.IndicatorCurrent--;
                }
                else
                {
                    if (_Board.IndicatorCurrent > 1)
                        _Board.IndicatorCurrent--;
                    else
                        _Board.Status = "F";    // fail because the bounce didn't change anything
                    // later we may try again using a modified technique
                }
            }
        }
        public override void SetStrategyStatus()
        {
            switch (this._Board.Status)
            {
                case "G":    // goal state
                    Status = "Success";
                    break;
                case "I":   // intermediate state
                    Status = "Still working - temp: " + _Board.IndicatorCurrent.ToString();
                    break;
                case "S":   // switching methods
                    Status = "In process - temp: " + _Board.IndicatorCurrent.ToString();
                    break;
                case "F":   // failed state
                    Status = "Failure - temp: " + _Board.IndicatorCurrent.ToString();
                    break;
            }
        }
        public Byte Temperature
        {
            get { return _bytTemperature; }
            set { _bytTemperature = value; }
        }
        public Tile NextFreeTile()
        {
            Byte bytCount, bytLowestCount = 100;
            Tile LowestTile = null;
            List<Tile> LowestTiles = new List<Tile>();

            for (Byte bytCol = 0; bytCol < _Board.Columns; bytCol++)
            {
                Queen qn = _Board.Queens[bytCol];
                List<Tile> tLowest = GetLowestAnnealedRowStates(qn.BoardPosition.Column);
                // skip those queens already in their goal state
                // defensive code - no conflicts, nothing to do
                if (qn.BoardPosition.Conflicts == 0)
                    return null;

                // skip those rows whose queens are already at their lowest point
                if ((tLowest.Count == 1) && (qn.BoardPosition == tLowest[0]) && _bytTemperature == 0)
                    continue;

                // new position must be less than queen
                if ((qn.BoardPosition.Conflicts > tLowest[0].Conflicts - _bytTemperature))
                {
                    bytCount = tLowest[0].Conflicts;
                    if (bytCount < bytLowestCount)
                        bytLowestCount = bytCount;
                    // so add new tiles to our master list
                    foreach (Tile til in tLowest)
                        LowestTiles.Add(til);
                }
            }
            // now get rid of any tiles above our lowest threshold
            for (Int32 idx = LowestTiles.Count - 1; idx >= 0; idx--)
            {
                Tile til = LowestTiles[idx];
                if (til.Conflicts > bytLowestCount + _bytTemperature)
                    LowestTiles.RemoveAt(idx);
            }
            // in the event we can't get a lowest tile
            // because the queens are already in their lowest state
            // return null
            if (LowestTiles.Count == 0)
                return null;
            // if we have more than one lowest tile, randomly pick lowest one
            if (LowestTiles.Count > 1)
            {
                // randomize the tiles, get the random row with the lowest tile
                Random rnd = new Random();
                int iTile = rnd.Next(LowestTiles.Count - 1);
                LowestTile = LowestTiles[iTile];
            }
            else
                LowestTile = LowestTiles[0]; // just one tile so return it
            return LowestTile;
        }
        private List<Tile> GetLowestAnnealedRowStates(Byte bytCol)
        {
            Byte bytCount, bytLowestCount = 64;
            Tile tLowest = _Board.Tiles[0];
            // per Miles, try to save all tiles that are lowest and randomly pick
            // among them
            List<Tile> LowestTiles = new List<Tile>();
            // first pass, find the lowest conflict count for the row
            for (Byte bytRow = 0; bytRow < _Board.Rows; bytRow++)
            {
                bytCount = _Board.Tiles[(bytCol * _Board.Columns) + bytRow].Conflicts;
                if (bytCount < bytLowestCount)
                    bytLowestCount = bytCount;
            }
            // second pass, load all those tiles whose conflict count is low enough
            for (Byte bytRow = 0; bytRow < _Board.Rows; bytRow++)
            {
                if (_Board.Tiles[(bytCol * _Board.Columns) + bytRow].Conflicts <= bytLowestCount + _bytTemperature)
                {
                    tLowest = _Board.Tiles[(bytCol * _Board.Columns) + bytRow];
                    LowestTiles.Add(tLowest);
                }
            }
            return LowestTiles;
        }
    }
}
