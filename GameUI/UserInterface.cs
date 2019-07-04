using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GameLogicUnit;

namespace GameUI
{
    public class UserInterface
    {
        private SettingsForm m_SettingsForm;
        private BoardForm m_BoardForm;
        private int m_BlackPlayerGamesWon = 0;
        private int m_WhitePlayerGamesWon = 0;

        public void RunGame()
        {
            bool isPlayAgain = false;

            do
            {
                runSingleGame();
                isPlayAgain = askUserToPlayAgain();
            }
            while (isPlayAgain);
        }

        private void runSingleGame()
        {
            m_SettingsForm = new SettingsForm();
            m_SettingsForm.ShowDialog();
            m_BoardForm = new BoardForm((int)m_SettingsForm.BoardSize, m_SettingsForm.Player2Type);
            m_BoardForm.ShowDialog();
            updateGamesWon(m_BoardForm.Winner);
        }

        private void updateGamesWon(Player.ePlayerColor i_WinningPlayer)
        {
            if (i_WinningPlayer == Player.ePlayerColor.Black)
            {
                m_BlackPlayerGamesWon++;
            }
            else if (i_WinningPlayer == Player.ePlayerColor.White)
            {
                m_WhitePlayerGamesWon++;
            }
        }

        private bool askUserToPlayAgain()
        {
            bool isPlayAgain = false;
            string message = string.Format(
@"{0} Won!! ({1}/{2}) ({3}/{4})
Would you like another round?",
                m_BoardForm.Winner,
                m_BoardForm.Player1.PlayerScore,
                m_BoardForm.Player2.PlayerScore,
                m_BlackPlayerGamesWon,
                m_WhitePlayerGamesWon);

            DialogResult playAgainDialogResult = MessageBox.Show(message, "Othello", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (playAgainDialogResult == DialogResult.Yes)
            {
                isPlayAgain = true;
            }

            return isPlayAgain;
        }
    }
}
