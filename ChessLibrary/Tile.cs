using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    [Serializable]
    public class Tile
    {
        public byte Row { get; set; }
        public byte Column { get; set; }
        public byte Conflicts { get; set; }
        public Queen? TileQueen { get; set; } // placeholder for a queen

        public Tile(byte bytRow, byte bytCol)
        {
            Row = bytRow;
            Column = bytCol;
            TileQueen = null;
        }

    }

}
