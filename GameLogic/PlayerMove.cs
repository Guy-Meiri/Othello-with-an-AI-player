using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogicUnit
{
    public struct PlayerMove
    {
        private int m_Row;
        private int m_Column;
        private int m_EnemyPiecesEatenFromMove;

        public PlayerMove(int i_Row, int i_Column, int i_EnemyPiecesEatenFromMove = 0)
        {
            m_Row = i_Row;
            m_Column = i_Column;
            m_EnemyPiecesEatenFromMove = i_EnemyPiecesEatenFromMove;
        }

        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int Column
        {
            get { return m_Column; }
            set { m_Column = value; }
        }

        public int EnemyPiecesEatenFromMove
        {
            get { return m_EnemyPiecesEatenFromMove; }
            set { m_EnemyPiecesEatenFromMove = value; }
        }

        public bool IsMoveLegal()
        {
            return m_EnemyPiecesEatenFromMove > 0;
        }
    }
}
