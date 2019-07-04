using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogicUnit
{
    public class Board
    {
        private readonly int r_Rows;
        private readonly int r_Columns;
        private readonly BoardCell[,] r_Board = null;

        public Board(int i_RowsInBoard, int i_ColumnsInBoard)
        {
            this.r_Rows = i_RowsInBoard;
            this.r_Columns = i_ColumnsInBoard;
            r_Board = new BoardCell[r_Rows, r_Columns];
            initializeBoard();
        }

        public Board(Board i_Other)
        {
            this.r_Rows = i_Other.r_Rows;
            this.r_Columns = i_Other.r_Columns;
            this.r_Board = new BoardCell[r_Rows, r_Columns];
            copyBoardCells(i_Other);
        }

        private void initializeBoard()
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Columns; j++)
                {
                    r_Board[i, j] = new BoardCell(i, j, Player.ePlayerNumber.None);
                }
            }

            placeInitialPiecesOnBoard();
        }

        private void copyBoardCells(Board i_Other)
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Columns; j++)
                {
                    r_Board[i, j] = new BoardCell(i_Other[i, j]);
                }
            }
        }

        private void placeInitialPiecesOnBoard()
        {
            r_Board[(r_Rows / 2) - 1, (r_Columns / 2) - 1].OccupyingPlayer = Player.ePlayerNumber.Player2;
            r_Board[r_Rows / 2, r_Columns / 2].OccupyingPlayer = Player.ePlayerNumber.Player2;
            r_Board[(r_Rows / 2) - 1, r_Columns / 2].OccupyingPlayer = Player.ePlayerNumber.Player1;
            r_Board[r_Rows / 2, (r_Columns / 2) - 1].OccupyingPlayer = Player.ePlayerNumber.Player1;
        }

        public void FlipCell(int i_Row, int i_Column)
        {
            r_Board[i_Row, i_Column].FlipOccupyingPlayer();
        }

        public int Rows
        {
            get { return r_Rows; }
        }

        public int Columns
        {
            get { return r_Columns; }
        }

        public BoardCell this[int i_Row, int i_Column]
        {
            get
            {
                return r_Board[i_Row, i_Column];
            }

            set
            {
                r_Board[i_Row, i_Column] = value;
            }
        }

        public bool IsCellInCorners(int i_Row, int i_Column)
        {
            bool isTopLeftCorner = i_Row == 0 && i_Column == 0;
            bool isTopRightCorner = i_Row == 0 && i_Column == r_Columns - 1;
            bool isBottomLeftCorner = i_Row == r_Rows - 1 && i_Column == 0;
            bool isBottomRightCorner = i_Row == r_Rows - 1 && i_Column == r_Columns - 1;

            return isTopLeftCorner || isTopRightCorner || isBottomLeftCorner || isBottomRightCorner;
        }
    }
}
