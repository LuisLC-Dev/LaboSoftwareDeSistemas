using Antlr4.Runtime;
using System;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica3
{
    class ErrorListener : Antlr4.Runtime.BaseErrorListener, IAntlrErrorListener<int>
    {
        // Método llamado cuando se detecta un error sintáctico en el Parser
        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            // Agrega el número de línea del error a una lista en Form1
            SicxeForm.errorList.Add(line.ToString() + ": " + msg);
        }

        // Método llamado cuando se detecta un error léxico en el Lexer
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            // Agrega el número de línea del error a una lista en Form1
            SicxeForm.errorList.Add(line.ToString() + ": " + msg);
        }
    }
}
