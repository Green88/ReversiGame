using System;
using System.Collections.Generic;
using System.Text;

namespace A13_Ex05_1
{
    public enum ePlayerType
    {
        Human = 1,
        Computer = 2
    }

    public struct PickedCell
    {
        private int m_Row;
        private int m_Column;

        public PickedCell(int i_Row, int i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public int PickARow
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int PickAColumn
        {
            get { return m_Column; }
            set { m_Column = value; }
        }
    }

    public class Player
    {
        private string m_PlayerName;
        private ePlayerType m_PlayerType;
        private eKindOfCell m_DiskColor;
        private int m_Score;
        private List<PickedCell> m_AvailableMovesForPlayer;

        public Player()
        {
            m_PlayerName = "Black Player";
            m_PlayerType = ePlayerType.Human;
            m_Score = 0;
            m_DiskColor = eKindOfCell.Black;
            m_AvailableMovesForPlayer = new List<PickedCell>();
        }

        public Player(ePlayerType i_PlayerType, eKindOfCell i_DiskColor)
        {
            m_PlayerType = i_PlayerType;
            m_DiskColor = i_DiskColor;
            m_Score = 0;
            m_AvailableMovesForPlayer = new List<PickedCell>();
            if (PlayerType == ePlayerType.Human)
            {
                m_PlayerName = string.Format("{0} Player", DiskColor.ToString());
            }
            else
            {
                m_PlayerName = "Computer";
            }  
        }

        public string PlayerName
        {
            get { return m_PlayerName; }
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
            set { m_PlayerType = value; }
        }

        public eKindOfCell DiskColor
        {
            get { return m_DiskColor; }
            set { m_DiskColor = value; }
        }

        public int PlayerScore
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public List<PickedCell> AvailableMoves
        {
            get { return m_AvailableMovesForPlayer; }
        }

        public void SetNewAvailableMove(PickedCell i_Cell)
        {
            m_AvailableMovesForPlayer.Add(i_Cell);
        }

        public void ClearAvailableMovesList()
        {
            m_AvailableMovesForPlayer.Clear();
        }
    }
}
