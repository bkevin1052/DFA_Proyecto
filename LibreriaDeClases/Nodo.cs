using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaDeClases
{
    public class Nodo
    {

        public Nodo()
        {
            First = new List<int>();
            Last = new List<int>();
            Nullable = false;
            EsPadre = false;
            Num = 0;
        }

        public bool EsPadre
        {
            get;
            set;
        }

        public int Num
        {
            get;
            set;
        }

        public Nodo Izquierdo
        {
            get;
            set;
        }

        public Nodo Derecho
        {
            get;
            set;
        }

        public Symbol Simbolo
        {
            get;
            set;
        }

        public List<int> First
        {
            get;
            set;
        }

        public List<int> Last
        {
            get;
            set;
        }

        public bool Nullable
        {
            get;
            set;
        }
    }
}
