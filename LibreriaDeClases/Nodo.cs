using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaDeClases
{
    public class Nodo
    {
        private string Valor;
        private Nodo Padre;
        private Nodo Izquierdo;
        private Nodo Derecho;

        private List<int> FirstPos;
        private List<int> LastPos;
        private bool Nullable;
        private int Num;

        public Nodo(string valor)
        {
            this.Valor = valor;
            Padre = null;
            Izquierdo = null;
            Derecho = null;
            FirstPos = new List<int>();
            LastPos = new List<int>();
            Nullable = false;
            Num = 0;
        }
        public int getNum()
        {
            return Num;
        }

        public void setNum(int num)
        {
            this.Num = num;
        }

        public string getValor()
        {
            return Valor;
        }
        
        public void setValor(string valor)
        {
            this.Valor = valor;
        }

        public Nodo getPadre()
        {
            return Padre;
        }
        
        public void setPadre(Nodo padre)
        {
            this.Padre = padre;
        }
        
        public Nodo getIzquierdo()
        {
            return Izquierdo;
        }
        
        public void setIzquierdo(Nodo izquierdo)
        {
            this.Izquierdo = izquierdo;
        }
        
        public Nodo getDerecho()
        {
            return Derecho;
        }
        
        public void setDerecho(Nodo derecho)
        {
            this.Derecho = derecho;
        }

        public void addToFirstPos(int number)
        {
            FirstPos.Add(number);
        }
        public void addAllToFirstPos(List<int> set)
        {
            FirstPos.AddRange(set);
        }

        public void addToLastPos(int number)
        {
            LastPos.Add(number);
        }
        public void addAllToLastPos(List<int> set)
        {
            LastPos.AddRange(set);
        }
        
        public List<int> getFirstPos()
        {
            return FirstPos;
        }
        
        public List<int> getLastPos()
        {
            return LastPos;
        }
        
        public bool EsNullable()
        {
            return Nullable;
        }
        
        public void setNullable(bool nullable)
        {
            this.Nullable = nullable;
        }
    }
    }
}
