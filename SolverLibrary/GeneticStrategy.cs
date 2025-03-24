using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace SolverLibrary
{
    public class GeneticStrategy : SolutionStrategy
    {
        private List<ChessBoard> _popBoards = new List<ChessBoard>();
        private const Double RANDOM_CROSSOVER = .75;   // should crossover about 6 columns per pass
        private const Double RANDOM_MUTATION = .125;   // should mutate about 1 column per pass
        // create a population of 4 - our existing board plus 3 other boards
        private const Byte POP_SIZE = 4;
        public GeneticStrategy(ChessBoard brd)
            : base(brd)
        {
            if (brd.Status == "")
            {
                // initialization
                brd.IndicatorMax = 1000;
                brd.IndicatorCurrent = 0;
                // create new array of board population
                // with a bunch of random boards
                // each board already has a random set of queens
                for (Int32 idx = 0; idx < POP_SIZE - 1; idx++)
                {
                    ChessBoard popBoard = new ChessBoard();
                    popBoard.IndicatorMax = 1000;
                    popBoard.IndicatorCurrent = 0;
                    popBoard.UpdateConflicts();
                    _popBoards.Add(popBoard);
                }
                // existing board is last board
                _popBoards.Add(brd);
            }
            else
            {
                if (brd.objStuffToStash != null)
                {
                    // we need to retrieve our population from the main board
                    for (Int32 idx = 0; idx < POP_SIZE; idx++)
                    {
                        _popBoards = (List<ChessBoard>)brd.objStuffToStash;
                        _popBoards[idx].IndicatorCurrent++;
                    }
                }
                else
                {
                    // initialize everything
                    brd.IndicatorMax = 100;
                    brd.IndicatorCurrent = 0;
                    // create new array of board population
                    // with a bunch of random boards
                    for (Int32 idx = 0; idx < POP_SIZE - 1; idx++)
                    {
                        ChessBoard popBoard = new ChessBoard();
                        popBoard.IndicatorMax = 100;
                        popBoard.IndicatorCurrent = 0;
                        popBoard.UpdateConflicts();
                        _popBoards.Add(popBoard);
                    }
                    // existing board is last board
                    _popBoards.Add(brd);
                }
            }
        }
        public override void ApplyStrategy()
        {
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            ApplyGeneticAlgorithm();
            this.NewConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            SetStrategyStatus();
            SetBoardState();
        }
        private void ApplyGeneticAlgorithm()
        {
            ChessBoard brdWinner, brdLoser, brdTemp;
            Int32 iWinner, iLoser;
            // in this strategy, we take several steps
            // 1. pick two boards at random
            Random rnd = new Random();
            iWinner = rnd.Next(POP_SIZE);
            iLoser = rnd.Next(POP_SIZE);
            // so they are not the same
            while (iWinner == iLoser)
                iLoser = rnd.Next(POP_SIZE);

            brdWinner = _popBoards[iWinner];
            brdLoser = _popBoards[iLoser];

            // 2. decide the best of the two and set aside as the winner
            if (brdWinner.TotalQueenNoConflicts() < brdLoser.TotalQueenNoConflicts())
            {
                // if loser if better than winner swap
                brdTemp = brdLoser;
                brdLoser = brdWinner;
                brdWinner = brdTemp;
            }

            // 3. crossover random columns from winner to loser
            for (Int32 idx = 0; idx < 8; idx++)
            {
                if (rnd.NextDouble() <= RANDOM_CROSSOVER)
                {
                    brdLoser.Queens[idx].BoardPosition.Column = brdWinner.Queens[idx].BoardPosition.Column;
                    brdLoser.Queens[idx].BoardPosition.Row = brdWinner.Queens[idx].BoardPosition.Row;
                }
            }
            // 4. at random intervals, mutate one or more of loser columns
            for (Int32 idx = 0; idx < 8; idx++)
            {
                if (rnd.NextDouble() <= RANDOM_MUTATION)
                {
                    brdLoser.Queens[idx].BoardPosition.Row = (Byte)rnd.Next(8);
                }
            }
            // 5. update the boards and pass back as the display board the winner
            brdWinner.UpdateConflicts();
            brdLoser.UpdateConflicts();
            _Board = brdWinner;
            // hopefully preserve our array for the next iteration
            _Board.objStuffToStash = _popBoards;
        }
        public override void SetBoardState()
        {
            switch (this._Board.Status)
            {
                case "G":    // goal state
                    Status = "Success";
                    break;
                case "":
                    Status = "Still working, iterations: " + _Board.IndicatorCurrent;
                    Status += "\r\nConflicts " + _popBoards[0].Queens[0].BoardPosition.Conflicts.ToString();;
                    for (Int32 idx = 1; idx < POP_SIZE; idx++)
                    {
                        Status += ", " + _popBoards[idx].Queens[0].BoardPosition.Conflicts.ToString();
                    }
                    //Status = "I";
                    break;
                case "I":   // intermediate state
                    Status = "Still working, iterations: " + _Board.IndicatorCurrent;
                    Status += "\r\nConflicts " + _popBoards[0].Queens[0].BoardPosition.Conflicts.ToString(); ;
                    for (Int32 idx = 1; idx < POP_SIZE; idx++)
                    {
                        Status += ", " + _popBoards[idx].Queens[0].BoardPosition.Conflicts.ToString();
                    }
                    break;
                case "F":   // failed state
                    Status = "Failure";
                    break;
            }
        }
        public override void SetStrategyStatus()
        {
            if (this.NewConflicts == 0)
                _Board.Status = "G";    // no conflicts - we are done
            else if (_Board.IndicatorCurrent < _Board.IndicatorMax)  // no goal state
                for (Int32 idx = 0; idx < POP_SIZE; idx++)
                {
                    _popBoards[idx].Status = "I";
                }
            else
                _Board.Status = "F";    // local minima, we're stuck
        }
        public override Tile TestStrategy()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        //private List<Queen> LowestQueens(Byte bytNumQueens)
        //{
        //    List<Queen> lq = new List<Queen>[bytNumQueens];
        //    Byte bytLowestHigh = 9;    // 8 is the max
        //    Byte bytConflicts = 0;
        //    for (Int32 idx = 0; idx < bytNumQueens; idx++)
        //    {
        //        lq[idx] = null;
        //    }
        //    Byte bytLowestHigh;
        //    // evaluate fitness of each queen
        //    // by comparing # of no conflicts vs. maximum for that queen - 7
        //    // no queen conflicts with itself so we can shortcut the code
        //    // and not worry about the queen itself
        //    foreach (Queen qn in _Board.Queens)
        //    {
        //        bytConflicts = 0;
        //        for (Int32 idx = 0; idx < 8; idx++)
        //        {
        //            // count conflicts of this queen with all other queens
        //            if(_Board.QueenConflict(qn, _Board.Queens[idx]))
        //                bytConflicts++;
        //        }
                
        //    }
        //}
    }
}
