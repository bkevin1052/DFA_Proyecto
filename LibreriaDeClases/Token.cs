﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaDeClases
{
    public class Token
    {

        public Token()
        {
            ListaSimbolos = new List<Symbol>();
        }

        public List<Symbol> ListaSimbolos
        {
            get;
            set;
        }

        public int num
        {
            get;
            set;
        }
    }
}
