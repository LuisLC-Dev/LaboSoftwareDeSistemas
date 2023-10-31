using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Practica3
{
    public partial class SicxeForm : Form
    {
        string archivo;
        List<List<String>> code;
        public static List<string> errorList = new List<string>();
        Dictionary<String, String> TabSim;
        string Base = "fffff";
        string cp = "";
        string[] regObj;
        List<RegistroM> regM = new List<RegistroM>();
        List<RegistroT> regT = new List<RegistroT>();

        public SicxeForm()
        {
            InitializeComponent();
        }
              //******************************************Abrir archivos .s******************************************
        private void openFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "s files (*.s)|*.s|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            { if (ofd.FileName.Contains(".s"))                           // Con esto por defecto intentara abrir archivos con terminacion .s
                { this.archivo = ofd.FileName;                           //Guarda el nombre del archivo para evitar errores
                    codeTB.Text = "";
                    errorsTB.Text = "";                                 
                    codeTB.Lines = File.ReadAllLines(archivo);           //Lee el archivo ".s" e imprime en textBox_codigo
                    this.code = new List<List<String>>();                //Con esto vacia el contenido del codigo
                }
            }
        }
                //******************************************Tabla de Errores ******************************************
        private void analizeCodeButton_Click(object sender, EventArgs e)
        {
            {
                
                errorsTB.Text = "";                                      //Reinicia la tabla de errores
                tabSimDGV.Rows.Clear();
                codeDetailsDGV.Rows.Clear();
                TabSim = new Dictionary<string, string>();
                code = new List<List<String>>();                         //Agrega el codigo en la lista de uno en uno para uso en paso 1
                for (int i = 0; i < codeTB.Lines.Length; i++)
                { String trim = codeTB.Lines[i].Trim();                  //Elimina espacios delante y detras del string,  para evitar errores
                    if (trim != "")
                        code.Add(new List<string> { trim });
                }
                errorList = new List<string>();
                if (codeTB.Text != "")                                   //Si la lista de errores se encuentra vacia, continua
                {String code = codeTB.Text + Environment.NewLine;
                    sicxeGramLexer lex = new sicxeGramLexer(new AntlrInputStream(code));
                    CommonTokenStream tokens = new CommonTokenStream(lex);
                    sicxeGramParser parser = new sicxeGramParser(tokens);
                    ErrorListener errores = new ErrorListener();
                    parser.AddErrorListener(errores);
                    try
                    {
                        IParseTree parseTree = parser.program();         // Analizador de la gramatica                                                
                        if (errorList.Count > 0)
                        {
                            foreach (String error in errorList)
                            {
                                errorsTB.Text += error + Environment.NewLine;
                            }
                        }
                        else
                            errorsTB.Text = "No se encontraron errores"; //Una vez realizado el analisis, muestra este mensaje en caso de que no hubo errores
                    }
                    catch (RecognitionException ex)
                    { Console.Error.WriteLine(ex.StackTrace);
                    }
                    // Crea el archivo de errores
                    String ArchivoT = this.archivo;                     //Busca y usa la ruta ".s"
                    ArchivoT = ArchivoT.Remove(ArchivoT.Length - 2, 2); //Aqui se modifica la extension para crear un nuevo archivo .t
                    ArchivoT += ".t";
                    FileStream fs = File.Create(ArchivoT);              //Aqui se procede a crear el archivo
                    fs.Close();
                    File.WriteAllLines(ArchivoT, errorsTB.Lines);       //Toma el contenido del textbox de errores y lo pasa al archivo .t

                    paso1();                                            //Aqui comienza a hacer el calculo del paso 1
                   
                }
                else
                    MessageBox.Show("Inserta codigo");
            }

        }
                    //******************************************PASO 1******************************************
        private void paso1()
        {
            
            int ContadorPrograma = 0; // Contador de Programa
            for (int i = 0; i < code.Count; i++)
            {
                // Crear un lexer a partir del input de código
                sicxeGramLexer lex = new sicxeGramLexer(new AntlrInputStream(code[i][0] + Environment.NewLine));

                // Crear un flujo de tokens utilizando el lexer
                CommonTokenStream tokens = new CommonTokenStream(lex);

                // Crear un parser utilizando el flujo de tokens
                sicxeGramParser parser = new sicxeGramParser(tokens);

                // Inicializar un listener de errores
                ErrorListener errores = new ErrorListener();
                parser.AddErrorListener(errores);

                // Lista para almacenar posibles errores
                errorList = new List<string>();

                // Crear una nueva fila para el DataGridView
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(codeDetailsDGV);
                
                    for (int j = 0; j <= 9; j++)
                    {
                        if (r.Cells[j].Value == null)
                            r.Cells[j].Value = "";
                    }
                r.Cells[0].Value = i;
                r.Cells[2].Value = Convert.ToString(ContadorPrograma, 16);
                
                if (i == 0)
                {
                    // Parsear el código para verificar si pertenece al inicio
                    IParseTree parseTree = parser.start();

                    r.Cells[1].Value = "---";
                    r.Cells[6].Value = "---";

                    // Obtener la lista de tokens
                    IList<IToken> t = tokens.GetTokens();

                    // Verificar el tipo de token y actualizar el DataGridView según corresponda
                    if (t[0].Type.ToString() == "25")
                        r.Cells[3].Value = t[0].Text;

                    // Si hay errores, actualizar el DataGridView con información sobre el error
                    if (errorList.Count > 0)
                    {
                        r.Cells[6].Value = "Error: Sintaxis";
                        r.Cells[6].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        r.Cells[4].Value = t[1].Text;
                        List<String> op = RegresarOperandos(t);

                        if (op.Count == 1)
                        {
                            r.Cells[5].Value = op[0];
                            String num = r.Cells[5].Value.ToString();

                            // Convertir el valor al formato adecuado y actualizar el ContadorPrograma
                            if (num.Last() == 'h' || num.Last() == 'H')
                            {
                                num = num.Remove(num.Length - 1, 1);
                                ContadorPrograma += Convert.ToInt32(num, 16);
                            }
                            else
                            {
                                ContadorPrograma += Int32.Parse(r.Cells[5].Value.ToString());
                            }
                        }
                    }
                }
                else if (i == code.Count - 1)
                {
                    // Parsear el código para verificar si pertenece al final/end
                    IParseTree parseTree = parser.end();

                    r.Cells[1].Value = "---";
                    r.Cells[6].Value = "---";

                    // Obtener la lista de tokens
                    IList<IToken> t = tokens.GetTokens();

                    // Si hay errores, actualizar el DataGridView con información sobre el error
                    if (errorList.Count > 0)
                    {
                        r.Cells[6].Value = "Error: Sintaxis";
                        r.Cells[6].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        // Actualizar el DataGridView con el texto del token
                        r.Cells[4].Value = t[0].Text;

                        // Obtener operandos y si hay uno, establecer su valor en el DataGridView
                        List<String> op = RegresarOperandos(t);
                        if (op.Count == 1)
                            r.Cells[5].Value = op[0];
                    }

                }
                else
                {   // Parsear la entrada actual para verificar si corresponde a una proposición
                    IParseTree parseTree = parser.proposition();
                    // Establecer un valor predeterminado en la columna 6 (por ejemplo, columna Dirección)
                    r.Cells[7].Value = "---";
                    // Recuperar todos los tokens del código actual
                    IList<IToken> t = tokens.GetTokens();
                    // Variable para verificar si hay un símbolo/etiqueta duplicado
                    bool ErrorSimboloDuplicado = false;
                    // Comprobar si el primer token es una etiqueta


                    if (t[0].Type.ToString() == "25")                   
                    {
                        // Establecer la etiqueta en la columna correspondiente
                        r.Cells[3].Value = t[0].Text;
                        if (TabSim.ContainsKey(t[0].Text))
                            // Marcar un error si el símbolo ya existe
                            ErrorSimboloDuplicado = true;               //No debe insertarse en tabla de sim y da error en la linea
                    }

                    // Determinar el formato de la instrucción y establecer en la columna correspondiente
                    r.Cells[1].Value = RegresarFormato(t);
                    // Recuperar la instrucción específica y establecer en la columna correspondiente
                    r.Cells[4].Value = RegresarInstruccion(t);
                    // Recuperar los operandos utilizados en la instrucción y agregarlos a la columna correspondiente
                    List<String> op = RegresarOperandos(t);             
                    foreach (String o in op)
                        r.Cells[5].Value += o;

                    // Si la instrucción es de formato 3 o 4, identificar el tipo de direccionamiento
                    if (r.Cells[5].Value != null && (r.Cells[1].Value.ToString() == "3" || r.Cells[1].Value.ToString() == "4"))
                    {
                        // Determinar el tipo de direccionamiento en función de los caracteres especiales en el operando
                        if (r.Cells[5].Value.ToString().Contains("#"))
                        {
                            r.Cells[7].Value = "Inmediato"; // Modo de direccionamiento inmediato
                        }
                        else if (r.Cells[5].Value.ToString().Contains("@"))
                        {
                            r.Cells[7].Value = "Indirecto"; // Modo de direccionamiento indirecto
                        }
                        else
                        {
                            r.Cells[7].Value = "Simple"; // Modo de direccionamiento simple
                        }
                    }

                    // Tratar con operandos extras en instrucciones RSUB o +RSUB
                    if (r.Cells[4].Value.ToString().Contains("RSUB") || r.Cells[4].Value.ToString().Contains("+RSUB"))
                    {
                        // Si no hay operandos (null o cadena vacía), establecer un valor predeterminado
                        if (r.Cells[5].Value == null || r.Cells[5].Value.ToString() == "")
                        {
                            r.Cells[7].Value = "Simple"; // No hay operandos en RSUB
                        }
                        else
                        {
                            // Si hay operandos en RSUB, marcar un error de sintaxis en la columna 6
                            r.Cells[6].Value = "Error: Sintaxis";
                            r.Cells[6].Style.ForeColor = System.Drawing.Color.Red;
                        }
                    }


                    // Tratar con operandos en formato 1 y generar un error de sintaxis
                    if (r.Cells[1].Value.ToString() == "1")
                    {
                        // Verificar si hay un operando presente y no está vacío
                        if (r.Cells[5].Value != null && r.Cells[5].Value.ToString() != "")
                        {
                            // Establecer un mensaje de error de sintaxis en la columna 6 y cambiar el color a rojo
                            r.Cells[6].Value = "Error: Sintaxis";
                            r.Cells[6].Style.ForeColor = System.Drawing.Color.Red;
                        }
                    }


                    // Tratar con errores y la inserción de símbolos en TabSim
                    if (errorList.Count > 0)
                    {
                        // Si hay errores en la lista, verificar si la instrucción es "Error" y establecer el mensaje de error correspondiente
                        if (r.Cells[4].Value.ToString() == "Error")
                        {
                            r.Cells[6].Value = "Error: Instrucción no existe";
                        }
                        else
                        {
                            // Establecer un mensaje de error de sintaxis si no es una instrucción de error
                            r.Cells[6].Value = "Error: Sintaxis";
                        }
                        // Cambiar el color del texto a rojo para indicar un error
                        r.Cells[6].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (ErrorSimboloDuplicado)
                    {
                        // Si hay un símbolo duplicado, establecer un mensaje de error correspondiente
                        r.Cells[6].Value = "Error: Símbolo Duplicado";
                        // Cambiar el color del texto a rojo para indicar un error
                        r.Cells[6].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    // Si no hay ningún tipo de error y hay una etiqueta al inicio del programa, debe agregarse a TabSim y dGV_Sim
                    if (t[0].Type.ToString() == "25") // Identificar si el primer token es una etiqueta
                    {
                        // Agregar la etiqueta y su valor (en decimal) a TabSim
                        TabSim.Add(t[0].Text, ContadorPrograma.ToString());

                        // Crear una nueva fila para el DataGridView tabSimDGV
                        DataGridViewRow rs = new DataGridViewRow();
                        rs.CreateCells(tabSimDGV); // Crea las celdas en el nuevo renglón según las columnas existentes en el DataGridView que se le pasa

                        // Establecer el valor de la etiqueta en la primera columna
                        rs.Cells[0].Value = t[0].Text;

                        // Convertir el valor decimal a hexadecimal y establecerlo en la segunda columna
                        rs.Cells[1].Value = Convert.ToString(ContadorPrograma, 16);

                        // Agregar la fila a la tabla DataGridView de Archivo Intermedio (tabSimDGV)
                        tabSimDGV.Rows.Add(rs);
                    }


                    //Incremento de CP para formato 1-4 y directivas
                    if (!(r.Cells[1].Value.ToString() == "Error" || r.Cells[6].Value.ToString() == "Error: Sintaxis" || r.Cells[6].Value.ToString() == "Error: Instruccion no existe"))
                    {
                        switch (r.Cells[1].Value.ToString())
                        {
                            case "1":
                                ContadorPrograma += 1;
                                break;
                            case "2":
                                ContadorPrograma += 2;
                                break;
                            case "3":
                                ContadorPrograma += 3;
                                break;
                            case "4":
                                ContadorPrograma += 4;
                                break;
                            case "---": //Directiva
                                {
                                    switch (r.Cells[4].Value.ToString())
                                    {
                                        case "BASE": //nada
                                            break;
                                        case "RESW":
                                            String s = r.Cells[5].Value.ToString();
                                            if (s.Last() == 'h' || s.Last() == 'H')
                                            {
                                                s = s.Remove(s.Length - 1, 1);//Remover letra
                                                ContadorPrograma += Convert.ToInt32(s, 16) * 3;
                                            }
                                            else
                                            {
                                                ContadorPrograma += Int32.Parse(r.Cells[5].Value.ToString()) * 3;
                                            }
                                            break;
                                        case "RESB":
                                            String s2 = r.Cells[5].Value.ToString();
                                            if (s2.Last() == 'h' || s2.Last() == 'H')
                                            {
                                                s2 = s2.Remove(s2.Length - 1, 1);//Remover letra
                                                ContadorPrograma += Convert.ToInt32(s2, 16);
                                            }
                                            else
                                            {
                                                ContadorPrograma += Int32.Parse(r.Cells[5].Value.ToString());
                                            }
                                            break;
                                        case "WORD":
                                            ContadorPrograma += 3;
                                            break;
                                        case "BYTE":
                                            String s3 = r.Cells[5].Value.ToString();
                                            if (s3.First() == 'X')
                                            {
                                                int calculo = (s3.Length - 3) / 2;
                                                if ((s3.Length - 3) % 2 == 1)
                                                    calculo++;
                                                ContadorPrograma += calculo;
                                            }
                                            else if (s3.First() == 'C')
                                            {
                                                int calculo = s3.Length - 3;
                                                ContadorPrograma += calculo;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                // Agregar la fila actual al DataGridView codeDetailsDGV, que representa el archivo intermedio
                codeDetailsDGV.Rows.Add(r);
            }
            // Actualizar el tamaño del programa como último paso a considerar
            programSizeLBL2.Text = Convert.ToString(ContadorPrograma, 16) + 'H';
            // Llamar a la función Paso2() para continuar con la siguiente etapa del proceso (no se proporciona el código de esta parte).
            Paso2();
            GeneraRegistrosObj();
        }

        String RegresarFormato(IList<IToken> t)
        {
            // Inicializar el contador y el valor de retorno predeterminado como "---" (se considera directiva por defecto)
            int cont = 0;
            String ret = "---";

            // Determinar el formato de la instrucción en función de los tokens proporcionados
            // Formato1: 15, Formato2: 16-19, Formato3: 20, Formato4: 21
            foreach (IToken token in t)
            {
                switch (token.Type.ToString())
                {
                    case "15":
                        ret = "1"; 
                        cont++;
                        break;
                    case "16":
                        ret = "2";
                        cont++;
                        break;
                    case "17":
                        ret = "2";
                        cont++;
                        break;
                    case "18":
                        ret = "2";
                        cont++;
                        break;
                    case "19":
                        ret = "2";
                        cont++;
                        break;
                    case "20":
                        ret = "3";
                        cont++;
                        break;
                    case "3":
                        ret = "3"; //Caso especial de RSUB
                        cont++;
                        break;
                    case "21":
                        ret = "4";
                        cont++;
                        break;
                    case "6":
                        ret = "4"; //Caso especial de +RSUB
                        cont++;
                        break;
                    default: break;
                }
            }
            if (cont <= 1)
                return ret;
            else
                return "Error";
        }

        String RegresarInstruccion(IList<IToken> t)
        {
            int cont = 0;
            String ret = "";
            //Formato1: 15 , Formato2: 16-19 , Formato3: 20 , Formato4: 21
            foreach (IToken token in t)
            {
                if (token.Type == 3 || token.Type == 6 || (token.Type >= 7 && token.Type <= 11) || (token.Type >= 15 && token.Type <= 21))
                {
                    ret = token.Text;
                    cont++;
                }
            }
            if (cont == 1)
                return ret;
            else
                return "Error";
        }

        List<String> RegresarOperandos(IList<IToken> t)
        {
            List<String> Operandos = new List<String>();
            for (int i = 0; i < t.Count; i++)
            {
                if (i != 0 && (t[i].Type == 1 || t[i].Type == 2 || t[i].Type == 4 || t[i].Type == 5 || t[i].Type == 14 || (t[i].Type >= 23 && t[i].Type <= 29)))
                    Operandos.Add(t[i].Text);
            }
            return Operandos;
        }

        private void codeDetailsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Paso2()
        {
            
            Base = "ffffff";
            //Tabla de codigos operacionales
            Dictionary<String, String> CodOp = new Dictionary<String, String>
            {
                {"ADD","18"},{"ADDF","58"},{"ADDR","90"},{"AND","40"},{"CLEAR","B4"},{"COMP","28"},{"COMPF","88"},{"COMPR","A0"},{"DIV","24"},{"DIVF","64"},
                {"DIVR","9C"},{"FIX","C4"},{"FLOAT","C0"},{"HIO","F4"},{"J","3C"},{"JEQ","30"},{"JGT","34"},{"JLT","38"},{"JSUB","48"},{"LDA","00"},
                {"LDB","68"},{"LDCH","50"},{"LDF","70"},{"LDL","08"},{"LDS","6C"},{"LDT","74"},{"LDX","04"},{"LPS","D0"},{"MUL","20"},{"MULF","60"},
                {"MULR","98"},{"NORM","C8"},{"OR","44"},{"RD","D8"},{"RMO","AC"},{"RSUB","4C"},{"SHIFTL","A4"},{"SHIFTR","A8"},{"SIO","F0"},{"SSK","EC"},
                {"STA","0C"},{"STB","78"},{"STCH","54"},{"STF","80"},{"STI","D4"},{"STL","14"},{"STS","7C"},{"STSW","E8"},{"STT","84"},{"STX","10"},
                {"SUB","1C"},{"SUBF","5C"},{"SUBR","94"},{"SVC","B0"},{"TD","E0"},{"TIO","F8"},{"TIX","2C"},{"TIXR","B8"},{"WD","DC"}
            };

            Dictionary<String, String> Registros = new Dictionary<string, string>
            {
                {"A","0"},{"X","1"},{"L","2"},{"B","3"},{"S","4"},{"T","5"},{"F","6"},{"CP","8"},{"PC","8"},{"SW","9"}
            };

            int renglon = 0; //Conteo de renglones para pasar como parametro
                             //Inicio de creacion codigo objeto
            
            foreach (DataGridViewRow row in codeDetailsDGV.Rows)
            {

                if (row.Cells[4].Value.ToString() == "BASE")
                {
                    if (TabSim.ContainsKey(row.Cells[5].Value.ToString()))
                    {
                        Base = TabSim[row.Cells[5].Value.ToString()];
                    }
                }
                if (renglon + 1 < codeDetailsDGV.Rows.Count - 1)
                    cp = codeDetailsDGV.Rows[renglon + 1].Cells[2].Value.ToString();



                //Si hay un error de instruccion o sintaxis anteriormente se lo salta
                if (row.Cells[6].Value != null && (row.Cells[6].Value.ToString().Contains("Error: Instrucción no existe") || row.Cells[6].Value.ToString().Contains("Error: Sintaxis")))
                    row.Cells[7].Value = "---";
                else
                {
                    //Filtrar por formato o directiva
                    switch (row.Cells[1].Value)
                    {
                        case "1":
                            CodigoObjeto_Formato1(CodOp, renglon, row.Cells[4].Value.ToString());
                            break;
                        case "2":
                            CodigoObjeto_Formato2(CodOp, Registros, renglon, row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString());
                            break;
                        case "3":
                            if (row.Cells[4].Value.ToString() == "RSUB")
                                CodigoObjeto_Formato3(CodOp, TabSim, renglon, row.Cells[4].Value.ToString(), "");
                            else
                                CodigoObjeto_Formato3(CodOp, TabSim, renglon, row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString());
                            break;
                        case "4":
                            if (row.Cells[4].Value.ToString() == "+RSUB")
                                CodigoObjeto_Formato4(CodOp, TabSim, renglon, row.Cells[4].Value.ToString(), "");
                            else
                                CodigoObjeto_Formato4(CodOp, TabSim, renglon, row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString());
                            break;
                        case "---":
                            if (row.Cells[4].Value.ToString() == "BYTE" || row.Cells[4].Value.ToString() == "WORD")
                                CodigoObjeto_Directiva(row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString(), renglon);
                            break;
                    }
                }
                renglon++; //Siguiente renglon en tabla
            }
            
        }

        #region CodigosObjeto
        private void CodigoObjeto_Formato1(Dictionary<String, String> d, int renglon, String instruccion)
        {
            codeDetailsDGV.Rows[renglon].Cells[8].Value = d[instruccion];
        }

        private void CodigoObjeto_Formato2(Dictionary<String, String> d, Dictionary<String, String> reg, int renglon, String instruccion, String Operandos)
        {
            String conc = "";
            String[] op = Operandos.Split(',');
            foreach (String o in op)
            {
                if (reg.ContainsKey(o.Trim()))
                    conc += reg[o.Trim()];
                else
                    conc += Convert.ToString(Convert.ToInt32(o.Trim(), 16) - 1, 16);
            }
            if (conc.Length % 2 == 1)
                codeDetailsDGV.Rows[renglon].Cells[8].Value = d[instruccion] + conc + '0';
            else
                codeDetailsDGV.Rows[renglon].Cells[8].Value = d[instruccion] + conc;
        }

        private void CodigoObjeto_Formato3(Dictionary<String, String> d, Dictionary<String, String> tabsim, int renglon, String instruccion, String Operandos)
        {
            if (instruccion == "RSUB")
            {
                codeDetailsDGV.Rows[renglon].Cells[8].Value = "4f0000";
                return;
            }

            int xbpe = 0;
            String conc = "";
            String[] op = Operandos.Split(',');
            string simbolo = "";

            if (op.Length > 1)
            {
                xbpe = +8; //si es indexado
            }

            switch (Operandos[0])
            {
                case '@':
                    conc += Convert.ToString(Convert.ToInt32(d[instruccion], 16) + 2, 16);
                    for (int i = 1; i <= op[0].Length - 1; i++)
                    {
                        simbolo += op[0][i];//obtener el nombre de la etiqueta o el numero
                    }
                    if (op.Length > 1)
                    {
                        xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Modo de direccionamiento no existe";
                    }
                    else
                    {
                        if (tabsim.ContainsKey(simbolo))
                        {
                            simbolo = Convert.ToString(Convert.ToInt32(tabsim[simbolo]), 16);
                            if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                            {
                                xbpe += 2;
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                {
                                    for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                }
                                else
                                {
                                    string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                    for (int i = 0; i <= 2; i++)
                                    {
                                        conc += aux[aux.Length - i - 1];
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                {
                                    xbpe += 4;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16);
                                }
                                else
                                {
                                    xbpe += 6;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";
                                    codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                }
                            }
                        }
                        else
                        if (isNumber(simbolo))
                        {

                            if (simbolo.Contains("h") || simbolo.Contains("H"))
                            {
                                string aux = simbolo.Remove(simbolo.Length - 1);//remover la h para poder operar con hexadecimal
                                if (Convert.ToInt32(aux.ToString(), 16) < 4095)
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    for (int i = aux.Length; i <= 2; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += aux;
                                }
                                else
                                {
                                    if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                                    {
                                        xbpe += 2;
                                        conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                        if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                        {
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            aux = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                            for (int i = 0; i <= 2; i++)
                                            {
                                                conc += aux[aux.Length - i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                        {
                                            xbpe += 4;
                                            conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Aqui van las operaciones en decimal
                                if (Convert.ToInt32(simbolo) < 4095)
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    for (int i = simbolo.Length; i <= 2; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += simbolo;
                                }
                                else
                                {
                                    if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                                    {
                                        xbpe += 2;
                                        conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                        if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                        {
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16);
                                            for (int i = 0; i <= 2; i++)
                                            {
                                                conc += aux[aux.Length - i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                        {
                                            xbpe += 4;
                                            conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Símbolo no existe en TABSIM";
                            xbpe += 6;
                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                        }
                    }
                    break;
                case '#':
                    conc += Convert.ToString(Convert.ToInt32(d[instruccion], 16) + 1, 16);
                    for (int i = 1; i <= op[0].Length - 1; i++)
                    {
                        simbolo += op[0][i];//obtener el nombre de la etiqueta o el numero
                    }
                    if (op.Length > 1)
                    {
                        xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Modo de direccionamiento no existe";
                    }
                    else
                    {
                        if (tabsim.ContainsKey(simbolo))
                        {
                            simbolo = Convert.ToString(Convert.ToInt32(tabsim[simbolo]), 16);
                            if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                            {
                                xbpe += 2;
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                {
                                    for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                }
                                else
                                {
                                    string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                    for (int i = 0; i <= 2; i++)
                                    {
                                        conc += aux[aux.Length - 1 - i];
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                {
                                    xbpe += 4;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16);
                                }
                                else
                                {
                                    xbpe += 6;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";
                                    codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                }
                            }
                        }
                        else
                        if (isNumber(simbolo))
                        {
                            if (simbolo.Contains("h") || simbolo.Contains("H"))
                            {
                                simbolo.Remove(simbolo.Length);//remover la h para poder operar con hexadecimal
                                if (Convert.ToInt32(simbolo.ToString(), 16) < 4095)
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    for (int i = simbolo.Length; i <= 2; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += simbolo;
                                }
                                else
                                {
                                    if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                                    {
                                        xbpe += 2;
                                        conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                        if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                        {
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                            for (int i = 0; i <= 2; i++)
                                            {
                                                conc += aux[aux.Length - i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                        {
                                            xbpe += 4;
                                            conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Aqui van las operaciones en decimal
                                if (Convert.ToInt32(simbolo) < 4095)
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    for (int i = simbolo.Length; i <= 2; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += simbolo;
                                }
                                else
                                {
                                    if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                                    {
                                        xbpe += 2;
                                        conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                        if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                        {
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16);
                                            for (int i = 0; i <= 2; i++)
                                            {
                                                conc += aux[aux.Length - i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                        {
                                            xbpe += 4;
                                            conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                            for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                            {
                                                conc += "0";
                                            }
                                            conc += Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16), 16);
                                        }
                                        else
                                        {
                                            xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Símbolo no existe en TABSIM";
                            xbpe += 6;
                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                        }
                    }
                    break;
                default:
                    conc += Convert.ToString(Convert.ToInt32(d[instruccion], 16) + 3, 16);

                    simbolo = op[0];
                    if (tabsim.ContainsKey(simbolo))
                    {
                        simbolo = Convert.ToString(Convert.ToInt32(tabsim[simbolo]), 16);
                        if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                        {
                            xbpe += 2;
                            conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                            if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                            {
                                for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                {
                                    conc += "0";
                                }
                                conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                            }
                            else
                            {
                                string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                for (int i = 0; i <= 2; i++)
                                {
                                    conc += aux[aux.Length - 1 - i];
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                            {
                                xbpe += 4;
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                {
                                    conc += "0";
                                }
                                conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16);
                            }
                            else
                            {
                                xbpe += 6;
                                conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";
                                codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                            }
                        }
                    }
                    else
                    if (isNumber(simbolo))
                    {
                        if (simbolo.Contains("h") || simbolo.Contains("H"))
                        {
                            string aux = simbolo.Remove(simbolo.Length - 1);//remover la h para poder operar con hexadecimal
                            if (Convert.ToInt32(aux.ToString(), 16) < 4095)
                            {
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                for (int i = aux.Length; i <= 2; i++)
                                {
                                    conc += "0";
                                }
                                conc += aux;
                            }
                            else
                            {
                                if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                                {
                                    xbpe += 2;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                    {
                                        for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                        {
                                            conc += "0";
                                        }
                                        conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                    }
                                    else
                                    {
                                        aux = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(cp.ToString(), 16), 16);
                                        for (int i = 0; i <= 2; i++)
                                        {
                                            conc += aux[aux.Length - i];
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                    {
                                        xbpe += 4;
                                        conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                        for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                        {
                                            conc += "0";
                                        }
                                        conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16) - Convert.ToInt32(Base.ToString(), 16), 16);
                                    }
                                    else
                                    {
                                        xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Aqui van las operaciones en decimal
                            if (Convert.ToInt32(simbolo) < 4095)
                            {
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                for (int i = simbolo.Length; i <= 2; i++)
                                {
                                    conc += "0";
                                }
                                conc += simbolo;
                            }
                            else
                            {
                                if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) >= -2048 && Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) <= 2047)
                                {
                                    xbpe += 2;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                    if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16) >= 0)
                                    {
                                        for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16).Length; i < 3; i++)
                                        {
                                            conc += "0";
                                        }
                                        conc += Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16);
                                    }
                                    else
                                    {
                                        string aux = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(cp.ToString(), 16), 16);
                                        for (int i = 0; i <= 2; i++)
                                        {
                                            conc += aux[aux.Length - i];
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16) >= 0 && Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16) <= 4095)
                                    {
                                        xbpe += 4;
                                        conc += Convert.ToString(Convert.ToInt32(xbpe), 16);
                                        for (int i = Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16), 16).Length; i < 3; i++)
                                        {
                                            conc += "0";
                                        }
                                        conc += Convert.ToString(Convert.ToInt32(simbolo.ToString()) - Convert.ToInt32(Base.ToString(), 16), 16);
                                    }
                                    else
                                    {
                                        xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: no es relativo a B o a CP";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Símbolo no existe en TABSIM";
                        xbpe += 6;
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fff";//convertir xbpe a 
                    }
                    break;
            }
            if (conc.Length % 2 != 0)
                conc = "0" + conc;
            codeDetailsDGV.Rows[renglon].Cells[8].Value = conc;
        }

        private void CodigoObjeto_Formato4(Dictionary<String, String> d, Dictionary<String, String> tabsim, int renglon, String instruccion, String Operandos)
        {
            if (instruccion == "RSUB")
            {
                codeDetailsDGV.Rows[renglon].Cells[8].Value = "4f100000*";
                return;
            }
            int xbpe = 1;
            String conc = "";
            String[] op = Operandos.Split(',');
            string simbolo = "";
            string inst = instruccion.Remove(0, 1);
            if (op.Length > 1)
            {
                xbpe = +8; //si es indexado
            }
            switch (Operandos[0])
            {
                case '@':
                    conc += Convert.ToString(Convert.ToInt32(d[inst], 16) + 2, 16);
                    for (int i = 1; i <= op[0].Length - 1; i++)
                    {
                        simbolo += op[0][i];//obtener el nombre de la etiqueta o el numero
                    }
                    if (op.Length > 1)
                    {
                        xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fffff";//convertir xbpe a 
                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Modo de direccionamiento no existe";
                    }
                    else
                    {
                        if (tabsim.ContainsKey(simbolo))
                        {
                            simbolo = tabsim[simbolo];
                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                            for (int i = simbolo.Length; i < 5; i++)
                            {
                                conc += "0";
                            }
                            conc += simbolo + "*";
                        }
                        else
                        if (isNumber(simbolo))
                        {
                            if (simbolo.Contains("h") || simbolo.Contains("H"))
                            {
                                simbolo.Remove(simbolo.Length);//remover la h para poder operar con hexadecimal
                                if (Convert.ToInt32(simbolo.ToString(), 16) < 4095)
                                {
                                    xbpe += 6;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16) + "fffff";
                                    codeDetailsDGV.Rows[renglon].Cells[9].Value = "error:operando fuera de rango";
                                }
                                else
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                                    for (int i = simbolo.Length; i < 5; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += simbolo;
                                }
                            }
                            else
                            {
                                //Aqui van las operaciones en decimal
                                if (Convert.ToInt32(simbolo) < 4095)
                                {
                                    xbpe += 6;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16) + "fffff";
                                    codeDetailsDGV.Rows[renglon].Cells[9].Value = "error:operando fuera de rango";
                                }
                                else
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                                    for (int i = simbolo.Length; i < 5; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16), 16);
                                }
                            }

                        }
                        else
                        {
                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Símbolo no existe en TABSIM";
                            xbpe += 6;
                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fffff";//convertir xbpe a 
                        }
                    }
                    break;
                case '#':
                    conc += Convert.ToString(Convert.ToInt32(d[inst], 16) + 1, 16);
                    for (int i = 1; i <= op[0].Length - 1; i++)
                    {
                        simbolo += op[0][i];//obtener el nombre de la etiqueta o el numero
                    }
                    if (op.Length > 1)
                    {
                        xbpe += 6; //Marca error porque no es un modo de direccionamiento válido
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fffff";//convertir xbpe a 
                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Modo de direccionamiento no existe";
                    }
                    else
                    {
                        if (tabsim.ContainsKey(simbolo))
                        {
                            simbolo = tabsim[simbolo];
                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                            for (int i = simbolo.Length; i < 5; i++)
                            {
                                conc += "0";
                            }
                            conc += simbolo + "*";
                        }
                        else
                        if (isNumber(simbolo))
                        {
                            if (simbolo.Contains("h") || simbolo.Contains("H"))
                            {
                                simbolo.Remove(simbolo.Length);//remover la h para poder operar con hexadecimal
                                if (Convert.ToInt32(simbolo.ToString(), 16) < 4095)
                                {
                                    xbpe += 6;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16) + "fffff";
                                    codeDetailsDGV.Rows[renglon].Cells[9].Value = "error:operando fuera de rango";
                                }
                                else
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                                    for (int i = simbolo.Length; i < 5; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += simbolo;
                                }
                            }
                            else
                            {
                                //Aqui van las operaciones en decimal
                                if (Convert.ToInt32(simbolo) < 4095)
                                {
                                    xbpe += 6;
                                    conc += Convert.ToString(Convert.ToInt32(xbpe), 16) + "fffff";
                                    codeDetailsDGV.Rows[renglon].Cells[9].Value = "error:operando fuera de rango";
                                }
                                else
                                {
                                    conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                                    for (int i = simbolo.Length; i < 5; i++)
                                    {
                                        conc += "0";
                                    }
                                    conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16), 16);
                                }
                            }

                        }
                        else
                        {
                            codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Símbolo no existe en TABSIM";
                            xbpe += 6;
                            conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fffff";//convertir xbpe a 
                        }
                    }
                    break;
                default:
                    simbolo = op[0];
                    conc += Convert.ToString(Convert.ToInt32(d[inst], 16) + 3, 16);
                    if (tabsim.ContainsKey(simbolo))
                    {
                        simbolo = tabsim[simbolo];
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                        for (int i = simbolo.Length; i < 5; i++)
                        {
                            conc += "0";
                        }
                        conc += simbolo + "*";
                    }
                    else
                        if (isNumber(simbolo))
                    {
                        if (simbolo.Contains("h") || simbolo.Contains("H"))
                        {
                            string aux = simbolo.Remove(simbolo.Length - 1);//remover la h para poder operar con hexadecimal
                            if (Convert.ToInt32(aux.ToString(), 16) < 4095)
                            {
                                xbpe += 6;
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16) + "fffff";
                                codeDetailsDGV.Rows[renglon].Cells[9].Value = "error:operando fuera de rango";
                            }
                            else
                            {
                                conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                                for (int i = simbolo.Length; i < 5; i++)
                                {
                                    conc += "0";
                                }
                                conc += simbolo;
                            }
                        }
                        else
                        {
                            //Aqui van las operaciones en decimal
                            if (Convert.ToInt32(simbolo) < 4095)
                            {
                                xbpe += 6;
                                conc += Convert.ToString(Convert.ToInt32(xbpe), 16) + "fffff";
                                codeDetailsDGV.Rows[renglon].Cells[9].Value = "error:operando fuera de rango";
                            }
                            else
                            {
                                conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16));
                                for (int i = simbolo.Length; i < 5; i++)
                                {
                                    conc += "0";
                                }
                                conc += Convert.ToString(Convert.ToInt32(simbolo.ToString(), 16), 16);
                            }
                        }

                    }
                    else
                    {
                        codeDetailsDGV.Rows[renglon].Cells[9].Value = "error: Símbolo no existe en TABSIM";
                        xbpe += 6;
                        conc += Convert.ToString(Convert.ToInt32(xbpe.ToString(), 16)) + "fffff";//convertir xbpe a 
                    }
                    break;
            }
            if (conc.Contains("*"))
            {
                if (conc.Length % 2 != 1)
                {
                    conc = "0" + conc;
                }
            }
            else
            {
                if (conc.Length % 2 != 0)
                {
                    conc = "0" + conc;
                }
            }    
            codeDetailsDGV.Rows[renglon].Cells[8].Value = conc;
        }

        private void CodigoObjeto_Directiva(string directiva, string operando, int renglon)
        {
            string codObj = "";
            switch (directiva)
            {
                case "BYTE":
                    String[] op = operando.Split('\'');
                    if (op[0] == "C")
                        for (int i = 0; i < op[1].Length; i++)
                            codObj += Convert.ToString(Convert.ToInt32((int)op[1][i]), 16);
                    else
                        if (op[0] == "X" && op[1].Length % 2 != 0)
                        codObj = "0" + op[1];
                    else
                        codObj = op[1];
                    break;
                case "WORD":
                    if (operando.Contains("h") || operando.Contains("H"))
                    {
                        for (int i = operando.Length - 1; i < 6; i++)
                            codObj += "0";
                        for (int i = 0; i < operando.Length - 1; i++)
                            codObj += operando[i];
                    }
                    else
                    {
                        for (int i = Convert.ToString(Convert.ToInt32(operando), 16).Length; i < 6; i++)
                            codObj += "0";
                        for (int i = 0; i < Convert.ToString(Convert.ToInt32(operando), 16).Length; i++)
                            codObj += Convert.ToString(Convert.ToInt32(operando), 16)[i];
                    }
                    break;
                default:
                    break;
            }
            codeDetailsDGV.Rows[renglon].Cells[8].Value = codObj;
        }

        private bool isNumber(string cadena)
        {
            if (cadena[cadena.Length - 1] == 'h' || cadena[cadena.Length - 1] == 'H')
            {
                for (int i = 0; i < cadena.Length - 1; i++)
                {
                    if (!(((int)cadena[i] >= 65 && (int)cadena[i] <= 70) || ((int)cadena[i] >= 48 && (int)cadena[i] <= 57)) || ((int)cadena[i] >= 97 && (int)cadena[i] <= 102))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cadena.Length - 1; i++)
                {
                    if (!((int)cadena[i] >= 48 && (int)cadena[i] <= 57))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region registrosobj
        void GeneraRegistrosObj()
        {
            registerDGV.Rows.Clear();
            //limpiar las listas de registros
            regM = new List<RegistroM>();
            regT = new List<RegistroT>();

            DataGridViewRow H = new DataGridViewRow();
            H.CreateCells(registerDGV);
            H.Cells[0].Value = "H";
            H.Cells[1].Value = registroH();
            //MessageBox.Show(t.returnRegister());
            registerDGV.Rows.Add(H);
            //generar registro H
            string nomprog = codeDetailsDGV.Rows[0].Cells[3].Value.ToString();
            //generar registros T
            RegistroT auxT = new RegistroT();
            for (int i = 1; i < codeDetailsDGV.RowCount; i++)
            {

                //4 instrucciones // 6 Errores paso 1// 9 
                if (codeDetailsDGV.Rows[i].Cells[8].Value.ToString() != "" /*Esto es si no esta vacio el codobj*/ )
                {
                    String codObj = codeDetailsDGV.Rows[i].Cells[8].Value.ToString();
                    if (auxT.size == 0)
                    {
                        auxT.setInicio(codeDetailsDGV.Rows[i].Cells[2].Value.ToString());
                    }

                    //Verificar si es relocalizable
                    if (codeDetailsDGV.Rows[i].Cells[8].Value.ToString().Contains("*"))
                    {
                        if (codeDetailsDGV.Rows[i].Cells[4].Value.ToString() == "WORD")
                        {
                            codObj = codObj.Remove(codObj.Length - 1);
                            MessageBox.Show(codObj);
                            regM.Add(new RegistroM(codeDetailsDGV.Rows[i].Cells[2].Value.ToString().Substring(0,5), 6, "+", nomprog));
                        }
                        else {
                            regM.Add(new RegistroM(/**/ Convert.ToString(Convert.ToInt32(codeDetailsDGV.Rows[i].Cells[2].Value.ToString(), 16) +1, 16)/**/ , 5, "+", nomprog));
                        }
                    }
                    
                    if (auxT.size + codObj.Length / 2 <= 30) //verifica que se pueda agregar el codobj por el tamaño
                    {
                        auxT.addObjCode(codObj.Length / 2, codeDetailsDGV.Rows[i].Cells[8].Value.ToString());
                    }
                    else
                    {
                        regT.Add(auxT); // si no cabe por el tamaño entonces corta el registro objeto y lo agrega a la lista de registros T
                        auxT = new RegistroT(codObj); //crea un nuevo registro T
                        auxT.addObjCode(codObj.Length / 2, codeDetailsDGV.Rows[i].Cells[8].Value.ToString()); //agrega el objeto que no se pudo insertar antes
                    }
                }
                if (isDirective(codeDetailsDGV.Rows[i].Cells[4].Value.ToString()) && auxT.size > 0)
                {
                        regT.Add(auxT); //Corta el registro Objeto y lo agrega a la lista de registros T
                        auxT = new RegistroT(codeDetailsDGV.Rows[i].Cells[2].Value.ToString()); //crea un nuevo registro T                   
                }

            }
            //Generar registros M
            foreach (RegistroT t in regT)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(registerDGV);
                r.Cells[0].Value = "T";
                r.Cells[1].Value = t.returnRegister();
                //MessageBox.Show(t.returnRegister());
                registerDGV.Rows.Add(r);
            }
            //generar registro E
            foreach (RegistroM m in regM)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(registerDGV);
                r.Cells[0].Value = "M";
                r.Cells[1].Value = m.returnRegister();
                //MessageBox.Show(t.returnRegister());
                registerDGV.Rows.Add(r);
            }

            DataGridViewRow E = new DataGridViewRow();
            E.CreateCells(registerDGV);
            E.Cells[0].Value = "E";
            E.Cells[1].Value = RegistroE();
            //MessageBox.Show(t.returnRegister());
            registerDGV.Rows.Add(E);

        }

        private string RegistroE()
        {
            String CodObE = "";

            if (codeDetailsDGV.Rows[codeDetailsDGV.Rows.Count - 1].Cells[5].Value.ToString() == "")
            { //Si END no tcodeDetailsDGViene etiqueta, busca la primera instruccion que genera codigo objeto
                int i = 0;
                for (i = 0; i < codeDetailsDGV.Rows.Count; i++)
                { //Busca en la columna de CodOb el primer CodOb generado que no sea de instruccion Word o Byte
                    if (codeDetailsDGV.Rows[i].Cells[8].Value != "" && (codeDetailsDGV.Rows[i].Cells[4].Value.ToString() != "BYTE" && codeDetailsDGV.Rows[i].Cells[4].Value.ToString() != "WORD"))
                        CodObE = codeDetailsDGV.Rows[i].Cells[2].Value.ToString().PadLeft(6, '0');
                    return CodObE;
                }
            }
            else //END tiene etiqueta, por tanto hay que guardarla y buscarla en el TABSIM
            {
                string etiq = codeDetailsDGV.Rows[codeDetailsDGV.Rows.Count - 1].Cells[5].Value.ToString(); //Guardado de la etiqueta de END
                int i = 0;
                if (TabSim.ContainsKey(etiq))
                {
                    return Convert.ToString(Convert.ToInt32(TabSim[etiq]), 16).PadLeft(6, '0');
                }
                else
                    return "FFFFFF";
            }
            return "";
        }

        string registroH()
        {

            return "000000" + codeDetailsDGV.Rows[codeDetailsDGV.Rows.Count -1].Cells[2].Value.ToString().PadLeft(6, '0');
        }
        bool isDirective(string s1)
        {
            if (s1 == "RESB" || s1 == "RESW" || s1 == "ORG")
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "s files (*.s)|*.s|All Files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.archivo = sfd.FileName;
                File.WriteAllText(sfd.FileName, codeTB.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            codeTB.Text = "";
            codeDetailsDGV.Rows.Clear();
            tabSimDGV.Rows.Clear();
            errorsTB.Text = "";
        }
    }
}
