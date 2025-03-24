using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace SolverLibrary
{
    public class KSimulatedAnnealingStrategy : SolutionStrategy
    {
        Double _dblTemperature = 1000, _dblAlpha = .99;
        Boolean _bTileUsed = false;
        Double _dblCurrentTileError = 0, _dblErrorMax = 0;
        Byte _bytCurrentTileConflicts = 0;
        Int16 _iConflictDiff = 0;

        public KSimulatedAnnealingStrategy(ChessBoard brd)
            : base(brd)
        {
            if (brd.Status == "")
            {
                // initialization
                brd.IndicatorMax = 1000;
                brd.IndicatorCurrent = 1000;
                brd.OldValue = 1000;
            }
            else
            {
                brd.OldValue *= _dblAlpha;
                _dblTemperature = brd.OldValue;
                brd.IndicatorCurrent--;
            }
        }
        public Double Temperature
        {
            get { return _dblTemperature; }
            set { _dblTemperature = value; } 
        }
        public override void ApplyStrategy()
        {
            // the idea is to start with a relatively large value and then turn down
            // the temperature until you find solution or are done
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            Tile tilNextFree = null;
            _bTileUsed = false;
            tilNextFree = NextFreeTile();
            if (tilNextFree != null)
            {
                _bTileUsed = true;
                Byte bytCol = tilNextFree.Column;
                Queen qn = _Board.Queens[bytCol];
                Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = tilNextFree.Row;
                this._Board.Queens[bytCol].BoardPosition = tilNextFree;
                this._Board.UpdateConflicts();
            }
            this.NewConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            SetBoardState();
            SetStrategyStatus();
        }
        public override Tile TestStrategy()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void SetBoardState()
        {
            // test goal state first
            if (NewConflicts == 0)
            {
                _Board.Status = "G";
                return;
            }
            else
            {   
                _Board.Status = "I";        // keep going
                if(Temperature <= 0 || _Board.IndicatorCurrent <= 0)
                    _Board.Status = "F";
            }
        }
        public Tile NextFreeTile()
        {
            Tile NextTile = PickRandomTile();
            if( IsTileOK(NextTile))
                return NextTile;
            else
                return null;
        }
        public Tile PickRandomTile()
        {
            List<Tile> lstTiles = new List<Tile>();
            Tile tilFinal;

            for (Int32 idx = 0; idx < 8; idx++)
            {
                for (Int32 jdx = 0; jdx < 8; jdx++)
                {
                    Tile til = _Board.Tiles[idx * 8 + jdx];
                    // capt. obvious - return tile if it contains goal state
                    if (til.Conflicts == 0)
                        return til;
                    Int32 iMaxConflicts = (Int32)Math.Truncate(Math.Sqrt(Temperature));
                    if(iMaxConflicts < 1)
                        iMaxConflicts = 1;
                    if (IsTileOK(til) && til.Conflicts <= iMaxConflicts)
                        lstTiles.Add(til);
                }
            }
            // if no tiles fit, then return random one
            if (lstTiles.Count == 0)
            {
                Random rnd = new Random();
                int iTile = rnd.Next(64);
                return _Board.Tiles[iTile];
            }
            // if we have more than one lowest tile, randomly pick lowest one
            if (lstTiles.Count > 1)
            {
                // randomize the tiles, get the random row with the lowest tile
                Random rnd = new Random();
                int iTile = rnd.Next(lstTiles.Count);
                tilFinal = lstTiles[iTile];
            }
            else
                tilFinal = lstTiles[0]; // just one tile so return it
            return tilFinal;
        }
        public Boolean IsTileOK(Tile tilTarget)
        {
            // if tile reduces conflicts, accept it
            // otherwise calculate the error of a given tile
            // error = exp ^ (deltaE / T)
            // if error below random, then OK, otherwise reject
            // in this case, set our error schedule = T/Tmax
            Double deltaE = _Board.Queens[0].BoardPosition.Conflicts - tilTarget.Conflicts;
            _iConflictDiff = (Int16)deltaE;
            if (deltaE < 0)
            {
                //if (deltaE < -2)
                //    System.Windows.Forms.MessageBox.Show("Stop");
                Double testExp = deltaE / _Board.OldValue;
                Double dblTestError = Math.Exp(testExp);
                _dblErrorMax = dblTestError;    // largest error we allow
                // now compare to random number between 0 and 1
                Random rnd = new Random();
                Double dblCompare = rnd.NextDouble();
                _dblCurrentTileError = dblCompare;
                if (_dblCurrentTileError < _dblErrorMax)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }
        public override void SetStrategyStatus()
        {
            switch (this._Board.Status)
            {
                case "G":    // goal state
                    Status = "Success";
                    break;
                case "I":   // intermediate state
                    Status = "Still working - temp: " + Math.Round(_dblTemperature, 3).ToString();
                    Status += "\r\n" + "Error Max:" + Math.Round(_dblErrorMax, 3).ToString();
                    if (_bTileUsed)
                    {
                        Status += "\r\n" + "Tile Used,";
                        if (_dblCurrentTileError < _dblErrorMax)
                            Status += "\r\n" + "Tile below threshold";
                    }
                    else
                    {
                        Status += "\r\n" + "Tile Not Used,";
                        if (_dblCurrentTileError > _dblErrorMax)
                            Status += "\r\n" + "Tile above error threshold";
                        else
                            Status += "\r\n" + "Tile in unknown state";
                    }
                    Status += "\r\n" + "Conflicts: " + _bytCurrentTileConflicts.ToString() 
                        + ", Error: " + Math.Round(_dblCurrentTileError, 3).ToString();
                    if (_bTileUsed)
                    {
                        if (_iConflictDiff > 0)
                            Status += "\r\nTile Reduces Conflicts";
                        else if (_iConflictDiff == 0)
                            Status += "\r\nTile is same";
                        else
                            Status += "\r\nTile increased conflicts";
                    }
                    Status += "\r\n" + "Iterations: " + (1000 - _Board.IndicatorCurrent).ToString();
                    break;
                case "F":   // failed state
                    Status = "Failure - temp: " + Math.Round(Temperature, 3).ToString();
                    break;
            }
        }
    }
}
