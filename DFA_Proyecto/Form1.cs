using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibreriaDeClases;

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

        public Form1()
        {
            InitializeComponent();
            open = new OpenFileDialog();
            sets = new Dictionary<string,Set>();
            pilaTokens = new Stack<Token>();
            actions = new Dictionary<int, LibreriaDeClases.Action>();
            error = new Dictionary<string, Error>();
        }

        private void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            open.DefaultExt = ".txt";
            open.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtRuta.Text = open.FileName;
                lecturaArchivo = new StreamReader(open.FileName);

                //Inicio de lectura
                LecturaSets();
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
                while ((linea = lecturaArchivo.ReadLine()) != null)
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
                while (linea != null && linea.ToUpper() != "ACTIONS")
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
                while ((linea = lecturaArchivo.ReadLine()) != null && !linea.ToUpper().Contains("ERROR"))
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

        /// <summary>
        /// Metodo para generar un automata
        /// </summary>
        private void CrearAutomata()
        {
            automata = new Automata(pilaTokens);
            automata.Concatenar();
            Nodo raiz = automata.Raiz();
        }
    }
}
