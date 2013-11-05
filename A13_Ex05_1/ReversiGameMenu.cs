using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace A13_Ex05_1
{
    public class ReversiGameMenu
    {
        private Player m_BlackPlayer;
        private Player m_WhitePlayer;
        private int m_BoardSize;
        private ReversiGameManager m_ReversiGameManager;
        private GameSettingsForm m_SettingsForm;
        private ReversiGameForm m_GameForm;

        public ReversiGameMenu()
        {
            Application.EnableVisualStyles();
            m_BlackPlayer = new Player();
            m_SettingsForm = new GameSettingsForm();
            m_SettingsForm.ShowDialog();
            m_BoardSize = m_SettingsForm.BoardSize + 2;
            m_WhitePlayer = new Player(m_SettingsForm.PlayerType, eKindOfCell.White);
            m_ReversiGameManager = new ReversiGameManager(m_BlackPlayer, m_WhitePlayer, m_BoardSize);
        }

        public void ReversiGameLoop()
        {
            bool wantToContinueGame = true;
            string messageForPlayers = string.Empty;
            int blackWins = 0;
            int whiteWins = 0;

            while (wantToContinueGame)
            {
                m_GameForm = new ReversiGameForm(m_ReversiGameManager);
                m_GameForm.ShowDialog();

                if (m_BlackPlayer.PlayerScore > m_WhitePlayer.PlayerScore)
                {
                    blackWins++;
                }
                else if (m_WhitePlayer.PlayerScore > m_BlackPlayer.PlayerScore)
                {
                    whiteWins++;
                }
                else
                {
                    blackWins++;
                    whiteWins++;
                }

                messageForPlayers = string.Format(
                    "{0} won!! ({1}/{2}) ({3}/{4}){5} Would you like another round?",
                    m_ReversiGameManager.WhoIsAWinner(),
                    m_BlackPlayer.PlayerScore,
                    m_WhitePlayer.PlayerScore,
                    blackWins,
                    whiteWins,
                    Environment.NewLine);

                if (MessageBox.Show(messageForPlayers, "Reversi", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    wantToContinueGame = false;
                }
                else
                {
                    m_ReversiGameManager.RestartGame();
                } 
            }
        }
    }
}
