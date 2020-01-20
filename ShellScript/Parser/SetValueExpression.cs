using ShellScript.Lexer;
using ShellScript.MemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser
{
    class SetValueExpression : Expression
    {
        private Token Name;
        private Token[] Expression;
        private MemoryManager Memory;

        public SetValueExpression(Token name, Token[] expression, MemoryManager memory)
        {
            Name = name;
            Expression = expression;
            Memory = memory;
        }

        public override ExpressionResult Invoke()
        {
            var result = new MathExpression(Expression, Memory).Invoke().Data;
            Memory.SetValue(result, Name.Text);
            return ExpressionResult.Default;
        }
    }
}
