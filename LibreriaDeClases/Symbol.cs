using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaDeClases
{
    public class Symbol
    {
        public Symbol()
        {
            esOperador = false;
        }

        public string Simbolo
        {
            get;
            set;
        }

        public bool esOperador
        {
            get;
            set;
        }
    }
}
