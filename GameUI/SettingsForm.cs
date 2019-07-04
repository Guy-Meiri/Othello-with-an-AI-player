using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using GameLogicUnit;

namespace GameUI
{
    public class SettingsForm : Form
    {
        private Button m_ButtonBoardSize;
        private Button m_ButtonPlayAgainstComputer;
        private Button m_ButtonPlayAgainstFriend;
        private BoardForm.eBoardSize m_BoardSize = BoardForm.eBoardSize.SixBySix;
        private int m_CurrentBoardSizeIndex = 0;
        private Player.ePlayerType m_Player2Type = Player.ePlayerType.Human;

        public SettingsForm()
        {
            InitializeComponent();
        }

        public Player.ePlayerType Player2Type
        {
            get
            {
                return m_Player2Type;
            }
        }

        public BoardForm.eBoardSize BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        private void InitializeComponent()
        {
            this.m_ButtonBoardSize = new Button();
            this.m_ButtonPlayAgainstComputer = new Button();
            this.m_ButtonPlayAgainstFriend = new Button();
            this.SuspendLayout();
            this.StartPosition = FormStartPosition.CenterScreen;

            // buttonBoardSize
            this.m_ButtonBoardSize.Anchor = (AnchorStyles)(AnchorStyles.Top
            | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right);
            this.m_ButtonBoardSize.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.m_ButtonBoardSize.Location = new Point(21, 23);
            this.m_ButtonBoardSize.Name = "buttonBoardSize";
            this.m_ButtonBoardSize.Size = new Size(328, 50);
            this.m_ButtonBoardSize.TabIndex = 0;
            this.m_ButtonBoardSize.Text = "Board Size: 6x6 (click to increase)";
            this.m_ButtonBoardSize.UseVisualStyleBackColor = true;
            this.m_ButtonBoardSize.Click += new EventHandler(this.buttonBoardSize_Click);

            // buttonPlayAgainstComputer
            this.m_ButtonPlayAgainstComputer.Anchor = (AnchorStyles)(AnchorStyles.Top
            | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right);
            this.m_ButtonPlayAgainstComputer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.m_ButtonPlayAgainstComputer.Location = new Point(21, 103);
            this.m_ButtonPlayAgainstComputer.Name = "buttonPlayAgainstComputer";
            this.m_ButtonPlayAgainstComputer.Size = new Size(157, 50);
            this.m_ButtonPlayAgainstComputer.TabIndex = 1;
            this.m_ButtonPlayAgainstComputer.Text = "Play against the computer";
            this.m_ButtonPlayAgainstComputer.UseVisualStyleBackColor = true;
            this.m_ButtonPlayAgainstComputer.Click += new EventHandler(this.buttonPlayAgainstComputer_Click);

            // buttonPlayAgainstFriend
            this.m_ButtonPlayAgainstFriend.Anchor = (AnchorStyles)(AnchorStyles.Top
            | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right);
            this.m_ButtonPlayAgainstFriend.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.m_ButtonPlayAgainstFriend.Location = new Point(192, 103);
            this.m_ButtonPlayAgainstFriend.Name = "buttonPlayAgainstFriend";
            this.m_ButtonPlayAgainstFriend.Size = new Size(157, 50);
            this.m_ButtonPlayAgainstFriend.TabIndex = 2;
            this.m_ButtonPlayAgainstFriend.Text = "Play against your friend";
            this.m_ButtonPlayAgainstFriend.UseVisualStyleBackColor = true;
            this.m_ButtonPlayAgainstFriend.Click += new EventHandler(this.buttonPlayAgainstFriend_Click);

            // settingForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(365, 173);
            this.Controls.Add(this.m_ButtonPlayAgainstFriend);
            this.Controls.Add(this.m_ButtonPlayAgainstComputer);
            this.Controls.Add(this.m_ButtonBoardSize);
            this.MaximizeBox = false;
            this.Name = "settingForm";
            this.Text = "Othello - Game Settings";
            this.ResumeLayout(false);
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            Array boardSizeArr = Enum.GetValues(typeof(BoardForm.eBoardSize));

            m_CurrentBoardSizeIndex = (m_CurrentBoardSizeIndex + 1) % boardSizeArr.Length;
            m_BoardSize = (BoardForm.eBoardSize)boardSizeArr.GetValue(m_CurrentBoardSizeIndex);
            m_ButtonBoardSize.Text = string.Format("Board Size: {0}x{0} (click to increase)", (int)m_BoardSize);
        }

        private void buttonPlayAgainstComputer_Click(object sender, EventArgs e)
        {
            m_Player2Type = Player.ePlayerType.Computer;
            this.Close();
        }

        private void buttonPlayAgainstFriend_Click(object sender, EventArgs e)
        {
            m_Player2Type = Player.ePlayerType.Human;
            this.Close();
        }
    }
}