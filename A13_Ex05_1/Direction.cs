using System;
using System.Collections.Generic;
using System.Text;

namespace A13_Ex05_1
{
    public struct Direction
    {
        private int m_IDirection;
        private int m_JDirection;

        public Direction(int i_IDirection, int i_JDirection)
        {
            m_IDirection = i_IDirection;
            m_JDirection = i_JDirection;
        }

        public int ISideDirection
        {
            get { return m_IDirection; }
            set { m_IDirection = value; }
        }

        public int JSideDirection
        {
            get { return m_JDirection; }
            set { m_JDirection = value; }
        }
    }
}
