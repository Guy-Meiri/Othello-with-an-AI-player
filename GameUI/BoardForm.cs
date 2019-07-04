using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GameLogicUnit;

namespace GameUI
{
    public class BoardForm : Form
    {
        private const int k_ComputerMoveSleepTime = 500;
        private const int k_BoardCellWidth = 65;
        private const int k_BoardCellHeight = 65;
        private const int k_ScreenWidthAdjustment = 27;
        private const int k_ScreenHeightAdjustment = 36;
        private readonly int r_BoardSize;
        private readonly GameLogic r_GameLogic;
        private readonly GraphicBoardCell[,] r_BoardMatrix;

        public BoardForm(int i_BoardSize, Player.ePlayerType i_Player2Type)
        {
            r_GameLogic = new GameLogic(new GameState(i_BoardSize, i_BoardSize, "Black", "White", Player.ePlayerType.Human, i_Player2Type));
            r_BoardSize = i_BoardSize;
            r_BoardMatrix = new GraphicBoardCell[r_BoardSize, r_BoardSize];
            buildBoardCells();
            this.Width = ((i_BoardSize + 1) * k_BoardCellWidth) - ((k_BoardCellWidth / 2) + k_ScreenWidthAdjustment);
            this.Height = ((i_BoardSize + 1) * k_BoardCellHeight) - k_ScreenHeightAdjustment;
            this.Text = getCurrentGameTitle();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            r_GameLogic.ChangeToNextPlayerWithLegalMoves();
            showPossibleMoves();
        }

        private void showPossibleMoves()
        {
            foreach (PlayerMove currentMove in r_GameLogic.PossibleMovesList)
            {
                int currentRow = currentMove.Row;
                int currentColumn = currentMove.Column;
                r_BoardMatrix[currentRow, currentColumn].SetImage(GraphicBoardCell.eBoardCellType.PossibleMove);
                r_BoardMatrix[currentRow, currentColumn].Enabled = true;
            }
        }

        private void clearPossibleMoves()
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (r_BoardMatrix[i, j].CurrentCellType == GraphicBoardCell.eBoardCellType.PossibleMove)
                    {
                        r_BoardMatrix[i, j].Enabled = false;
                        r_BoardMatrix[i, j].SetImage(GraphicBoardCell.eBoardCellType.Empty);
                    }
                }
            }
        }

        public Player Player1
        {
            get
            {
                return r_GameLogic.CurrentGameState.Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return r_GameLogic.CurrentGameState.Player2;
            }
        }

        private GraphicBoardCell.eBoardCellType getBoardCellTypeFromOccupyingPlayer(Player.ePlayerNumber i_OccupyingPlayer)
        {
            GraphicBoardCell.eBoardCellType cellType = GraphicBoardCell.eBoardCellType.Empty;

            switch (i_OccupyingPlayer)
            {
                case Player.ePlayerNumber.None:
                    cellType = GraphicBoardCell.eBoardCellType.Empty;
                    break;
                case Player.ePlayerNumber.Player1:
                    cellType = GraphicBoardCell.eBoardCellType.BlackPlayer;
                    break;
                case Player.ePlayerNumber.Player2:
                    cellType = GraphicBoardCell.eBoardCellType.WhitePlayer;
                    break;
            }

            return cellType;
        }

        private void buildBoardCells()
        {
            GraphicBoardCell.eBoardCellType newCellType;

            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    newCellType = getGraphicBoardCellType(i, j);
                    r_BoardMatrix[i, j] = new GraphicBoardCell(i, j, k_BoardCellWidth, k_BoardCellHeight, newCellType);
                    r_BoardMatrix[i, j].Left = j * k_BoardCellWidth;
                    r_BoardMatrix[i, j].Top = i * k_BoardCellHeight;
                    r_BoardMatrix[i, j].Click += graphicBoardCell_Click;
                    this.Controls.Add(r_BoardMatrix[i, j]);
                    r_GameLogic.CurrentGameState.GameBoard[i, j].OccupyingPlayerChanged += boardCell_OccupyingPlayerChanged;
                }
            }
        }

        private void boardCell_OccupyingPlayerChanged(object i_Sender, EventArgs i_EventArgs)
        {
            BoardCell currentBoardCell = (BoardCell)i_Sender;
            int row = currentBoardCell.Row;
            int column = currentBoardCell.Column;

            GraphicBoardCell.eBoardCellType currentBoardCellType = getBoardCellTypeFromOccupyingPlayer(currentBoardCell.OccupyingPlayer);
            r_BoardMatrix[row, column].SetImage(currentBoardCellType);
            r_BoardMatrix[row, column].Enabled = false;
        }

        private void graphicBoardCell_Click(object sender, EventArgs e)
        {
            GraphicBoardCell currentBoardCell = sender as GraphicBoardCell;

            if (currentBoardCell != null && currentBoardCell.Enabled)
            {
                PlayerMove chosenMove = new PlayerMove(currentBoardCell.Row, currentBoardCell.Column);
                r_GameLogic.UpdateGamestate(chosenMove);
                clearPossibleMoves();
                r_GameLogic.CurrentGameState.ChangePlayerTurn();
                r_GameLogic.ChangeToNextPlayerWithLegalMoves();

                makeComputerMoveIfNeeded();

                if (!r_GameLogic.CurrentGameState.IsGameOver)
                {
                    this.Text = getCurrentGameTitle();
                    showPossibleMoves();
                }
                else
                {
                    if (!r_GameLogic.IsBoardFull())
                    {
                        MessageBox.Show("There are no possible moves left for either player. Press OK to see the final score");
                    }

                    this.Close();
                }
            }
        }

        private void makeComputerMoveIfNeeded()
        {
            while (r_GameLogic.CurrentGameState.CurrentPlayer.PlayerType == Player.ePlayerType.Computer && !r_GameLogic.CurrentGameState.IsGameOver)
            {
                this.Text = getCurrentGameTitle();
                System.Threading.Thread.Sleep(k_ComputerMoveSleepTime);
                PlayerMove chosenMove = r_GameLogic.GetComputerMove();
                r_GameLogic.UpdateGamestate(chosenMove);
                r_GameLogic.CurrentGameState.ChangePlayerTurn();
                r_GameLogic.ChangeToNextPlayerWithLegalMoves();
            }
        }

        public Player.ePlayerColor Winner
        {
            get
            {
                return r_GameLogic.CurrentGameState.GetWinningPlayer();
            }
        }

        private string getCurrentGameTitle()
        {
            return string.Format("Othello - {0}'s Turn", r_GameLogic.CurrentGameState.CurrentPlayer.PlayerColor);
        }

        private GraphicBoardCell.eBoardCellType getGraphicBoardCellType(int i_Row, int i_Column)
        {
            GraphicBoardCell.eBoardCellType newCellType = GraphicBoardCell.eBoardCellType.Empty;

            if (r_GameLogic.CurrentGameState.GameBoard[i_Row, i_Column].OccupyingPlayer == Player.ePlayerNumber.Player1)
            {
                newCellType = GraphicBoardCell.eBoardCellType.BlackPlayer;
            }
            else if (r_GameLogic.CurrentGameState.GameBoard[i_Row, i_Column].OccupyingPlayer == Player.ePlayerNumber.Player2)
            {
                newCellType = GraphicBoardCell.eBoardCellType.WhitePlayer;
            }

            return newCellType;
        }

        public enum eBoardSize
        {
            SixBySix = 6,
            EightByEight = 8,
            TenByTen = 10,
            TwelveByTwelve = 12
        }
    }
}