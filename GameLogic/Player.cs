using System;

namespace GameLogicUnit
{
    public class Player
    {
        private const int k_InitialScore = 2;
        private readonly string r_PlayerName;
        private readonly ePlayerType r_PlayerType;
        private readonly ePlayerNumber r_PlayerNumber;
        private readonly ePlayerColor r_PlayerColor;
        private int m_PlayerScore;

        public Player(string i_PlayerName, ePlayerType i_PlayerType, ePlayerNumber i_PlayerNumber, ePlayerColor i_PlayerColor, int i_PlayerScore = k_InitialScore)
        {
            r_PlayerName = i_PlayerName;
            r_PlayerType = i_PlayerType;
            r_PlayerNumber = i_PlayerNumber;
            m_PlayerScore = i_PlayerScore;
            r_PlayerColor = i_PlayerColor;
        }

        public Player(Player i_Other)
        {
            this.r_PlayerName = i_Other.r_PlayerName;
            this.r_PlayerType = i_Other.r_PlayerType;
            this.r_PlayerNumber = i_Other.r_PlayerNumber;
            this.r_PlayerColor = i_Other.r_PlayerColor;
            this.m_PlayerScore = i_Other.m_PlayerScore;
        }

        public ePlayerNumber getOpponentPlayer()
        {
            ePlayerNumber opponentPlayer = ePlayerNumber.None;

            if (r_PlayerNumber == ePlayerNumber.Player1)
            {
                opponentPlayer = ePlayerNumber.Player2;
            }
            else if (r_PlayerNumber == ePlayerNumber.Player2)
            {
                opponentPlayer = ePlayerNumber.Player1;
            }

            return opponentPlayer;
        }

        public string PlayerName
        {
            get { return r_PlayerName; }
        }

        public int PlayerScore
        {
            get { return m_PlayerScore; }
            set { m_PlayerScore = value; }
        }

        public ePlayerType PlayerType
        {
            get { return r_PlayerType; }
        }

        public ePlayerNumber PlayerNumber
        {
            get { return r_PlayerNumber; }
        }

        public ePlayerColor PlayerColor
        {
            get { return r_PlayerColor; }
        }

        public enum ePlayerType
        {
            Human = 1,
            Computer = 2
        }

        public enum ePlayerNumber
        {
            Player1 = 0,
            Player2 = 1,
            None = 2
        }

        public enum ePlayerColor
        {
            Black = 0,
            White = 1,
            None = 2
        }
    }
}
