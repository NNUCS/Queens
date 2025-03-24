using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    [Serializable]
    public class Queen
    {
        private Tile _myTile;

        public Queen(Byte bytRow, Byte bytCol)
        {
            _myTile = new Tile(bytRow, bytCol);
        }
        public Boolean PositionConflict(Tile tilTest)
        {
            // same Tile, same queen, no conflict
            if (_myTile.Column == tilTest.Column)
                return false;
            // first check vertical and horizontal
            if (_myTile.Row == tilTest.Row || _myTile.Column == tilTest.Column)
                return true;
            // next check diagonal lower left to upper right
            if ((_myTile.Row - _myTile.Column) == (tilTest.Row - tilTest.Column))
                return true;
            // next check diagonal upper left to lower right
            if ((_myTile.Row + _myTile.Column) == (tilTest.Row + tilTest.Column))
                return true;

            return false;
        }
        public Boolean PositionConflict(Queen qn)
        {
            Tile tilTest = qn._myTile;
            // first check vertical and horizontal
            if (_myTile.Row == tilTest.Row || _myTile.Column == tilTest.Column)
                return true;
            // next check diagonal lower left to upper right
            if ((_myTile.Row - _myTile.Column) == (tilTest.Row - tilTest.Column))
                return true;
            // next check diagonal upper left to lower right
            if ((_myTile.Row + _myTile.Column) == (tilTest.Row + tilTest.Column))
                return true;

            return false;
        }
        public Tile BoardPosition
        {
            get { return _myTile; }
            set { _myTile = value; }
        }
        // how many queens this queen attacks
        // this does not count prior queens
        public Byte ConflictCount(List<Queen> Queens)
        {
            Byte bytConflictCount = 0;
            for (int iCol = _myTile.Column + 1; iCol < 8; iCol++)
            {
                // count how many succeding queens are in conflict
                if (PositionConflict(Queens[iCol].BoardPosition))
                    bytConflictCount++;
            }
            return bytConflictCount;
        }
        // how many queens this queen attacks
        // counts prior queens so there will be overlap
        public Byte TotalConflictCount(List<Queen> Queens)
        {
            Byte bytConflictCount = 0;
            for (int iCol = 0; iCol < 8; iCol++)
            {
                // count how many succeding queens are in conflict
                if (PositionConflict(Queens[iCol].BoardPosition))
                    bytConflictCount++;
            }
            return bytConflictCount;
        }
        // how many queens this queen does not attack
        public Byte NoConflictCount(List<Queen> Queens)
        {
            Byte bytNoConflictCount = 0;
            for (int iCol = _myTile.Column + 1; iCol < 8; iCol++)
            {
                // count how many succeding queens are in conflict
                if (!PositionConflict(Queens[iCol].BoardPosition))
                    bytNoConflictCount++;
            }
            return bytNoConflictCount;
        }
    }

}
