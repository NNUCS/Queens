using System;
using System.Collections.Generic;
using System.Text;
using ChessLibrary;

namespace SolverLibrary
{
    public class TabuStrategy : SolutionStrategy
    {
        private List<UInt64> lTabu = new List<UInt64>();
        public TabuStrategy(ChessBoard brd)
            : base(brd)
        {
            if (brd.Status == "")
            {
                // initialization
                brd.IndicatorMax = 100;   // this is the most iterations we allow before giving up
                brd.IndicatorCurrent = 0;
            }
            else
            {
                brd.IndicatorCurrent++;
                if (brd.objStuffToStash != null)
                {
                    lTabu = (List<UInt64>)brd.objStuffToStash;
                }
            }
        }
        public override void ApplyStrategy()
        {
            this.OldConflicts = _Board.Queens[0].BoardPosition.Conflicts;
            Tile tilNextFree = null;

            tilNextFree = LowestNeighborhoodTile();
            if (tilNextFree != null)
            {
                Byte bytCol = tilNextFree.Column;
                Queen qn = _Board.Queens[bytCol];
                Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = tilNextFree.Row;
                this._Board.Queens[bytCol].BoardPosition = tilNextFree;
                UInt64 uiNew = GetTabuValue();
                lTabu.Add(uiNew);
                lTabu.Sort();
                this._Board.UpdateConflicts();
                this.NewConflicts = _Board.Queens[0].BoardPosition.Conflicts;
                _Board.objStuffToStash = lTabu;
                SetBoardState();
                SetStrategyStatus();
            }
            else  // we're done, can't find any new tiles
            {
                _Board.Status = "F";
                Status = "Failure - iteration: " + _Board.IndicatorCurrent.ToString();
            }
        }
        public override Tile TestStrategy()
        {
            Tile tilNextFree = LowestNeighborhoodTile();
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
                if (_Board.IndicatorCurrent < _Board.IndicatorMax)
                    _Board.Status = "I";        // keep going
                else
                    _Board.Status = "F";
                return;
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
                    Status = "Still working - iteration: " + _Board.IndicatorCurrent.ToString();
                    break;
                case "F":   // failed state
                    Status = "Failure - iteration: " + _Board.IndicatorCurrent.ToString();
                    break;
            }
        }
        public Tile LowestNeighborhoodTile()
        {
            Byte bytLowestCount = _Board.Queens[0].BoardPosition.Conflicts;
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

                // new position(s) must be less than or equal to current lowest count
                if (tLowest[0].Conflicts <= bytLowestCount)
                {
                    // so add new tiles to our master list
                    foreach (Tile til in tLowest)
                        LowestTiles.Add(til);
                }
            }
            if (LowestTiles.Count > 0)
            {
                // get rid of any tiles that would make configurations we already tried
                for (Int32 idx = (LowestTiles.Count - 1); idx >= 0; idx--)
                {
                    Byte bytCol = LowestTiles[idx].Column;
                    Queen qn = _Board.Queens[bytCol];
                    Byte bytRowOld = qn.BoardPosition.Row, bytRowNew = LowestTiles[idx].Row;
                    Tile tilOld = this._Board.Queens[bytCol].BoardPosition;
                    this._Board.Queens[bytCol].BoardPosition = LowestTiles[idx];
                    UInt64 uiTabuTest = GetTabuValue();
                    for (Int32 jdx = 0; jdx < lTabu.Count; jdx++)
                    {
                        if (uiTabuTest == lTabu[jdx])
                        {
                            // we have already tried this tile so don't use it
                            LowestTiles.RemoveAt(idx);
                        }
                    }
                    qn.BoardPosition = tilOld;
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
        private List<Tile> GetLowestRowTile(Byte bytCol)
        {
            Byte bytCount, bytLowestCount = 64;
            Tile tLowest = _Board.Tiles[0];
            // try to save all tiles that are lowest and randomly pick
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
        private UInt64 GetTabuValue()
        {
            UInt64 uiTabuValue = 0;

            // each row/col represents a single bit in our Tabu value
            foreach (Queen qn in _Board.Queens)
            {
                Byte bBitPos = (Byte)((qn.BoardPosition.Column * 8) + qn.BoardPosition.Row);
                UInt64 iBitNum = (UInt64)Math.Pow(2, bBitPos);
                uiTabuValue |= iBitNum;
            }
            return uiTabuValue;
        }
    }
}
