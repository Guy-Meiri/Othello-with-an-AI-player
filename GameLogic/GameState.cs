using System;

namespace GameLogicUnit
{
    public class GameState
    {
        private const int k_NumOfPlayers = 2;
        private Board m_GameBoard;
        private Player[] m_PlayersArray;
        private int m_CurrentPlayerIndex;
        private bool m_IsGameOver;
        private int m_NumberOfSkippedTurns;

        public GameState(int i_RowsInBoard, int i_ColumnsInBoard, string i_Player1Name, string i_Player2Name, Player.ePlayerType i_Player1Type, Player.ePlayerType i_Player2Type)
        {
            m_GameBoard = new Board(i_RowsInBoard, i_ColumnsInBoard);
            m_PlayersArray = new Player[k_NumOfPlayers];
            m_PlayersArray[0] = new Player(i_Player1Name, i_Player1Type, Player.ePlayerNumber.Player1, Player.ePlayerColor.Black);
            m_PlayersArray[1] = new Player(i_Player2Name, i_Player2Type, Player.ePlayerNumber.Player2, Player.ePlayerColor.White);
            m_IsGameOver = false;
            m_CurrentPlayerIndex = 0;
            m_NumberOfSkippedTurns = 0;
        }

        public GameState(GameState i_Other)
        {
            this.m_CurrentPlayerIndex = i_Other.m_CurrentPlayerIndex;
            this.m_GameBoard = new Board(i_Other.m_GameBoard);
            this.m_IsGameOver = i_Other.m_IsGameOver;
            this.m_NumberOfSkippedTurns = i_Other.m_NumberOfSkippedTurns;
            this.m_CurrentPlayerIndex = i_Other.m_CurrentPlayerIndex;
            this.m_PlayersArray = new Player[k_NumOfPlayers];
            this.m_PlayersArray[0] = new Player(i_Other.m_PlayersArray[0]);
            this.m_PlayersArray[1] = new Player(i_Other.m_PlayersArray[1]);
        }

        public Player.ePlayerColor GetWinningPlayer()
        {
            Player.ePlayerColor winningPlayer = Player.ePlayerColor.None;

            if (Player1.PlayerScore > Player2.PlayerScore)
            {
                winningPlayer = Player.ePlayerColor.Black;
            }
            else if (Player1.PlayerScore < Player2.PlayerScore)
            {
                winningPlayer = Player.ePlayerColor.White;
            }

            return winningPlayer;
        }

        public void ChangePlayerTurn()
        {
            m_CurrentPlayerIndex = (m_CurrentPlayerIndex + 1) % k_NumOfPlayers;
        }

        public Player CurrentPlayer
        {
            get { return m_PlayersArray[m_CurrentPlayerIndex]; }
        }

        public Player OpponentPlayer
        {
            get { return m_PlayersArray[(m_CurrentPlayerIndex + 1) % 2]; }
        }

        public bool IsGameOver
        {
            get { return m_IsGameOver; }
            set { m_IsGameOver = value; }
        }

        public Board GameBoard
        {
            get { return m_GameBoard; }
        }

        public Player Player1
        {
            get { return m_PlayersArray[0]; }
        }

        public Player Player2
        {
            get { return m_PlayersArray[1]; }
        }

        public int NumOfPlayers
        {
            get { return k_NumOfPlayers; }
        }

        public int NumberOfSkippedTurns
        {
            get { return m_NumberOfSkippedTurns; }
            set { m_NumberOfSkippedTurns = value; }
        }

        public int EvaluatePlayerScoreDifference()
        {
            return m_PlayersArray[1].PlayerScore - m_PlayersArray[0].PlayerScore;
        }
    }
}
