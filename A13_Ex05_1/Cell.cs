using System;
using System.Collections.Generic;
using System.Text;

namespace A13_Ex05_1
{
    public enum eKindOfCell
    {
        Black,
        White,
        Empty,
        Wall
    }

    public class Cell
    {
        private eKindOfCell m_CellKind;

        public Cell()
        {
            // default
            m_CellKind = eKindOfCell.Empty;
        }

        public eKindOfCell CellKind
        {
            get { return m_CellKind; }
            set { m_CellKind = value; }
        }

        public void ChangeCellKind()
        {
            if(m_CellKind == eKindOfCell.White)
            {
                m_CellKind = eKindOfCell.Black;
            }
            else
            {
                if(m_CellKind == eKindOfCell.Black)
                {
                    m_CellKind = eKindOfCell.White;
                }
            }
        }
    }
}
