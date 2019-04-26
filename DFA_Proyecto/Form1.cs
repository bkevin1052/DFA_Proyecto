using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LibreriaDeClases;
using System.Data;

namespace DFA_Proyecto
{
    public partial class Form1 : Form
    {
        //VARIABLES GLOBALES
        OpenFileDialog open;
        StreamReader lecturaArchivo;
        string abecedarioMayus = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", abecedarioMinus = "abcdefghijklmnopqrstuvwxyz", digitos = "0123456789", espacio = "_";
        string linea;  
       
        
        //SETS
        string inicioMayus = string.Empty, finMayus = string.Empty, temp = string.Empty, inicioMin = string.Empty,
        finMin = string.Empty, inicioEs = string.Empty, finEs = string.Empty, inicioChar = string.Empty, finChar = string.Empty;
        Dictionary<string, Set> sets;
        Set SetTemp;

        bool setsLeido = false;

        //TOKENS
        Stack<Token> pilaTokens;
        Token nuevoToken;
        Symbol simboloToken;

        //ACTIONS
        Dictionary<int, LibreriaDeClases.Action> actions;
        LibreriaDeClases.Action ActionTemp;
        int llaves;
        string[] action;

        //ERROR
        Error ErrorTemp;
        Dictionary<string, Error> error;

        //AUTOMATA
        Automata automata;
        private Dictionary<int, List<int>> Follow;
        private int IDEstado = 0;
        List<Estado> estados;
        List<string> ListaSinDuplicados;

        //ESCRITURA
        string escritura;
        StreamWriter escribir;
        List<Symbol> simbolos;

        public Form1()
        {
            InitializeComponent();
            open = new OpenFileDialog();
        }

        private void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            open.DefaultExt = ".txt";
            open.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtRuta.Text = open.FileName;
                lecturaArchivo = new StreamReader(open.FileName);
                sets = new Dictionary<string, Set>();
                pilaTokens = new Stack<Token>();
                actions = new Dictionary<int, LibreriaDeClases.Action>();
                error = new Dictionary<string, Error>();
                //Inicio de lectura
                LecturaSets();
                AgregarAutomata();
            }
        }

        /// <summary>
        /// LECTURA SETS
        /// </summary>
        private void LecturaSets()
        {
            try
            {
                int x, y;
                while ((linea = lecturaArchivo.ReadLine()) != null && linea != "")
                {
                    linea = linea.TrimEnd().TrimStart();
                    if (linea.ToUpper() == "TOKENS")
                    {
                        LecturaTokens();
                    }
                    if (linea.ToUpper() == "SETS" || setsLeido != true)
                    {
                        setsLeido = true;
                    }
                    else
                    {
                        //LECTURA DEL DICCIONARIO LETRA
                        if (linea.ToUpper().Contains("LETRA"))
                        {
                            if (linea.Contains("="))
                            {
                                linea = linea.TrimEnd().TrimStart();
                                for (int i = 0; i < abecedarioMayus.Length; i++)
                                {
                                    if (linea.TrimEnd(' ').Contains(("'" + abecedarioMayus.ToCharArray()[i] + "'").ToString()))
                                    {
                                        inicioMayus = temp;
                                        temp = abecedarioMayus.ToCharArray()[i].ToString();
                                        if (inicioMayus != null)
                                        {
                                            finMayus = abecedarioMayus.ToCharArray()[i].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < abecedarioMinus.Length; i++)
                                {
                                    if (linea.TrimEnd(' ').Contains(("'" + abecedarioMinus.ToCharArray()[i] + "'").ToString()))
                                    {
                                        inicioMin = temp;
                                        temp = abecedarioMinus.ToCharArray()[i].ToString();
                                        if (inicioMin != null)
                                        {
                                            finMin = abecedarioMinus.ToCharArray()[i].ToString();
                                        }
                                    }
                                }

                                for (int i = 0; i < espacio.Length; i++)
                                {
                                    if (linea.TrimEnd(' ').Contains(("'" + espacio.ToCharArray()[i] + "'").ToString()))
                                    {
                                        inicioEs = temp;
                                        temp = espacio.ToCharArray()[i].ToString();
                                        if (inicioEs != null)
                                        {
                                            finEs = espacio.ToCharArray()[i].ToString();
                                        }
                                    }
                                }

                                //Creando set LETRA
                                SetTemp = new Set();

                                if (inicioMayus != "" && finMayus != "")
                                {
                                    x = abecedarioMayus.IndexOf(inicioMayus);
                                    y = abecedarioMayus.LastIndexOf(finMayus);
                                    SetTemp.set.AddRange(abecedarioMayus.Substring(x, y - x + 1).ToList());
                                }


                                if (inicioMin != "" && finMin != "")
                                {
                                    x = abecedarioMinus.IndexOf(inicioMin);
                                    y = abecedarioMinus.LastIndexOf(finMin);
                                    SetTemp.set.AddRange(abecedarioMinus.Substring(x, y - x + 1).ToList());
                                }

                                if (inicioEs != "" && finEs != "")
                                {
                                    x = espacio.IndexOf(inicioEs);
                                    y = espacio.IndexOf(finEs);
                                    SetTemp.set.Add(Convert.ToChar(finEs));
                                }

                                sets.Add("LETRA", SetTemp);
                            }
                        }
                        //LECTURA DEL DICCIONARIO DE DIGITO
                        else if (linea.Contains("DIGITO"))
                        {
                            if (linea.Contains("="))
                            {
                                linea = linea.TrimEnd().TrimStart();
                                for (int i = 0; i < digitos.Length; i++)
                                {
                                    if (linea.TrimEnd(' ').Contains(("'" + digitos.ToCharArray()[i] + "'").ToString()))
                                    {
                                        inicioMayus = temp;
                                        temp = digitos.ToCharArray()[i].ToString();
                                        if (inicioMayus != null)
                                        {
                                            finMayus = digitos.ToCharArray()[i].ToString();
                                        }
                                    }
                                }

                                //Creando set de Digitos
                                SetTemp = new Set();

                                if (inicioMayus != "" && finMayus != "")
                                {
                                    x = digitos.IndexOf(inicioMayus);
                                    y = digitos.LastIndexOf(finMayus);
                                    SetTemp.set.AddRange(digitos.Substring(x, y - x + 1).ToList());
                                }

                                sets.Add("DIGITO", SetTemp);

                            }
                        }
                        //LECTURA DEL DICCIONARIO CHARSET
                        else if (linea.Contains("CHARSET"))
                        {
                            int contador = 0;
                            linea = linea.TrimEnd().TrimStart();
                            if (linea.Contains("="))
                            {
                                while (linea.Contains("=") && contador != 2)
                                {
                                    if (linea.TrimEnd(' ').Contains("chr(") || linea.TrimEnd(' ').Contains("CHR("))
                                    {
                                        for (int i = 0; i < 255; i++)
                                        {
                                            if (linea.Contains("(" + i + ")"))
                                            {
                                                inicioChar = temp;
                                                temp = i.ToString();
                                                if (inicioChar != null)
                                                {
                                                    finChar = temp.ToString();
                                                    contador++;
                                                }
                                            }
                                        }
                                        SetTemp = new Set();

                                        if (contador == 2)
                                        {
                                            for (int i = int.Parse(inicioChar); i < int.Parse(finChar); i++)
                                            {
                                                SetTemp.set.Add((char)i);
                                            }
                                            sets.Add("CHARSET", SetTemp);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show(linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// LECTURA TOKENS
        /// </summary>
        private void LecturaTokens()
        {
            try
            {                
                char[] charLinea;
                linea = lecturaArchivo.ReadLine();
                while (linea != null && linea.ToUpper() != "ACTIONS" && linea != "")
                {
                    linea = linea.TrimEnd().TrimStart();
                    if (linea.ToUpper() == "ACTIONS")
                    {
                        LecturaActions();
                    }
                    if (linea.ToUpper().Contains("TOKEN"))
                    {
                        if (linea.Contains("="))
                        {
                            charLinea = linea.ToCharArray();
                            linea = linea.Remove(0, RetornarIgual(charLinea) + 1);
                            int num = RetornarNum(charLinea);
                            linea = linea.TrimEnd().TrimStart();
                            charLinea = linea.ToCharArray();

                            int contadorComillas = 0;
                            string simbolo = "";
                            nuevoToken = new Token();
                            nuevoToken.num = num;

                            foreach (char c in charLinea)
                            {
                                if (c == '\'')
                                {
                                    if(contadorComillas == 0)
                                    {
                                        contadorComillas++;
                                    }
                                    else
                                    {
                                        simboloToken = new Symbol();
                                        if (simbolo == "")
                                        {
                                            simbolo = "\'";
                                        }
                                        simboloToken.Simbolo = simbolo;
                                        nuevoToken.ListaSimbolos.Add(simboloToken);
                                        contadorComillas = 0;
                                        simbolo = "";
                                    }
                                }else if (c == '*' || c == '|' || c == '(' || c == ')' || c == '?' )
                                {
                                    if (contadorComillas == 1)
                                    {
                                        simbolo += c;
                                    }
                                    else
                                    {
                                        simboloToken = new Symbol();
                                        simboloToken.Simbolo = c.ToString();
                                        simboloToken.esOperador = true;
                                        nuevoToken.ListaSimbolos.Add(simboloToken);
                                    }
                                }else if (c == ' ')
                                {
                                    if (contadorComillas == 1)
                                    {
                                        simbolo += c;
                                    }
                                    else if(simbolo.Length > 0)
                                    {
                                        simboloToken = new Symbol();
                                        simboloToken.Simbolo = simbolo;
                                        nuevoToken.ListaSimbolos.Add(simboloToken);
                                        simbolo = "";
                                    }
                                }
                                else
                                {
                                    simbolo += c;
                                }
                            }
                            pilaTokens.Push(nuevoToken);
                        }
                    }
                    linea = lecturaArchivo.ReadLine();
                }
                LecturaActions();
            }
            catch(Exception e)
            {
                MessageBox.Show( e + linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Metodo para retornar un entero donde se encuentra el signo =
        /// </summary>
        /// <param name="linea"></param>
        /// <returns></returns>
        private int RetornarNum(char[] linea)
        {
            for (int i = 0; i < linea.Length; i++)
            {                
                if (char.IsNumber(linea[i]) && char.IsNumber(linea[i+1]))
                {
                    return Convert.ToInt16(linea[i].ToString() + linea[i+1].ToString());
                }
                else if (char.IsDigit(linea[i]))
                {
                    return Convert.ToInt16(linea[i].ToString());
                }
            }
            return 0;
        }

        private int RetornarIgual(char[] linea)
        {
            for (int i = 0; i < linea.Length; i++)
            {
                if (linea[i].Equals('='))
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// LECTURA ACTIONS
        /// </summary>
        private void LecturaActions()
        {
            try
            {
                while ((linea = lecturaArchivo.ReadLine()) != null && !linea.ToUpper().Contains("ERROR") && linea != "")
                {
                    linea = linea.TrimEnd().TrimStart();
                    if (linea.ToUpper().Contains("ERROR"))
                    {
                        LecturaError();                      
                    }

                    if (linea.Trim().TrimEnd().TrimStart() == "RESERVADAS()")
                    {
                        continue;
                    }
                    else if (linea.Trim().TrimEnd().TrimStart() == "{")
                    {
                        llaves++;
                    }
                    else if (linea.Trim().TrimEnd().TrimStart() == "}")
                    {
                        llaves++;
                    }
                    else if (linea.Contains("\'"))
                    {
                        action = linea.TrimEnd().TrimStart().Split('\'');
                        ActionTemp = new LibreriaDeClases.Action();
                        ActionTemp.Valor = action[1];
                        actions.Add(int.Parse(action[0].Substring(0, 2)), ActionTemp);
                    }
                }
                LecturaError();
                CrearAutomata();
            }
            catch(Exception e)
            {
                MessageBox.Show(linea + " " + e, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// LECTURA ERROR
        /// </summary>
        private void LecturaError()
        {
            try
            {
                string[] ArregloLinea;
                ArregloLinea = linea.Split('=');
                ErrorTemp = new Error();
                ErrorTemp.Valor = int.Parse(ArregloLinea[1]);
                error.Add("ERROR", ErrorTemp);
            }
            catch
            {
                MessageBox.Show(linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private void AgregarAutomata()
        {
            Nodo raiz = new Nodo();
            raiz = automata.Raiz();
            simbolos = new List<Symbol>();
            RecorrerArbol(raiz);
            ListaSinDuplicados = simbolos.Select(x => x.Simbolo.TrimEnd().TrimStart()).Distinct().ToList();
            int contador = 0;
            foreach(string x in ListaSinDuplicados)
            {
                if(x == "#")
                {
                    continue;
                }
                automataGrid.Columns.Add(contador.ToString(), x);
                contador++;
            }

            estados = new List<Estado>();
            Estado q0 = new Estado(IDEstado);
            q0.nombre = raiz.First;
            q0.ID = automataGrid.Rows.Add();
            //inicio de automata
            if (q0.nombre.Contains(Follow.Count))
            {
                q0.EsAceptable = true;
            }
            estados.Clear();
            estados.Add(q0);         
        }

        private void RecorrerArbol(Nodo raiz)
        {
            if (raiz == null)
            {
                return;
            }

            RecorrerArbol(raiz.Izquierdo);
            RecorrerArbol(raiz.Derecho);
            if(raiz.Derecho == null && raiz.Izquierdo == null)
            {
                simbolos.Add(raiz.Simbolo);
            }
        }

        /// <summary>
        /// Metodo para generar un automata
        /// </summary>
        private void CrearAutomata()
        {
            automata = new Automata(pilaTokens);
            automata.Concatenar();
            Nodo raiz = automata.Raiz();
            automata.EnumerarHojas(raiz);
            raiz = automata.Raiz();
            automata.AgregarNullable(raiz);
            raiz = automata.Raiz();
            automata.AgregarFirst(raiz);
            raiz = automata.Raiz();
            automata.AgregarLast(raiz);
            raiz = automata.Raiz();
            Follow = new Dictionary<int, List<int>>();
            for (int i = 1; i <= automata.NumeroNodos(); i++)
            {
                Follow.Add(i, new List<int>());
            }
            AgregarFollow(raiz);
            ArchivoFollows();

            if (!File.Exists(@"C:\Users\kevin\Desktop\follows.txt"))
            {
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\follows.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo follows.txt se creo correctamente C:\Users\kevin\Desktop\follows.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                File.Delete(@"C:\Users\kevin\Desktop\follows.txt");
                //File.Create(@"C:\Users\kevin\Desktop\follows.txt");
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\follows.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo follows.txt se creo correctamente C:\Users\kevin\Desktop\follows.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            escritura = "";

            if (!File.Exists(@"C:\Users\kevin\Desktop\arbol.txt"))
            {
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\arbol.txt");
                ArchivoArbol(raiz);
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo arbol.txt se creo correctamente C:\Users\kevin\Desktop\arbol.txt", "Informacion",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                File.Delete(@"C:\Users\kevin\Desktop\arbol.txt");
                //File.Create(@"C:\Users\kevin\Desktop\follows.txt");
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\arbol.txt");
                ArchivoArbol(raiz);
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo arbol.txt se creo correctamente C:\Users\kevin\Desktop\arbol.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            escritura = "";

            ArchivoSets();

            escritura = "";

            ArchivoActions();

            escritura = "";

            ArchivoError();
        }

        private void AgregarFollow(Nodo raiz)
        {            
            if(raiz == null)
            {
                return;
            }

            AgregarFollow(raiz.Izquierdo);
            AgregarFollow(raiz.Derecho);

            if (raiz.Simbolo.Simbolo == ".")
            {
                int[] lastpos_c1 = raiz.Izquierdo.Last.ToArray();
                List<int> firstpos_c2 = raiz.Derecho.First;
                for (int i = 0; i < lastpos_c1.Length; i++)
                {
                    Follow[lastpos_c1[i]].AddRange(firstpos_c2);
                }
            }
            else if (raiz.Simbolo.Simbolo == "*")
            {
                int[] lastpos_n = raiz.Last.ToArray();
                List<int> firstpos_n = raiz.First;
                for (int i = 0; i < lastpos_n.Length; i++)
                {
                    Follow[lastpos_n[i]].AddRange(firstpos_n);
                }
            }
        }

        private void ArchivoFollows()
        {
            escritura = "";            

            for (int i = 1; i <= Follow.Keys.Count; i++)
            {
                escritura += Environment.NewLine;
                escritura += "NODO: " + i;
                escritura += Environment.NewLine;
                escritura +=  "FOLLOW: ";
                escritura += Environment.NewLine;

                for (int j = 0; j < Follow[i].Count; j++)
                {
                    escritura += Follow[i][j] + ", ";
                }
                escritura += Environment.NewLine;
            }
        }

        private void ArchivoArbol(Nodo raiz)
        {
            if(raiz == null)
            {
                return;
            }

            ArchivoArbol(raiz.Izquierdo);
            ArchivoArbol(raiz.Derecho);

            escritura += Environment.NewLine;
            escritura += "NODO: " + raiz.Simbolo.Simbolo;
            escritura += Environment.NewLine;
            escritura += "FIRST: ";
            escritura += Environment.NewLine;

            for (int i = 0; i < raiz.First.Count; i++)
            {
                escritura += raiz.First[i] + ", ";
            }
            escritura += Environment.NewLine;
            escritura += "LAST: ";
            escritura += Environment.NewLine;
            for (int i = 0; i < raiz.Last.Count; i++)
            {
                escritura += raiz.Last[i] + ", ";
            }
            escritura += Environment.NewLine;
            escritura += "NULABILIDAD: ";
            escritura += Environment.NewLine;
            escritura += raiz.Nullable.ToString();
            escritura += Environment.NewLine;

        }

        private void ArchivoSets()
        {
            foreach (var set in sets)
            {
                string nombre = set.Key;
                Set Lista = set.Value;
                escritura += Environment.NewLine;
                escritura += nombre + " = " ;

                for (int i = 0; i < Lista.set.Count; i++)
                {
                    escritura += Lista.set[i];
                }

                escritura += Environment.NewLine;
            }

            if (!File.Exists(@"C:\Users\kevin\Desktop\sets.txt"))
            {
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\sets.txt");                
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo sets.txt se creo correctamente  C:\Users\kevin\Desktop\sets.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                File.Delete(@"C:\Users\kevin\Desktop\sets.txt");
                //File.Create(@"C:\Users\kevin\Desktop\follows.txt");
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\sets.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo sets.txt se creo correctamente  C:\Users\kevin\Desktop\sets.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ArchivoActions()
        {
            foreach (var ac in actions)
            {
                int nombre = ac.Key;
                LibreriaDeClases.Action Lista = ac.Value;
                escritura += Environment.NewLine;
                escritura += nombre + " = ";
                escritura += Lista.Valor;
                escritura += Environment.NewLine;
            }

            if (!File.Exists(@"C:\Users\kevin\Desktop\actions.txt"))
            {
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\actions.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo actions.txt se creo correctamente  C:\Users\kevin\Desktop\actions.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                File.Delete(@"C:\Users\kevin\Desktop\actions.txt");
                //File.Create(@"C:\Users\kevin\Desktop\follows.txt");
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\actions.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo actions.txt se creo correctamente C:\Users\kevin\Desktop\actions.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ArchivoError()
        {
            foreach (var error in error)
            {
                string e = error.Key;
                Error er = error.Value;

                escritura += Environment.NewLine;
                escritura += e + " = " + er.Valor;
                escritura += Environment.NewLine;
            }


            if (!File.Exists(@"C:\Users\kevin\Desktop\error.txt"))
            {
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\error.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo error.txt se creo correctamente  C:\Users\kevin\Desktop\error.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                File.Delete(@"C:\Users\kevin\Desktop\error.txt");
                //File.Create(@"C:\Users\kevin\Desktop\follows.txt");
                escribir = new StreamWriter(@"C:\Users\kevin\Desktop\error.txt");
                escribir.Write(escritura);
                escribir.Close();
                MessageBox.Show(@"El Archivo error.txt se creo correctamente  C:\Users\kevin\Desktop\error.txt", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
