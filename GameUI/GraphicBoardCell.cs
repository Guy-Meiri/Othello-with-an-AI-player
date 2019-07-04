using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GameUI
{
    public class GraphicBoardCell : PictureBox
    {
        private readonly int r_Column;
        private readonly int r_Row;
        private eBoardCellType m_CurrentCellType = eBoardCellType.Uninitialized;

        public GraphicBoardCell(int i_Row, int i_Column, int i_CellWidth, int i_CellHeight, eBoardCellType i_CurrentCellType)
        {
            r_Row = i_Row;
            r_Column = i_Column;
            this.Height = i_CellHeight;
            this.Width = i_CellWidth;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Enabled = false;
            SetImage(i_CurrentCellType);
        }

        public void SetImage(eBoardCellType i_BoardCellType)
        {
            if (i_BoardCellType != m_CurrentCellType)
            {
                if (this.Image != null)
                {
                    this.Image.Dispose();
                    this.Image = null;
                }

                switch (i_BoardCellType)
                {
                    case eBoardCellType.Empty:
                        this.Image = Properties.Resources.NoChip;
                        break;
                    case eBoardCellType.BlackPlayer:
                        this.Image = Properties.Resources.BlackChip;
                        break;
                    case eBoardCellType.WhitePlayer:
                        this.Image = Properties.Resources.WhiteChip;
                        break;
                    case eBoardCellType.PossibleMove:
                        this.Image = Properties.Resources.PossibleMoveImage;
                        break;
                }

                m_CurrentCellType = i_BoardCellType;
                this.Refresh();
            }
        }

        public eBoardCellType CurrentCellType
        {
            get
            {
                return m_CurrentCellType;
            }
        }

        public enum eBoardCellType
        {
            Empty = 0,
            BlackPlayer = 1,
            WhitePlayer = 2,
            PossibleMove = 3,
            Uninitialized = 4
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
    }
}
