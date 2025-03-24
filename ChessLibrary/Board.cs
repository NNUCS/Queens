namespace ChessLibrary
{
    [Serializable]
    public class ChessBoard
    {
        #region Constants
        private const byte ROWS = 8, COLUMNS = 8;
        #endregion
        #region Properties
        public ushort IndicatorCurrent { get; set; }
        public ushort IndicatorMax { get; set; }
        public uint Iterations { get; set; } = 0;

        public List<Queen> Queens = new List<Queen>();
        public Tile[] Tiles;
        public String? Status {get; set;}
        public Object? objStuffToStash = null;  // in order to preserve strategy objects for interative use
        public byte Rows { get { return ROWS; } }
        public byte Columns { get { return COLUMNS; } }
        public double OldValue { get; set; } = 0; // for possible future strategies
        #endregion

        public ChessBoard()
        {
            Tiles = new Tile[ROWS * COLUMNS];
            RandomizeQueens();
        }
        // Perhaps someday we will have a variable board size
        public ChessBoard(byte bytRows, byte bytColumns)
        {
            Tiles = new Tile[bytRows * bytColumns];
            RandomizeQueens();
        }
        // copy constructor
        public ChessBoard(ChessBoard brd)
        {
            Tiles = new Tile[brd.Tiles.Length];
            for (int idx = 0; idx < Tiles.Length; idx++)
            {
                Tile tile = new Tile(brd.Tiles[idx].Row, brd.Tiles[idx].Column);
                Tiles[idx] = tile;
            }
            for (int idx = 0; idx < brd.Queens.Count; idx++)
            {
                Queen qn = new Queen(brd.Queens[idx].BoardPosition.Row, brd.Queens[idx].BoardPosition.Column);
                Queens.Add(qn);
            }
            // We can add these when we have a variable length board
            //_bytColumns = brd._bytColumns;
            //ROWS = brd.ROWS;
            OldValue = brd.OldValue;
            IndicatorCurrent = brd.IndicatorCurrent;
            IndicatorMax = brd.IndicatorMax;
            Iterations = brd.Iterations;
            Status = brd.Status;
        }


        public void RandomizeQueens()
        {
            // randomize the queens
            Queens.Clear();
            Random rnd = new Random();
            for (byte i = 0; i < ROWS; i++)
            {
                int iRow = rnd.Next(8);
                Queen qn = new Queen((byte)iRow, i);
                Queens.Add(qn);
                // do all columns for each row, then move to next row
                for (byte j = 0; j < COLUMNS; j++)
                {
                    Tiles[(i * ROWS) + j] = new Tile(j, i);  // unfortunately, this means we sequence backwards
                    Tiles[(i * ROWS) + j].Conflicts = 0;
                }
            }
            Status = "";
            IndicatorCurrent = 0;
            IndicatorMax = 0;
            Iterations = 0;
            OldValue = 0;
            UpdateConflicts();      // determine initial set of conflicts
        }
        public byte GetTotalConflictCount()
        {
            byte bytCount = 0;
            foreach (Queen qn in Queens)
            {
                bytCount += qn.ConflictCount(Queens);
            }
            return bytCount;
        }
        // get all conflicts for all squares
        public void UpdateConflicts()
        {
            byte bytCount = 0;
            Tile oldQueenLoc, newQueenLoc;
            for (byte bytCol = 0; bytCol < COLUMNS; bytCol++)
            {
                for (byte bytRow = 0; bytRow < ROWS; bytRow++)
                {
                    bytCount = 0;
                    newQueenLoc = new Tile(bytRow, bytCol);
                    Queen qn = this.Queens[bytCol];
                    oldQueenLoc = qn.BoardPosition;
                    qn.BoardPosition = newQueenLoc;
                    bytCount += this.GetTotalConflictCount();
                    this.Tiles[(bytCol * COLUMNS) + bytRow].Conflicts = bytCount;
                    this.Queens[bytCol].BoardPosition = oldQueenLoc;
                    qn.BoardPosition.Conflicts =
                        this.Tiles[(qn.BoardPosition.Column * COLUMNS) + qn.BoardPosition.Row].Conflicts;
                }
            }
        }
        // calculate what sort of conflict if queen moved to this spot
        // and populate the board
        public Boolean QueenConflict(Queen qn1, Queen qn2)
        {
            // same queen means no conflict
            if (qn1.BoardPosition.Column == qn2.BoardPosition.Column)
                return false;
            return qn1.PositionConflict(qn2);
        }
        // tells how many queens can attack a given tile
        public byte QueenConflictsByTile(Tile tilTest)
        {
            byte bytTotalConflicts = 0;
            foreach (Queen qn in Queens)
            {
                if (qn.PositionConflict(tilTest))
                    bytTotalConflicts++;
            }
            return bytTotalConflicts;
        }
        // tells how non-conflicts there are
        public byte TotalQueenNoConflicts()
        {
            byte bytTotalNoConflicts = 0;
            foreach (Queen qn in Queens)
            {
                for (byte bdx = 0; bdx < Queens.Count; bdx++)
                {
                    Queen qnTest = Queens[bdx];
                    if (!QueenConflict(qn, qnTest))
                        bytTotalNoConflicts++;
                }
            }
            return bytTotalNoConflicts;
        }
    }
}
