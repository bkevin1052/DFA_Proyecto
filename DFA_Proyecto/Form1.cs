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
        OpenFileDialog open;
        StreamReader lecturaArchivo;
        string abecedarioMayus = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", abecedarioMinus = "abcdefghijklmnopqrstuvwxyz", digitos = "0123456789", espacio = "_";
        string linea;

        
        //VARIABLES GLOBALES
        string inicioMayus = string.Empty, finMayus = string.Empty, temp = string.Empty, inicioMin = string.Empty,
        finMin = string.Empty, inicioEs = string.Empty, finEs = string.Empty, inicioChar = string.Empty, finChar = string.Empty;

        bool setsLeido = false, tokensLeido = false,actionsLeido = false,errorLeido = false;
        //SETS
        string CHARSET = string.Empty, DIGITO, LETRA = string.Empty;

        //TOKENS
        Stack<Token> pilaTokens;
        Token nuevoToken;

        //ACTIONS
        Dictionary<int, string> ListaActions;
        int llaves = 0;
        string[] expresionSimple;

        //ERROR
        private string errores;

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
                setsLeido = false;

                //LECTURA SETS
                linea = lecturaArchivo.ReadLine();
                linea = linea.TrimEnd().TrimStart();
                LecturaSets();

                //LECTURA TOKENS
                pilaTokens = new Stack<Token>();
                linea = lecturaArchivo.ReadLine();
                linea = linea.TrimEnd().TrimStart();
                LecturaTokens();

                //LECTURA ACTIONS
                linea = lecturaArchivo.ReadLine();
                linea = linea.TrimEnd().TrimStart();
                ListaActions = new Dictionary<int, string>();
                LecturaActions();

                //LECTURA ERROR
                LecturaError();
            }
        }           

        private void LecturaSets()
        {
            int x, y;            

            if (linea == "SETS" && setsLeido != true)
            {
                setsLeido = true;
                LecturaSets();
            }
            else
            {
                for (int j  = 0; j < 3; j++)
                {
                    linea = lecturaArchivo.ReadLine();
                    linea = linea.TrimEnd().TrimStart();
                    if (j == 0)
                    {
                        if (linea.Contains("LETRA"))
                        {
                            if (linea.Contains("="))
                            {
                                linea = linea.TrimEnd().TrimStart();
                                for (int i = 0; i < abecedarioMayus.Length; i++)
                                {
                                    if (linea.TrimEnd().Contains(("'" + abecedarioMayus.ToCharArray()[i] + "'").ToString()))
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
                                    if (linea.TrimEnd().Contains(("'" + abecedarioMinus.ToCharArray()[i] + "'").ToString()))
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
                                    if (linea.TrimEnd().Contains(("'" + espacio.ToCharArray()[i] + "'").ToString()))
                                    {
                                        inicioEs = temp;
                                        temp = espacio.ToCharArray()[i].ToString();
                                        if (inicioEs != null)
                                        {
                                            finEs = espacio.ToCharArray()[i].ToString();
                                        }
                                    }
                                }


                                if (inicioMayus != "" && finMayus != "")
                                {
                                    x = abecedarioMayus.IndexOf(inicioMayus);
                                    y = abecedarioMayus.LastIndexOf(finMayus);

                                    LETRA = abecedarioMayus.Substring(x, y - x + 1);
                                }


                                if (inicioMin != "" && finMin != "")
                                {
                                    x = abecedarioMinus.IndexOf(inicioMin);
                                    y = abecedarioMinus.LastIndexOf(finMin);
                                    LETRA = LETRA + abecedarioMinus.Substring(x, y - x + 1);
                                }

                                if (inicioEs != "" && finEs != "")
                                {
                                    x = espacio.IndexOf(inicioEs);
                                    y = espacio.IndexOf(finEs);
                                    LETRA = LETRA + finEs;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se encontro un signo  =  en LETRA en la columna de SETS \n" + linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show(linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                    else if (j == 1)
                    {
                        if (linea.Contains("DIGITO"))
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

                                if (inicioMayus != "" && finMayus != "")
                                {
                                    x = digitos.IndexOf(inicioMayus);
                                    y = digitos.LastIndexOf(finMayus);
                                    DIGITO = digitos.Substring(x, y - x + 1);
                                }

                            }
                            else
                            {
                                MessageBox.Show("No se encontro un signo  =  en LETRA en la columna de SETS \n" + linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show(linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;

                        }
                    }
                    else if (j == 2)
                    {
                        if (linea.Contains("CHARSET"))
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
                                        if (contador == 2)
                                        {
                                            for (int i = int.Parse(inicioChar); i < int.Parse(finChar); i++)
                                            {
                                                CHARSET = CHARSET + (char)i;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("No se encontro un rango en CHARSET", "ERROR",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            contador = 0;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se encontro un signo = en CHARSET", "ERROR",   MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show(linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }
            }
        }

        private void LecturaTokens()
        {
            
            if (linea == "TOKENS" && tokensLeido != true)
            {
                tokensLeido = true;
                LecturaTokens();
            }
            else
            {
                while((linea = lecturaArchivo.ReadLine()) != null)
                {
                    linea = linea.TrimEnd().TrimStart().ToUpper();
                    string[] expresionSimple;
                    if (linea == "ACTIONS")
                    {
                        return;
                    }

                    if (linea.Contains("TOKEN"))
                    {
                        if (linea.Contains("="))
                        {
                            expresionSimple = linea.Split('=');
                            expresionSimple[1] = expresionSimple[1].TrimStart().TrimEnd();

                            if (linea.Contains('\''))
                            {                              
                                nuevoToken = new Token();
                                nuevoToken.valor = expresionSimple[1];
                                pilaTokens.Push(nuevoToken);
                            }
                            else
                            {
                                nuevoToken = new Token();
                                nuevoToken.valor = expresionSimple[1];
                                pilaTokens.Push(nuevoToken);
                            }
                        }
                        else
                        {
                            MessageBox.Show("ERROR", "No se encontro un signo  =  en TOKEN en la columna de TOKENS\n" + linea, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                    else if(linea.Equals(""))
                    {
                        continue;
                    }
                    else
                    {
                        MessageBox.Show(linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }


        }

        private void LecturaActions()
        {
            if (linea == "ACTIONS" && actionsLeido != true)
            {
                actionsLeido = true;
                LecturaActions();
            }
            else
            {
                while((linea = lecturaArchivo.ReadLine()) != null)
                {
                    
                    linea = linea.TrimEnd().TrimStart();
                    if(linea.Contains("error") || linea.Contains("ERROR"))
                    {
                        return;
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
                        expresionSimple = linea.TrimEnd().TrimStart().Split('\'');
                        try
                        {
                            ListaActions.Add(int.Parse(expresionSimple[0].Substring(0, 2)), expresionSimple[1]);
                        }
                        catch
                        {
                            MessageBox.Show("No se pudo completar la lectura debido a siguiente error " + linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        if (linea == "ACTIONS")
                        {
                            actionsLeido = true;
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private void LecturaError()
        {
            if (linea.Contains("ERROR") || linea.Contains("error") || errorLeido == true)
            {
                linea = linea.Trim().TrimEnd().TrimStart();
                string[] expresion = linea.Split(' ');
            
                if (linea.Contains("="))
                {
                    errorLeido = true;
                    errores = expresion[2];
                }
                else
                {
                    MessageBox.Show("Error no contiene = " + linea, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            

            if (errorLeido == false)
            {
                MessageBox.Show("Error no contiene ERROR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
