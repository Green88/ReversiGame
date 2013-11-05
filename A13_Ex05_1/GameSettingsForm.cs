using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace A13_Ex05_1
{
    public class GameSettingsForm : Form
    {
        private const int k_StartingBoardSize = 6;
        private const int k_MaxBoardSize = 14;
        private const int k_DistanceFromBorder = 20;
        private const int k_ButtonsHeight = 50;
        private Button m_ButtonBoardSize;
        private Button m_ButtonPlayAgainstComputer;
        private Button m_ButtonPlayAgainstFriend;
        private ePlayerType m_PlayerType;
        private int m_BoardSize;

        public GameSettingsForm()
        {
            m_BoardSize = k_StartingBoardSize;
            this.Size = new Size(400, 200);
            this.Text = "Reversi - Game Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            m_ButtonBoardSize = new Button();
            m_ButtonBoardSize.Text = string.Format("Board Size: {0}x{0} (click to increase)", k_StartingBoardSize);
            m_ButtonBoardSize.Width = this.Width - (2 * k_DistanceFromBorder);
            m_ButtonBoardSize.Height = k_ButtonsHeight;
            m_ButtonBoardSize.Location = new Point(this.Location.X + k_DistanceFromBorder, this.Location.Y + k_DistanceFromBorder);
            m_ButtonBoardSize.Click += new EventHandler(buttonBoardSize_Click);

            m_ButtonPlayAgainstComputer = new Button();
            m_ButtonPlayAgainstComputer.Text = "Play against the computer";
            m_ButtonPlayAgainstComputer.Width = (this.Width / 2) - (2 * k_DistanceFromBorder);
            m_ButtonPlayAgainstComputer.Height = k_ButtonsHeight;
            m_ButtonPlayAgainstComputer.Location = new Point(m_ButtonBoardSize.Location.X, m_ButtonBoardSize.Location.Y + k_ButtonsHeight + k_DistanceFromBorder);
            m_ButtonPlayAgainstComputer.Click += new EventHandler(buttonGameOpponent_Click);

            m_ButtonPlayAgainstFriend = new Button();
            m_ButtonPlayAgainstFriend.Text = "Play against your friend";
            m_ButtonPlayAgainstFriend.Width = (this.Width / 2) - (2 * k_DistanceFromBorder);
            m_ButtonPlayAgainstFriend.Height = k_ButtonsHeight;
            m_ButtonPlayAgainstFriend.Location = new Point(m_ButtonBoardSize.Location.X + m_ButtonPlayAgainstFriend.Width + (2 * k_DistanceFromBorder), m_ButtonPlayAgainstComputer.Location.Y);
            m_ButtonPlayAgainstFriend.Click += new EventHandler(buttonGameOpponent_Click);

            this.Controls.AddRange(new Control[] { m_ButtonBoardSize, m_ButtonPlayAgainstComputer, m_ButtonPlayAgainstFriend });
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            string buttonText;

            if (m_BoardSize < k_MaxBoardSize)
            {
                buttonText = string.Format("Board Size: {0}x{0} (click to increase)", m_BoardSize + 2);
                m_ButtonBoardSize.Text = buttonText;
                m_BoardSize += 2;
            }
            else
            {
                buttonText = string.Format("Board Size: {0}x{0} (click to increase)", k_StartingBoardSize);
                m_ButtonBoardSize.Text = buttonText;
                m_BoardSize = k_StartingBoardSize;
            }
        }

        private void buttonGameOpponent_Click(object sender, EventArgs e)
        {
            if (sender == m_ButtonPlayAgainstComputer)
            {
                m_PlayerType = ePlayerType.Computer;
            }
            else
            {
                m_PlayerType = ePlayerType.Human;
            }

            this.Close();
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
        }
    }
}
