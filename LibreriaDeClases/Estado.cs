using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaDeClases
{
    public class Estado
    {
        public Estado(int ID)
        {
            this.ID = ID;
        }

        public int ID
        {
            get;
            set;
        }

        public List<int> nombre
        {
            get;
            set;
        }

        public Dictionary<string,Estado> Move
        {
            get;
            set;
        }

        public bool EsAceptable
        {
            get;
            set;
        }

        public bool EsMarcado
        {
            get;
            set;
        }
    }
}
