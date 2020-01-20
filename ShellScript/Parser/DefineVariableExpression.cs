using ShellScript.Lexer;
using ShellScript.MemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser
{
    class DefineVariableExpression : Expression
    {
        private Token Name;
        private Token Type;
        private Token[] Expression;
        private MemoryManager Memory;

        public DefineVariableExpression(Token name, Token type, Token[] expression, MemoryManager memory)
        {
            Name = name;
            Type = type;
            Memory = memory;
            Expression = expression;
        }

        public override ExpressionResult Invoke()
        {
            Memory.Create(Name.Text, Type.Text);
            if (Expression.Length > 0)
            {
                var result = new MathExpression(Expression, Memory).Invoke().Data;
                Memory.SetValue(result, Name.Text);
            }
            return ExpressionResult.Default;
        }
    }
}
