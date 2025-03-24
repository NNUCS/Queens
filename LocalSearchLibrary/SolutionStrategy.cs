using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace LocalSearchLibrary
{
    public abstract class SolutionStrategy
    {
        /// <summary>
        /// Represents the chess board (8x8)
        /// conflicts - number of before/after conflicts based upon a move
        /// status - tells the display what the current board status is
        /// </summary>
        protected Board _Board;   // setup of queens
        Byte _bytOldConflicts, _bytNewConflicts;
        String _sStatus;

        /// <summary>
        /// plug in a particular chess board to instantiate this strategy
        /// </summary>
        /// <param name="brd">the chess board</param>
        public SolutionStrategy(Board brd)
        {
            _Board = brd;
        }

        /// <summary>
        /// conflicts before strategy applied
        /// </summary>
        public Byte OldConflicts
        {
            get { return _bytOldConflicts; }
            set { _bytOldConflicts = value; }
        }
        /// <summary>
        /// conflicts after strategy applied
        /// </summary>
        public Byte NewConflicts
        {
            get { return _bytNewConflicts; }
            set { _bytNewConflicts = value; }
        }
        /// <summary>
        /// a string to report back results of strategy
        /// </summary>
        public String Status
        {
            get { return _sStatus; }
            set { _sStatus = value; }
        }
        /// <summary>
        /// the chess board
        /// </summary>
        public Board TheBoard
        {
            get { return _Board; }
        }

        /// <summary>
        /// call the specific strategy function to move a piece
        /// </summary>
        public abstract void ApplyStrategy();

        /// <summary>
        /// this function does a "check" of a strategy without actually moving a piece
        /// </summary>
        /// <returns>tile with the best results from the test, or null if no better tile</returns>
        public abstract Tile TestStrategy();

        /// <summary>
        /// set the state of the board itself
        /// </summary>
        public abstract void SetBoardState();
        /// <summary>
        /// set the state of a particular strategy
        /// </summary>
        public virtual void SetStrategyStatus()
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

        /// <summary>
        /// scans the chessboard and returns the tile with the least number of conflicts
        /// </summary>
        /// <returns>best tile</returns>
        public virtual Tile LowestFreeTile()
        {
            Byte bytCount, bytLowestCount = 100;
            Tile LowestTile = null;
            List<Tile> LowestTiles = new List<Tile>();

            for (Byte bytCol = 0; bytCol < _Board.Columns; bytCol++)
            {
                Queen qn = _Board.Queens[bytCol];
                List<Tile> tLowest = GetLowestRowTile(qn.BoardPosition.Column);
                // skip those queens already in their goal state
                // defensive code - no conflicts, nothing to do
                if (qn.BoardPosition.Conflicts == 0)
                    return null;

                // skip those rows whose queens are already at their lowest point
                if ((tLowest.Count == 1) && (qn.BoardPosition == tLowest[0]))
                    continue;

                // new position must be less than queen
                if ((qn.BoardPosition.Conflicts > tLowest[0].Conflicts))
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
                if (til.Conflicts > bytLowestCount)
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
                int iTile = rnd.Next(LowestTiles.Count);
                LowestTile = LowestTiles[iTile];
            }
            else
                LowestTile = LowestTiles[0]; // just one tile so return it
            return LowestTile;
        }
        /// <summary>
        /// returns the tile for a given column
        /// </summary>
        /// <param name="bytCol"></param>
        /// <returns></returns>
        private List<Tile> GetLowestRowTile(Byte bytCol)
        {
            Byte bytCount, bytLowestCount = 64;
            Tile tLowest = _Board.Tiles[0];
            // save all tiles that have the best score and randomly pick among them
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
                Byte bytIdx = bytCol;
                bytIdx *= _Board.Columns;
                bytIdx += bytRow;
                if (_Board.Tiles[bytIdx].Conflicts <= bytLowestCount)
                {
                    tLowest = _Board.Tiles[(bytCol * _Board.Columns) + bytRow];
                    LowestTiles.Add(tLowest);
                }
            }
            return LowestTiles;
        }
    }
}
