using ShellScript.MemoryModel;
using ShellScript.Parser;
using ShellScript.SyntaxAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript
{
    public class Shell
    {
        private readonly Lexer.Lexer Lexer;
        private readonly ExpressionParser Parser;
        private readonly MemoryManager Memory;

        public Shell()
        {
            Lexer = new Lexer.Lexer();
            Memory = new MemoryManager();
            Parser = new ExpressionParser(Memory);
        }
        public void DoString(string code)
        {
            var tokens = Lexer.Tokinaze(code);
            var expressions = Parser.Parse(tokens);
            foreach (var item in expressions)
            {
                item.Invoke();
            }
        }
        public void ClearMemory()
        {
            Memory.Clear();
        }
    }
}