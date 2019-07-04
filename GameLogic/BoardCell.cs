using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogicUnit
{
    public class BoardCell
    {
        private readonly int r_Row;
        private readonly int r_Column;
        private Player.ePlayerNumber m_OccupyingPlayer;

        public event EventHandler OccupyingPlayerChanged;

        public BoardCell(int i_Row, int i_Column, Player.ePlayerNumber i_OccupyingPlayer = Player.ePlayerNumber.None)
        {
            r_Row = i_Row;
            r_Column = i_Column;
            m_OccupyingPlayer = i_OccupyingPlayer;
        }

        public BoardCell(BoardCell i_Other)
        {
            this.r_Row = i_Other.r_Row;
            this.r_Column = i_Other.r_Column;
            this.m_OccupyingPlayer = i_Other.m_OccupyingPlayer;
        }

        public void FlipOccupyingPlayer()
        {
            if (m_OccupyingPlayer != Player.ePlayerNumber.None)
            {
                if (m_OccupyingPlayer == Player.ePlayerNumber.Player1)
                {
                    OccupyingPlayer = Player.ePlayerNumber.Player2;
                }
                else if (m_OccupyingPlayer == Player.ePlayerNumber.Player2)
                {
                    OccupyingPlayer = Player.ePlayerNumber.Player1;
                }
            }
        }

        public int Row
        {
            get
            {
                return r_Row;
            }
        }

        public int Column
        {
            get
            {
                return r_Column;
            }
        }

        public Player.ePlayerNumber OccupyingPlayer
        {
            get
            {
                return m_OccupyingPlayer;
            }

            set
            {
                m_OccupyingPlayer = value;
                onOccupyingPlayerChange();
            }
        }

        private void onOccupyingPlayerChange()
        {
            if (OccupyingPlayerChanged != null)
            {
                OccupyingPlayerChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public bool IsEmpty()
        {
            return m_OccupyingPlayer == Player.ePlayerNumber.None;
        }
    }
}
