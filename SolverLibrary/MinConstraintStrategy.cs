using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace SolverLibrary
{
    public class MinConstraintStrategy : SolutionStrategy
    {
        public MinConstraintStrategy(ChessBoard brd)
            : base(brd)
        {
            if (brd.Status == "")
            {
                // initialization
                brd.IndicatorMax = 100;
                brd.IndicatorCurrent = 0;
                brd.OldValue = 100;
            }
        }
        public override void ApplyStrategy()
        {
            Tile tilLowest = null;
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            // for this strategy, move across the board
            // put each queen in the lowest conflict position in its respective row
            // just for some variety, sweep from right to left every other iteration
            if ((_Board.IndicatorCurrent % 2) == 0)
            {
                for (Byte bdx = 0; bdx < _Board.Columns; bdx++)
                {
                    tilLowest = GetBestNoConflictRowTile(bdx);
                    if (tilLowest != null)
                    {
                        Byte bytCol = tilLowest.Column;
                        Queen qn = _Board.Queens[bytCol];
                        Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = tilLowest.Row;
                        this._Board.Queens[bytCol].BoardPosition = tilLowest;
                    }
                }
            }
            else
            {
                for (Int32 idx = _Board.Columns - 1; idx >= 0; idx--)
                {
                    tilLowest = GetBestNoConflictRowTile((Byte)idx);
                    if (tilLowest != null)
                    {
                        Byte bytCol = tilLowest.Column;
                        Queen qn = _Board.Queens[bytCol];
                        Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = tilLowest.Row;
                        this._Board.Queens[bytCol].BoardPosition = tilLowest;
                    }
                }
            }
            this._Board.UpdateConflicts();
            this.NewConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            SetBoardState();
            SetStrategyStatus();
        }
        public override Tile TestStrategy()
        {
            // pick out the lowest tile to use, returns null if no tile works
            Tile tilLowest = LowestFreeTile();
            return tilLowest;
        }
        public override void SetBoardState()
        {
            if (this.NewConflicts == 0)
                _Board.Status = "G";    // no conflicts - we are done
            else if (this.OldConflicts != this.NewConflicts)  // board change but no goal state
                _Board.Status = "I";
            else
            {
                if(_Board.IndicatorCurrent < _Board.IndicatorMax)
                    _Board.IndicatorCurrent++;
                else
                    _Board.Status = "F";    // local minima, we're stuck
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
                    Status = "Still working, iteration: " + _Board.IndicatorCurrent.ToString();
                    break;
                case "F":   // failed state
                    Status = "Failure";
                    break;
            }
        }
        private Tile GetBestNoConflictRowTile(Byte bytCol)
        {
            Byte bytCount, bytLowestCount = 64;
            Tile tLowest = _Board.Tiles[0];
            // per Miles, try to save all tiles that are lowest and randomly pick
            // among them
            List<Tile> LowestTiles = new List<Tile>();
            // first pass, find the lowest conflict count for the row
            for (Byte bytRow = 0; bytRow < _Board.Rows; bytRow++)
            {
                tLowest = _Board.Tiles[(bytCol * _Board.Columns) + bytRow];
                bytCount = _Board.QueenConflictsByTile(tLowest); ;
                if (bytCount < bytLowestCount)
                    bytLowestCount = bytCount;
            }
            // second pass, load all those tiles whose conflict count is low enough
            for (Byte bytRow = 0; bytRow < _Board.Rows; bytRow++)
            {
                Byte bytIdx = bytCol;
                bytIdx *= _Board.Columns;
                bytIdx += bytRow;
                tLowest = _Board.Tiles[(bytCol * _Board.Columns) + bytRow];
                if (_Board.QueenConflictsByTile(tLowest) <= bytLowestCount)
                {
                    LowestTiles.Add(tLowest);
                }
            }
            if (LowestTiles.Count == 0)
                return null;    // should never happen
            // if we have more than one lowest tile, randomly pick lowest one
            if (LowestTiles.Count > 1)
            {
                // randomize the tiles, get the random row with the lowest tile
                Random rnd = new Random();
                int iTile = rnd.Next(LowestTiles.Count);
                tLowest = LowestTiles[iTile];
            }
            else
                tLowest = LowestTiles[0]; // just one tile so return it
            return tLowest;
        }
    }
}
