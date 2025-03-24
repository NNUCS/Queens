using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace SolverLibrary
{
    public class RandomRestartStrategy : SolutionStrategy
    {
        public RandomRestartStrategy(ChessBoard brd)
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
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            // for this strategy, move the appropriate queen
            // to the lowest location in its column
            // pick out the lowest tile to use
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
                // we're stuck so do a random restart - with 1 column
                if (_Board.IndicatorCurrent < _Board.IndicatorMax)
                {
                    Random rnd = new Random();
                    Byte bytCol = (Byte)rnd.Next(8);
                    Byte bytNewPos = (Byte)rnd.Next(8);
                    // get a new position
                    while (bytNewPos == _Board.Queens[bytCol].BoardPosition.Row)
                        bytNewPos = (Byte)rnd.Next(8);
                    Tile tilRestart = _Board.Tiles[(bytCol * _Board.Columns) + bytNewPos];
                    _Board.Queens[bytCol].BoardPosition = tilRestart;
                    this._Board.IndicatorCurrent++;
                    this._Board.UpdateConflicts();
                }
                else
                    _Board.Status = "F";    // we're stuck, give up
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
                    Status = "Still working, indicator: " + _Board.IndicatorCurrent.ToString();
                    break;
                case "F":   // failed state
                    Status = "Failure";
                    break;
            }
        }
    }
}
