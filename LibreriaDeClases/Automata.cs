using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaDeClases
{
    public class Automata
    {

        private Stack<Token> pilaTokens;
        private List<Symbol> ListaSimbolos;
        private Symbol simbolo;
        private Token tokenTemp;
        private Nodo nodoTemp;
        private Nodo raiz;
        
        //Enumera el nodo
        private int NumeroNodo = 0;

        public Automata(Stack<Token> pilaTokens)
        {
            this.pilaTokens = pilaTokens;            
        }

        public Nodo NotacionPolaca(List<Symbol> ListaSymbol)
        {
            Stack<Nodo> pila = new Stack<Nodo>();
            
            for (int i = 0; i < ListaSimbolos.Count; i++)
            {
                if (!ListaSimbolos[i].esOperador)
                {
                    nodoTemp = new Nodo();
                    nodoTemp.Simbolo = ListaSimbolos[i];
                    pila.Push(nodoTemp);
                }
                else if (ListaSimbolos[i].Simbolo == "|" || ListaSimbolos[i].Simbolo == ".")
                {
                   nodoTemp = new Nodo();
                   nodoTemp.Simbolo = ListaSimbolos[i];
                   Nodo nodoTemp1 = pila.Pop();
                   Nodo nodoTemp2 = pila.Pop();
                   nodoTemp.Izquierdo = nodoTemp2;
                   nodoTemp.Derecho = nodoTemp1;
                   nodoTemp.EsPadre = true;
                   pila.Push(nodoTemp);                    

                }
                else if (ListaSimbolos[i].Simbolo == "*" || ListaSimbolos[i].Simbolo == "+" || ListaSimbolos[i].Simbolo == "?")
                {
                    nodoTemp = new Nodo();
                    nodoTemp.Simbolo = ListaSimbolos[i];
                    Nodo nodoTemp1 = pila.Pop();
                    nodoTemp.Izquierdo = nodoTemp1;
                    nodoTemp.EsPadre = true;
                    pila.Push(nodoTemp);
                }
            }

            return pila.Pop();
        }

        public int Operador(string Symbol)
        {
            switch (Symbol)
            {
                case "|":
                    return 1;
                case ".":
                    return 2;
                case "*":
                    return 3;
                case "?":
                    return 3;
            }
            return -1;
        }

        public List<Symbol> InfijoPosfijo(List<Symbol> Symbol)
        {
            List<Symbol> resultado = new List<Symbol>(); 

            Stack<Symbol> pila = new Stack<Symbol>();
            int n = Symbol.Count;
            for (int i = 0; i < n; ++i)
            {
                Symbol c = new Symbol();
                c = Symbol[i];

                if (!c.esOperador)
                {
                    resultado.Add(c);
                }
                else if (c.Simbolo == "(")
                {
                    pila.Push(c);
                }
                else if(c.Simbolo == ")")
                {
                    while (pila.Count > 0 && pila.Peek().Simbolo != "(")
                    {
                        resultado.Add(pila.Pop());
                    }
                    if(pila.Count > 0 && pila.Peek().Simbolo != "(")
                    {
                        return null;
                    }
                    else
                    {
                        pila.Pop();
                    }
                }
                else
                {
                    while (pila.Count > 0 && Operador(c.Simbolo) <= Operador(pila.Peek().Simbolo))
                    {
                        resultado.Add(pila.Pop());
                    }
                    pila.Push(c);
                }
            }

            while (pila.Count > 0)
            {
                resultado.Add(pila.Pop());
            }
            return resultado;
        }

        public void Concatenar()
        {
            ListaSimbolos = new List<Symbol>();
            simbolo = new Symbol();
            simbolo.Simbolo = "(";
            simbolo.esOperador = true;
            ListaSimbolos.Add(simbolo);
            int n = pilaTokens.Count;
            for (int i = 0; i < n; i++)
            {
                tokenTemp = new Token();
                tokenTemp = pilaTokens.Pop();
                simbolo = new Symbol();
                simbolo.Simbolo = "(";
                simbolo.esOperador = true;
                ListaSimbolos.Add(simbolo);
                int contador = 0;
                foreach (Symbol symbol in tokenTemp.ListaSimbolos)
                {
                    if(!symbol.esOperador)
                    {
                        if(contador == 0)
                        {
                            ListaSimbolos.Add(symbol);
                            contador++;
                        }
                        else if(ListaSimbolos[ListaSimbolos.Count - 1].esOperador)
                        {
                            ListaSimbolos.Add(symbol);
                            contador = 0;
                            if (symbol.Simbolo == "'" && !symbol.esOperador)
                            {
                                contador = 1;
                            }
                        }
                        else if(contador == 1)
                        {
                            Symbol punto = new Symbol();
                            punto.Simbolo = ".";
                            punto.esOperador = true;
                            ListaSimbolos.Add(punto);
                            ListaSimbolos.Add(symbol);                            
                        }
                    }
                    else
                    {
                        if (symbol.Simbolo == "*" || symbol.Simbolo == "|" || symbol.Simbolo == ")")
                        {
                            ListaSimbolos.Add(symbol);
                        }
                        else
                        {
                            if (contador == 1)
                            {
                                Symbol punto = new Symbol();
                                punto.Simbolo = ".";
                                punto.esOperador = true;
                                ListaSimbolos.Add(punto);
                                ListaSimbolos.Add(symbol);
                                
                            }
                        }
                    }
                }
                simbolo = new Symbol();
                simbolo.Simbolo = ")";
                simbolo.esOperador = true;
                ListaSimbolos.Add(simbolo);
                simbolo = new Symbol();
                simbolo.Simbolo = "|";
                simbolo.esOperador = true;
                ListaSimbolos.Add(simbolo);
            }
            ListaSimbolos.Remove(ListaSimbolos[ListaSimbolos.Count - 1]);
            simbolo = new Symbol();
            simbolo.Simbolo = ")";
            simbolo.esOperador = true;
            ListaSimbolos.Add(simbolo);
            simbolo = new Symbol();
            simbolo.Simbolo = ".";
            simbolo.esOperador = true;
            ListaSimbolos.Add(simbolo);
            simbolo = new Symbol();
            simbolo.Simbolo = "#";
            ListaSimbolos.Add(simbolo);
            ListaSimbolos = InfijoPosfijo(ListaSimbolos);
            raiz = NotacionPolaca(ListaSimbolos);

        }

        public Nodo Raiz()
        {
            return raiz;
        }

        public List<Symbol> Lista()
        {
            return ListaSimbolos;
        }

        public void EnumerarHojas(Nodo raiz)
        {
            if (raiz != null)
            {
                EnumerarHojas(raiz.Izquierdo);
                EnumerarHojas(raiz.Derecho);
                if (!raiz.EsPadre)
                {
                    raiz.Num = ++NumeroNodo;
                    raiz.Last.Add(NumeroNodo);
                    raiz.First.Add(NumeroNodo);
                }
            }
        }

        public void AgregarNullable(Nodo raiz)
        {
            if (raiz == null)
            {
                return;//Fin del algoritmo
            }
            AgregarNullable(raiz.Izquierdo);
            AgregarNullable(raiz.Derecho);
            if (raiz.Simbolo.Simbolo == "*")
            {
                raiz.Nullable = true;

            }
            else if (raiz.Simbolo.Simbolo == "|")
            {
               if (raiz.Izquierdo.Nullable == true || raiz.Derecho.Nullable == true)
               {
                   raiz.Nullable = true;
               }
            }
            else if (raiz.Simbolo.Simbolo == ".")
            {
                if (raiz.Izquierdo.Nullable == true && raiz.Derecho.Nullable == true)
                {
                    raiz.Nullable = true;
                }
            }
        }

        public void AgregarFirst(Nodo raiz)
        {
            if (raiz == null)
            {
                return;
            }

            AgregarFirst(raiz.Izquierdo);
            AgregarFirst(raiz.Derecho);

            if (raiz.Simbolo.Simbolo == ".")
            {
                if (raiz.Izquierdo.Nullable == true)
                {
                    if (!(raiz.Izquierdo.Simbolo.esOperador) && !(raiz.Derecho.Simbolo.esOperador))
                    {
                        raiz.First.AddRange(raiz.Izquierdo.First);
                        raiz.First.AddRange(raiz.Derecho.First);
                    }
                    else
                    {
                        raiz.First.AddRange(raiz.Izquierdo.First);
                        raiz.First.AddRange(raiz.Derecho.First);
                    }
                }
                else
                {
                    raiz.First.AddRange(raiz.Izquierdo.First);
                }
            }
            else if (raiz.Simbolo.Simbolo == "*" && raiz.Simbolo.esOperador)
            {
                raiz.First.AddRange(raiz.Izquierdo.First);
            }
            else if (raiz.Simbolo.Simbolo == "|")
            {
               if (!(raiz.Izquierdo.Simbolo.esOperador) && !(raiz.Derecho.Simbolo.esOperador))
               {
                   raiz.First.AddRange(raiz.Izquierdo.First);
                   raiz.First.AddRange(raiz.Derecho.First);
               }
               else
               {
                   raiz.First.AddRange(raiz.Izquierdo.First);
                   raiz.First.AddRange(raiz.Derecho.First);
               }
            }
        }

        public void AgregarLast(Nodo raiz)
        {
            if (raiz == null)
            {
                return;
            }

            AgregarLast(raiz.Izquierdo);
            AgregarLast(raiz.Derecho);

            if (raiz.Simbolo.Simbolo == ".")
            {
                if (raiz.Derecho.Nullable == true)
                {
                    if (!(raiz.Izquierdo.Simbolo.esOperador) && !(raiz.Derecho.Simbolo.esOperador))
                    {
                        raiz.Last.AddRange(raiz.Izquierdo.Last);
                        raiz.Last.AddRange(raiz.Derecho.Last);
                    }
                    else
                    {
                        raiz.Last.AddRange(raiz.Izquierdo.Last);
                        raiz.Last.AddRange(raiz.Derecho.Last);
                    }
                }
                else
                {
                    raiz.Last.AddRange(raiz.Derecho.Last);
                }
            }
            else if (raiz.Simbolo.Simbolo == "*" && raiz.Simbolo.esOperador)
            {
                raiz.Last.AddRange(raiz.Izquierdo.Last);
            }
            else if (raiz.Simbolo.Simbolo == "|")
            {
                try
                {
                    if (!(raiz.Izquierdo.Simbolo.esOperador) && !(raiz.Derecho.Simbolo.esOperador))
                    {
                        raiz.Last.AddRange(raiz.Izquierdo.Last);
                        raiz.Last.AddRange(raiz.Derecho.Last);
                    }
                    else
                    {
                        raiz.Last.AddRange(raiz.Izquierdo.Last);
                        raiz.Last.AddRange(raiz.Derecho.Last);
                    }
                }
                catch
                {

                }

            }
        }

        public int NumeroNodos()
        {
            return NumeroNodo;
        }

    }
}
