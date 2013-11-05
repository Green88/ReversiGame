using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace A13_Ex05_1
{
    public class ReversiGameForm : Form
    {
        private const int k_SizeOfButton = 60;
        private const int k_DistanceFromBorder = 15;
        private const string k_ButtonText = "O";
        private int m_ButtonsBoardSize;
        private ReversiGameManager m_GameManager;
        private Button[,] m_ButtonsBoardMatrix;

        public ReversiGameForm(ReversiGameManager i_GameManager)
        {
            m_GameManager = i_GameManager;
            m_ButtonsBoardSize = m_GameManager.BoardSize - 2;
            m_GameManager.m_NoAvailableMovesMessage += this.NoAvailableMovesMessage;
            this.Size = new Size((2 * k_DistanceFromBorder) + (k_SizeOfButton * m_ButtonsBoardSize), (3 * k_DistanceFromBorder) + (k_SizeOfButton * m_ButtonsBoardSize));
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Text = "Reversi Game";
            m_ButtonsBoardMatrix = new Button[m_ButtonsBoardSize, m_ButtonsBoardSize];
            initiateButtonsBoardMatrix();
        }

        private void initiateButtonsBoardMatrix()
        {
            int distanceFronPreviousButtonOnX = 15;
            int distanceFromPreviousButtonOnY = 5;

            for (int i = 0; i < m_ButtonsBoardSize; i++)
            {
                for (int j = 0; j < m_ButtonsBoardSize; j++)
                {
                    m_ButtonsBoardMatrix[i, j] = new Button();
                    setButtonsInformation(i, j);
                    m_ButtonsBoardMatrix[i, j].Width = k_SizeOfButton;
                    m_ButtonsBoardMatrix[i, j].Height = k_SizeOfButton;
                    m_ButtonsBoardMatrix[i, j].Location = new Point(this.Location.X + distanceFronPreviousButtonOnX, this.Location.Y + distanceFromPreviousButtonOnY);
                    m_ButtonsBoardMatrix[i, j].Click += new EventHandler(buttonFromBoardMatrix_Click);
                    distanceFronPreviousButtonOnX += k_SizeOfButton;
                    this.Controls.Add(m_ButtonsBoardMatrix[i, j]);
                }

                distanceFronPreviousButtonOnX = 15;
                distanceFromPreviousButtonOnY += k_SizeOfButton;
            }
        }

        private void setButtonsInformation(int i_Row, int i_Column)
        {
            m_ButtonsBoardMatrix[i_Row, i_Column].Enabled = false;
            m_ButtonsBoardMatrix[i_Row, i_Column].BackColor = Color.Beige;
            if (m_GameManager.BoardMatrix[i_Row + 1, i_Column + 1].CellKind == eKindOfCell.Black)
            {
                m_ButtonsBoardMatrix[i_Row, i_Column].BackColor = Color.Black;
                m_ButtonsBoardMatrix[i_Row, i_Column].ForeColor = Color.White;
                m_ButtonsBoardMatrix[i_Row, i_Column].Text = k_ButtonText;
                m_ButtonsBoardMatrix[i_Row, i_Column].Font = new Font("Arial", 14);
            }
            else if (m_GameManager.BoardMatrix[i_Row + 1, i_Column + 1].CellKind == eKindOfCell.White)
            {
                m_ButtonsBoardMatrix[i_Row, i_Column].BackColor = Color.White;
                m_ButtonsBoardMatrix[i_Row, i_Column].ForeColor = Color.Black;
                m_ButtonsBoardMatrix[i_Row, i_Column].Text = k_ButtonText;
                m_ButtonsBoardMatrix[i_Row, i_Column].Font = new Font("Arial", 14);
            }
            else
            {
                if (m_GameManager.TurnOfBlacks == true)
                {
                    if (m_GameManager.BlackPlayer.AvailableMoves.Contains(new PickedCell(i_Row + 1, i_Column + 1)))
                    {
                        m_ButtonsBoardMatrix[i_Row, i_Column].BackColor = Color.DarkGray;
                        m_ButtonsBoardMatrix[i_Row, i_Column].Enabled = true;
                    }
                }
                else
                {
                    if (m_GameManager.WhitePlayer.AvailableMoves.Contains(new PickedCell(i_Row + 1, i_Column + 1)))
                    {
                        m_ButtonsBoardMatrix[i_Row, i_Column].BackColor = Color.DarkGray;
                        m_ButtonsBoardMatrix[i_Row, i_Column].Enabled = true;
                    }
                }
            }
        }

        private void showUpdatedBoardMatrix()
        {
            for (int i = 0; i < m_ButtonsBoardSize; i++)
            {
                for (int j = 0; j < m_ButtonsBoardSize; j++)
                {
                    setButtonsInformation(i, j);
                }
            }
        }

        private void buttonFromBoardMatrix_Click(object sender, EventArgs e)
        {
            int row = 0;
            int column = 0;
            bool isEndOfGame;

            for (int i = 0; i < m_ButtonsBoardSize; i++)
            {
                for (int j = 0; j < m_ButtonsBoardSize; j++)
                {
                    if (this.m_ButtonsBoardMatrix[i, j] == sender as Button)
                    {
                        row = i + 1;
                        column = j + 1;
                    }
                }
            }

            if (m_GameManager.TurnOfBlacks == true)
            {
                isEndOfGame = m_GameManager.PlayMoveFromBoard(row, column, m_GameManager.BlackPlayer);
            }
            else
            {
                isEndOfGame = m_GameManager.PlayMoveFromBoard(row, column, m_GameManager.WhitePlayer);
            }

            showUpdatedBoardMatrix();

            if (isEndOfGame == true)
            {
                NoAvailableMovesMessage("Game over!");
                this.Close();
            }
        }

        private void NoAvailableMovesMessage(string i_MessageForUserAboutMoves)
        {
            MessageBox.Show(i_MessageForUserAboutMoves, "Reversi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            m_GameManager.m_NoAvailableMovesMessage -= this.NoAvailableMovesMessage;  
        }
    }
}
