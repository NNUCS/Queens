using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace LocalSearchLibrary
{
    public class StochasticHillClimb : SolutionStrategy
    {
        public StochasticHillClimb(Board brd)
            : base(brd)
        {
        }
        public override void ApplyStrategy()
        {
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            // for this strategy, move the appropriate queen
            // to the lowest location in its column
            // pick out the lowest tile to use
            Tile tilLowest = GetLowerFreeTile();
            if (tilLowest != null)
            {
                Byte bytCol = tilLowest.Column;
                Queen qn = _Board.Queens[bytCol];
                Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = tilLowest.Row;
                this._Board.Queens[bytCol].BoardPosition = tilLowest;
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
                _Board.Status = "F";    // local minima, we're stuck
        }
        public override void SetStrategyStatus()
        {
            switch (this._Board.Status)
            {
                case "G":    // goal state
                    Status = "Success";
                    break;
                case "I":   // intermediate state
                    Status = "Still working...";
                    break;
                case "F":   // failed state
                    Status = "Failure";
                    break;
            }
        }
        public Tile GetLowerFreeTile()
        {
            Byte bytLowestCount = _Board.Queens[0].BoardPosition.Conflicts;
            Tile LowestTile = null;
            List<Tile> LowestTiles = new List<Tile>();

            for (Byte bytCol = 0; bytCol < _Board.Columns; bytCol++)
            {
                for (Byte bytRow = 0; bytRow < _Board.Rows; bytRow++)
                {
                    Int32 tilIdx = (bytCol * 8) + bytRow;
                    if (_Board.Tiles[tilIdx].Conflicts < bytLowestCount)
                    {
                        // here's my take on the stochastic, enter a tile the number of times it
                        // is better than the current state
                        for(Int32 idx = 0; idx < bytLowestCount - _Board.Tiles[tilIdx].Conflicts; idx++)
                            LowestTiles.Add(_Board.Tiles[tilIdx]);   // add it to the pile if it qualifies
                    }
                }
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
                int iTile = rnd.Next(LowestTiles.Count);
                LowestTile = LowestTiles[iTile];
            }
            else
                LowestTile = LowestTiles[0]; // just one tile so return it
            return LowestTile;
        }
    }
}
