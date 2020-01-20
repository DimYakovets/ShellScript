using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Lexer
{
    class Token
    {
        public int Line { get; private set; }
        public int Column { get; private set; }
        public TokenType Type { get; private set; }
        public string Text { get; private set; }
        public Token(TokenType type, string data)
        {
            Type = type;
            Text = data;
            Line = 0;
            Column = 0;
        }
        public Token(TokenType type, string data, int line, int column)
        {
            Type = type;
            Text = data;
            Line = line;
            Column = column;
        }
        public override string ToString()
        {
            return $"'{Text}' : ({Line},{Column})";
        }
    }
}
