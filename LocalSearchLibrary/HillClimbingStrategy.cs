using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace LocalSearchLibrary
{
    public class HillClimbingStrategy : SolutionStrategy
    {
        public HillClimbingStrategy(Board brd)
            : base(brd)
        {
        }
        /// <summary>
        /// for this strategy, move the appropriate queen
        /// to the lowest location in its column
        /// pick out the lowest tile to use 
        /// </summary>
        public override void ApplyStrategy()
        {
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            Tile tilLowest = LowestFreeTile();
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
        /// <summary>
        /// pick out the best (lowest) tiles to use, returns null if no tile works
        /// </summary>
        /// <returns>best tile or null</returns>
        public override Tile TestStrategy()
        {
            // pick out the lowest tile to use, returns null if no tile works
            Tile tilLowest = LowestFreeTile();
            return tilLowest;
        }
        /// <summary>
        /// set the state of the chess board
        /// G - good, I - incomplete, F - fail
        /// </summary>
        public override void SetBoardState()
        {
            if (this.NewConflicts == 0)
                _Board.Status = "G";    // no conflicts - we are done
            else if (this.OldConflicts != this.NewConflicts)  // board change but no goal state
                _Board.Status = "I";
            else
                _Board.Status = "F";    // local minima, we're stuck
        }
        /// <summary>
        /// set the state of the current working strategy
        /// </summary>
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
    }
}
