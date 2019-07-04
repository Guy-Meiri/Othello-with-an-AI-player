using System;
using System.Collections.Generic;

namespace GameLogicUnit
{
    public class GameLogic
    {
        private readonly List<PlayerMove> r_PossibleMovesList;
        private GameState m_CurrentGameState;

        public GameLogic(GameState i_GameState = null)
        {
            if (i_GameState != null)
            {
                m_CurrentGameState = new GameState(i_GameState);
                updatePlayersScore();
            }

            r_PossibleMovesList = new List<PlayerMove>();
        }

        public List<PlayerMove> PossibleMovesList
        {
            get { return r_PossibleMovesList; }
        }

        public void ChangeToNextPlayerWithLegalMoves()
        {
            m_CurrentGameState.NumberOfSkippedTurns = 0;
            CalculatePossibleMovesForCurrentPlayer();

            if (r_PossibleMovesList.Count == 0)
            {
                m_CurrentGameState.NumberOfSkippedTurns++;
                m_CurrentGameState.ChangePlayerTurn();
                CalculatePossibleMovesForCurrentPlayer();

                if (r_PossibleMovesList.Count == 0)
                {
                    m_CurrentGameState.NumberOfSkippedTurns++;
                    m_CurrentGameState.IsGameOver = true;
                }
            }
        }

        public PlayerMove GetComputerMove()
        {
            PlayerMove chosenMove;

            chosenMove = ArtificialIntelligence.MiniMaxChooseMove(m_CurrentGameState);

            return chosenMove;
        }

        public void UpdateGamestate(PlayerMove i_ChosenMove)
        {
            // Place the new piece on the board
            int row = i_ChosenMove.Row;
            int column = i_ChosenMove.Column;

            m_CurrentGameState.GameBoard[row, column].OccupyingPlayer = m_CurrentGameState.CurrentPlayer.PlayerNumber;

            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Up, Direction.eHorizontal.Stay);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Up, Direction.eHorizontal.Right);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Stay, Direction.eHorizontal.Right);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Down, Direction.eHorizontal.Right);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Down, Direction.eHorizontal.Stay);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Down, Direction.eHorizontal.Left);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Stay, Direction.eHorizontal.Left);
            flipSurroundedOpponentPieces(row, column, Direction.eVertical.Up, Direction.eHorizontal.Left);

            updatePlayersScore();
        }

        private void updatePlayersScore()
        {
            int player1Score = 0;
            int player2Score = 0;

            for (int i = 0; i < m_CurrentGameState.GameBoard.Rows; i++)
            {
                for (int j = 0; j < m_CurrentGameState.GameBoard.Columns; j++)
                {
                    if (m_CurrentGameState.GameBoard[i, j].OccupyingPlayer == Player.ePlayerNumber.Player1)
                    {
                        player1Score++;
                    }
                    else if (m_CurrentGameState.GameBoard[i, j].OccupyingPlayer == Player.ePlayerNumber.Player2)
                    {
                        player2Score++;
                    }
                }
            }

            m_CurrentGameState.Player1.PlayerScore = player1Score;
            m_CurrentGameState.Player2.PlayerScore = player2Score;
        }

        private void flipSurroundedOpponentPieces(int i_BoardRow, int i_BoardColumn, Direction.eVertical i_VerticalDirection, Direction.eHorizontal i_HorizontalDirection)
        {
            int nextRow = i_BoardRow + (int)i_VerticalDirection;
            int nextColumn = i_BoardColumn + (int)i_HorizontalDirection;
            Player.ePlayerNumber opponentPlayerNumber = m_CurrentGameState.CurrentPlayer.getOpponentPlayer();

            // Check if move is valid in this direction
            int opponentPiecesSurroundedInDirection = getMoveValueInDirection(i_BoardRow, i_BoardColumn, i_VerticalDirection, i_HorizontalDirection);

            if (opponentPiecesSurroundedInDirection > 0)
            {
                while (m_CurrentGameState.GameBoard[nextRow, nextColumn].OccupyingPlayer == opponentPlayerNumber)
                {
                    m_CurrentGameState.GameBoard.FlipCell(nextRow, nextColumn);
                    nextRow = nextRow + (int)i_VerticalDirection;
                    nextColumn = nextColumn + (int)i_HorizontalDirection;
                }
            }
        }

        public GameState CurrentGameState
        {
            get { return m_CurrentGameState; }
        }

        public bool IsPlayerTypeValid(int i_PlayerType)
        {
            return Enum.IsDefined(typeof(Player.ePlayerType), i_PlayerType);
        }

        public void CalculatePossibleMovesForCurrentPlayer()
        {
            r_PossibleMovesList.Clear(); // Clear the moves from the last iteration

            for (int i = 0; i < m_CurrentGameState.GameBoard.Rows; i++)
            {
                for (int j = 0; j < m_CurrentGameState.GameBoard.Columns; j++)
                {
                    if (m_CurrentGameState.GameBoard[i, j].OccupyingPlayer == Player.ePlayerNumber.None)
                    {
                        PlayerMove currentMove = getMoveData(i, j);

                        if (currentMove.IsMoveLegal())
                        {
                            r_PossibleMovesList.Add(currentMove);
                        }
                    }
                }
            }
        }

        public bool IsBoardFull()
        {
            int combinedScore = m_CurrentGameState.Player1.PlayerScore + m_CurrentGameState.Player2.PlayerScore;
            int maximalPossibleScore = m_CurrentGameState.GameBoard.Rows * m_CurrentGameState.GameBoard.Columns;

            return combinedScore == maximalPossibleScore;
        }

        private PlayerMove getMoveData(int i_BoardRow, int i_BoardColumn)
        {
            PlayerMove playerMove = new PlayerMove(i_BoardRow, i_BoardColumn);

            playerMove.EnemyPiecesEatenFromMove = getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Up, Direction.eHorizontal.Stay);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Up, Direction.eHorizontal.Right);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Stay, Direction.eHorizontal.Right);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Down, Direction.eHorizontal.Right);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Down, Direction.eHorizontal.Stay);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Down, Direction.eHorizontal.Left);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Stay, Direction.eHorizontal.Left);
            playerMove.EnemyPiecesEatenFromMove += getMoveValueInDirection(i_BoardRow, i_BoardColumn, Direction.eVertical.Up, Direction.eHorizontal.Left);

            return playerMove;
        }

        private int getMoveValueInDirection(int i_BoardRow, int i_BoardColumn, Direction.eVertical i_VerticalDirection, Direction.eHorizontal i_HorizontalDirection)
        {
            int moveValue = 0;
            int nextRow = i_BoardRow + (int)i_VerticalDirection;
            int nextColumn = i_BoardColumn + (int)i_HorizontalDirection;

            if (isLocationInBoardRange(nextRow, nextColumn))
            {
                if (isLocationOccupiedByAnOpponent(nextRow, nextColumn))
                {
                    moveValue = countSequenceOfEatableOpponentPieces(nextRow, nextColumn, i_VerticalDirection, i_HorizontalDirection);
                }
            }

            return moveValue;
        }

        private int countSequenceOfEatableOpponentPieces(int i_BoardRow, int i_BoardColumn, Direction.eVertical i_VerticalDirection, Direction.eHorizontal i_HorizontalDirection)
        {
            int numberOfEatenOpponentPieces = 0;
            int nextRow = i_BoardRow;
            int nextColumn = i_BoardColumn;
            Player.ePlayerNumber opponentPlayer = m_CurrentGameState.CurrentPlayer.getOpponentPlayer();

            while (isLocationInBoardRange(nextRow, nextColumn) && opponentPlayer == m_CurrentGameState.GameBoard[nextRow, nextColumn].OccupyingPlayer)
            {
                numberOfEatenOpponentPieces++;
                nextRow = nextRow + (int)i_VerticalDirection;
                nextColumn = nextColumn + (int)i_HorizontalDirection;
            }

            if (!isLocationInBoardRange(nextRow, nextColumn) || m_CurrentGameState.GameBoard[nextRow, nextColumn].OccupyingPlayer == Player.ePlayerNumber.None)
            {
                numberOfEatenOpponentPieces = 0;
            }

            return numberOfEatenOpponentPieces;
        }

        private bool isLocationInBoardRange(int i_BoardRow, int i_BoardColumn)
        {
            bool isLocationInBoard = true;

            if (i_BoardRow < 0 || i_BoardRow >= m_CurrentGameState.GameBoard.Rows)
            {
                isLocationInBoard = false;
            }
            else if (i_BoardColumn < 0 || i_BoardColumn >= m_CurrentGameState.GameBoard.Columns)
            {
                isLocationInBoard = false;
            }

            return isLocationInBoard;
        }

        private bool isLocationOccupiedByAnOpponent(int i_BoardRow, int i_BoardColumn)
        {
            return m_CurrentGameState.GameBoard[i_BoardRow, i_BoardColumn].OccupyingPlayer == m_CurrentGameState.CurrentPlayer.getOpponentPlayer();
        }

        private class Direction
        {
            public enum eHorizontal
            {
                Left = -1,
                Right = 1,
                Stay = 0
            }

            public enum eVertical
            {
                Up = -1,
                Down = 1,
                Stay = 0
            }
        }
    }
}
