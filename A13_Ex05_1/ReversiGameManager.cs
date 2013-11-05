using System;
using System.Collections.Generic;
using System.Text;

namespace A13_Ex05_1
{
    public delegate void NoAvailableMovesMessageDelegate(string i_Message);

    public class ReversiGameManager
    {
        private const bool k_TurnOfBlacks = true;
        private static Random s_RandomCell = new Random();
        private Player m_BlackPlayer;
        private Player m_WhitePlayer;
        private Cell[,] m_BoardMatrix;
        private int m_BoardSize;
        private bool m_TurnInPlay;

        public event NoAvailableMovesMessageDelegate m_NoAvailableMovesMessage;

        public ReversiGameManager(Player i_BlackPlayer, Player i_WhitePlayer, int i_BoardSize)
        {
            m_BlackPlayer = i_BlackPlayer;
            m_WhitePlayer = i_WhitePlayer;
            m_BoardSize = i_BoardSize;
            m_BoardMatrix = new Cell[m_BoardSize, m_BoardSize];
            m_TurnInPlay = k_TurnOfBlacks;
            initializeMatrix();
            initiateGame();
        }

        public bool TurnOfBlacks
        {
            get
            {
                bool blacksTurn = false;
                if (m_TurnInPlay == k_TurnOfBlacks)
                {
                    blacksTurn = true;
                }

                return blacksTurn;
            }
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public Cell[,] BoardMatrix
        {
            get { return m_BoardMatrix; }
        }

        public Player WhitePlayer
        {
            get { return m_WhitePlayer; }
        }

        public Player BlackPlayer
        {
            get { return m_BlackPlayer; }
        }

        public Player WhoPlaysNext()
        {
            Player playsNext;

            if (m_TurnInPlay == k_TurnOfBlacks)
            {
                playsNext = m_BlackPlayer;
            }
            else
            {
                playsNext = m_WhitePlayer;
            }

            return playsNext;
        }

        private void initiateGame()
        {
            UpdateAvailableMovesList(m_BlackPlayer);
            UpdateAvailableMovesList(m_WhitePlayer);
        }

        private void initializeMatrix()
        {
            int middleOfBoard = m_BoardSize / 2;

            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    m_BoardMatrix[i, j] = new Cell();
                }
            }

            for (int i = 0; i < m_BoardSize; i++)
            {
                m_BoardMatrix[i, 0].CellKind = eKindOfCell.Wall;
                m_BoardMatrix[i, m_BoardSize - 1].CellKind = eKindOfCell.Wall;
            }

            for (int i = 0; i < m_BoardSize; i++)
            {
                m_BoardMatrix[0, i].CellKind = eKindOfCell.Wall;
                m_BoardMatrix[m_BoardSize - 1, i].CellKind = eKindOfCell.Wall;
            }

            m_BoardMatrix[middleOfBoard - 1, middleOfBoard - 1].CellKind = eKindOfCell.Black;
            m_BoardMatrix[middleOfBoard, middleOfBoard].CellKind = eKindOfCell.Black;
            m_BoardMatrix[middleOfBoard - 1, middleOfBoard].CellKind = eKindOfCell.White;
            m_BoardMatrix[middleOfBoard, middleOfBoard - 1].CellKind = eKindOfCell.White;
        }

        private void cleanMatrix()
        {
            int middleOfBoard = m_BoardSize / 2;

            for (int i = 1; i < m_BoardSize - 1; i++)
            {
                for (int j = 1; j < m_BoardSize - 1; j++)
                {
                    m_BoardMatrix[i, j].CellKind = eKindOfCell.Empty;
                }
            }

            m_BoardMatrix[middleOfBoard - 1, middleOfBoard - 1].CellKind = eKindOfCell.Black;
            m_BoardMatrix[middleOfBoard, middleOfBoard].CellKind = eKindOfCell.Black;
            m_BoardMatrix[middleOfBoard - 1, middleOfBoard].CellKind = eKindOfCell.White;
            m_BoardMatrix[middleOfBoard, middleOfBoard - 1].CellKind = eKindOfCell.White;
        }

        public void RestartGame()
        {
            cleanMatrix();
            m_BlackPlayer.PlayerScore = 0;
            m_WhitePlayer.PlayerScore = 0;
            UpdateAvailableMovesList(m_BlackPlayer);
            UpdateAvailableMovesList(m_WhitePlayer);
        }

        private bool checkSkipOfTurnOrEndOfGame(Player i_Player)
        {
            bool isEndOfGame = false;
            string message;
            if (isBoardFull())
            {
                isEndOfGame = true;
            }
            else
            {
                if (!areThereAvailableMovesForAnyPlayer())
                {
                    message = "No moves at all!";
                    ShowMessage(message);
                    isEndOfGame = true;
                }
                else
                {
                    if (!areThereAvailableMovesForPlayer(i_Player))
                    {
                        if (i_Player.PlayerType == ePlayerType.Human)
                        {
                            message = string.Format("There are no available moves for {0}, skip this move", i_Player.PlayerName);
                            ShowMessage(message);
                        }
                        else if (i_Player.PlayerType == ePlayerType.Computer)
                        {
                            message = string.Format("There are no available moves for {0}, it will skip this move", i_Player.PlayerName);
                            ShowMessage(message);
                        }

                        m_TurnInPlay = !m_TurnInPlay;
                    }
                }
            }

            return isEndOfGame;
        }

        public void ShowMessage(string i_Message)
        {
            if (m_NoAvailableMovesMessage != null)
            {
                m_NoAvailableMovesMessage.Invoke(i_Message);
            }
        }

        public bool PlayMoveFromBoard(int i_Row, int i_Column, Player i_Player)
        {
            Player nextPlayer;
            bool isEndOfGame;
            PickedCell pickedCell = new PickedCell(i_Row, i_Column);

            updateBoardAfterAMove(pickedCell, i_Player);
            m_TurnInPlay = !m_TurnInPlay;
            nextPlayer = WhoPlaysNext();

            isEndOfGame = checkSkipOfTurnOrEndOfGame(nextPlayer);

            if (isEndOfGame == false)
            {
                nextPlayer = WhoPlaysNext();
                if (nextPlayer.PlayerType == ePlayerType.Computer)
                {
                    pickedCell = chooseCellRandomaly(nextPlayer);
                    updateBoardAfterAMove(pickedCell, nextPlayer);
                    m_TurnInPlay = !m_TurnInPlay;
                    nextPlayer = WhoPlaysNext();

                    if (isBoardFull() || !areThereAvailableMovesForAnyPlayer())
                    {
                        isEndOfGame = true;
                        PlayersScoreWhenGameEnded();
                    }
                    else if (!areThereAvailableMovesForPlayer(nextPlayer))
                    {
                        m_TurnInPlay = !m_TurnInPlay;
                    }
                }
            }
            else
            {
                PlayersScoreWhenGameEnded();
            }

            return isEndOfGame;
        }

        private void updateBoardAfterAMove(PickedCell i_CellToUpdate, Player i_Player)
        {
            eKindOfCell playersColor = i_Player.DiskColor;
            List<Direction> foundDirections;

            foundDirections = findFoeNeighbourDirection(playersColor, i_CellToUpdate.PickARow, i_CellToUpdate.PickAColumn);
            flipFoeDisks(foundDirections, i_CellToUpdate.PickARow, i_CellToUpdate.PickAColumn, playersColor);
            m_BoardMatrix[i_CellToUpdate.PickARow, i_CellToUpdate.PickAColumn].CellKind = i_Player.DiskColor;

            UpdateAvailableMovesList(m_BlackPlayer);
            UpdateAvailableMovesList(m_WhitePlayer);
        }

        private void flipFoeDisks(List<Direction> i_AvailableDirections, int i_ICoordinate, int i_JCoordinate, eKindOfCell i_CellColor)
        {
            List<Direction> flipDirections = new List<Direction>();

            foreach (Direction val in i_AvailableDirections)
            {
                int iDistance = 0, jDistance = 0;
                Cell checkCell = m_BoardMatrix[i_ICoordinate + val.ISideDirection, i_JCoordinate + val.JSideDirection];

                iDistance += val.ISideDirection;
                jDistance += val.JSideDirection;

                while (checkCell.CellKind == foeColor(i_CellColor))
                {
                    checkCell = m_BoardMatrix[i_ICoordinate + iDistance + val.ISideDirection, i_JCoordinate + jDistance + val.JSideDirection];
                    if (checkCell.CellKind == i_CellColor)
                    {
                        flipDirections.Add(val);
                    }

                    iDistance += val.ISideDirection;
                    jDistance += val.JSideDirection;
                }
            }

            foreach (Direction val in flipDirections)
            {
                int iDistance = 0;
                int jDistance = 0;
                Cell checkCell = m_BoardMatrix[i_ICoordinate + val.ISideDirection, i_JCoordinate + val.JSideDirection];

                iDistance += val.ISideDirection;
                jDistance += val.JSideDirection;

                while (checkCell.CellKind == foeColor(i_CellColor))
                {
                    m_BoardMatrix[i_ICoordinate + iDistance, i_JCoordinate + jDistance].ChangeCellKind();
                    checkCell = m_BoardMatrix[i_ICoordinate + val.ISideDirection + iDistance, i_JCoordinate + val.JSideDirection + jDistance];
                    iDistance += val.ISideDirection;
                    jDistance += val.JSideDirection;
                }
            }
        }

        public void UpdateAvailableMovesList(Player i_Player)
        {
            eKindOfCell playersColor = i_Player.DiskColor;
            List<Direction> foundDirection;
            bool isMoveAvailable;
            PickedCell foundCell = new PickedCell(0, 0);

            if (i_Player.AvailableMoves.Count != 0)
            {
                i_Player.ClearAvailableMovesList();
            }

            for (int i = 1; i < m_BoardMatrix.GetLength(1) - 1; i++)
            {
                for (int j = 1; j < m_BoardMatrix.GetLength(1) - 1; j++)
                {
                    if (m_BoardMatrix[i, j].CellKind == eKindOfCell.Empty)
                    {
                        // list of neighbours, potential for move
                        foundDirection = findFoeNeighbourDirection(playersColor, i, j);
                        if (foundDirection.Count != 0)
                        {
                            isMoveAvailable = findIfThereIsAMove(foundDirection, i, j, playersColor);
                            if (isMoveAvailable)
                            {
                                foundCell.PickARow = i;
                                foundCell.PickAColumn = j;
                                i_Player.SetNewAvailableMove(foundCell);
                            }
                        }
                    }
                }
            }
        }

        private List<Direction> findFoeNeighbourDirection(eKindOfCell i_CellColor, int i_ICoordinate, int i_JCoordinate)
        {
            List<Direction> directionsList = new List<Direction>();
            Direction findNeighbour = new Direction(0, 0);

            // up-left
            if (m_BoardMatrix[i_ICoordinate - 1, i_JCoordinate - 1].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = -1;
                findNeighbour.JSideDirection = -1;
                directionsList.Add(findNeighbour);
            }

            // up
            if (m_BoardMatrix[i_ICoordinate - 1, i_JCoordinate].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = -1;
                findNeighbour.JSideDirection = 0;
                directionsList.Add(findNeighbour);
            }

            // up-right
            if (m_BoardMatrix[i_ICoordinate - 1, i_JCoordinate + 1].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = -1;
                findNeighbour.JSideDirection = 1;
                directionsList.Add(findNeighbour);
            }

            // right
            if (m_BoardMatrix[i_ICoordinate, i_JCoordinate + 1].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = 0;
                findNeighbour.JSideDirection = 1;
                directionsList.Add(findNeighbour);
            }

            // down-right
            if (m_BoardMatrix[i_ICoordinate + 1, i_JCoordinate + 1].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = 1;
                findNeighbour.JSideDirection = 1;
                directionsList.Add(findNeighbour);
            }

            // down
            if (m_BoardMatrix[i_ICoordinate + 1, i_JCoordinate].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = 1;
                findNeighbour.JSideDirection = 0;
                directionsList.Add(findNeighbour);
            }

            // down-left
            if (m_BoardMatrix[i_ICoordinate + 1, i_JCoordinate - 1].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = 1;
                findNeighbour.JSideDirection = -1;
                directionsList.Add(findNeighbour);
            }

            // down-left
            if (m_BoardMatrix[i_ICoordinate, i_JCoordinate - 1].CellKind == foeColor(i_CellColor))
            {
                findNeighbour.ISideDirection = 0;
                findNeighbour.JSideDirection = -1;
                directionsList.Add(findNeighbour);
            }

            return directionsList;
        }

        private bool findIfThereIsAMove(List<Direction> i_DirectionList, int i_ICoordinate, int i_JCoordinate, eKindOfCell i_CellColor)
        {
            bool isThereAMove = false;

            foreach (Direction val in i_DirectionList)
            {
                int iDistance = 0, jDistance = 0;
                Cell checkCell = m_BoardMatrix[i_ICoordinate + val.ISideDirection, i_JCoordinate + val.JSideDirection];

                iDistance += val.ISideDirection;
                jDistance += val.JSideDirection;

                while (checkCell.CellKind == foeColor(i_CellColor))
                {
                    checkCell = m_BoardMatrix[i_ICoordinate + iDistance + val.ISideDirection, i_JCoordinate + jDistance + val.JSideDirection];
                    if (checkCell.CellKind == i_CellColor)
                    {
                        isThereAMove = true;
                    }

                    iDistance += val.ISideDirection;
                    jDistance += val.JSideDirection;
                }
            }

            return isThereAMove;
        }

        private PickedCell chooseCellRandomaly(Player i_Player)
        {
            PickedCell randomCell;
            int randomIndex;

            randomIndex = s_RandomCell.Next(i_Player.AvailableMoves.Count);
            randomCell = i_Player.AvailableMoves[randomIndex];
            return randomCell;
        }

        private eKindOfCell foeColor(eKindOfCell i_PlayersColor)
        {
            eKindOfCell foeColor = eKindOfCell.Empty;

            if (i_PlayersColor == eKindOfCell.Black)
            {
                foeColor = eKindOfCell.White;
            }
            else
            {
                if (i_PlayersColor == eKindOfCell.White)
                {
                    foeColor = eKindOfCell.Black;
                }
            }

            return foeColor;
        }

        private bool isBoardFull()
        {
            bool isBoardFull = true;

            foreach (Cell val in m_BoardMatrix)
            {
                if (val.CellKind == eKindOfCell.Empty)
                {
                    isBoardFull = false;
                    break;
                }
            }

            return isBoardFull;
        }

        private bool areThereAvailableMovesForAnyPlayer()
        {
            bool movesAvailable = true;

            if (!areThereAvailableMovesForPlayer(m_BlackPlayer) && !areThereAvailableMovesForPlayer(m_WhitePlayer))
            {
                movesAvailable = false;
            }

            return movesAvailable;
        }

        private bool areThereAvailableMovesForPlayer(Player i_Player)
        {
            bool areAvailable = false;

            if (i_Player.AvailableMoves.Count != 0)
            {
                areAvailable = true;
            }

            return areAvailable;
        }

        public string WhoIsAWinner()
        {
            string winner;

            if (m_BlackPlayer.PlayerScore > m_WhitePlayer.PlayerScore)
            {
                winner = m_BlackPlayer.PlayerName;
            }
            else
            {
                if (m_BlackPlayer.PlayerScore < m_WhitePlayer.PlayerScore)
                {
                    winner = m_WhitePlayer.PlayerName;
                }
                else
                {
                    winner = "Nobody";
                }
            }

            return winner;
        }

        public void PlayersScoreWhenGameEnded()
        {
            int blacksCount = 0;
            int whitesCount = 0;

            foreach (Cell val in m_BoardMatrix)
            {
                if (val.CellKind == eKindOfCell.Black)
                {
                    blacksCount++;
                }

                if (val.CellKind == eKindOfCell.White)
                {
                    whitesCount++;
                }
            }

            m_BlackPlayer.PlayerScore = blacksCount;
            m_WhitePlayer.PlayerScore = whitesCount;
        }
    }
}
