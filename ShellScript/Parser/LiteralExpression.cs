using ShellScript.Lexer;
using ShellScript.MemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser
{
    class LiteralExpression : Expression
    {
        private readonly Token Literal;
        private readonly MemoryManager Memory;

        public LiteralExpression(Token literal, MemoryManager memory)
        {
            Literal = literal;
            Memory = memory;
        }
        public override ExpressionResult Invoke()
        {
            if (Literal.Type == TokenType.literal_int)
            {
                return new ExpressionResult(ResultType.MATH, new ObjectModel(ObjectModel.INT, Literal.Text, null));
            }
            else if (Literal.Type == TokenType.literal_float)
            {
                return new ExpressionResult(ResultType.MATH, new ObjectModel(ObjectModel.FLOAT, Literal.Text, null));
            }
            else if (Literal.Type == TokenType.literal_bool)
            {
                return new ExpressionResult(ResultType.MATH, new ObjectModel(ObjectModel.BOOL, Literal.Text, null));
            }
            else if (Literal.Type == TokenType.literal_string)
            {
                return new ExpressionResult(ResultType.MATH, new ObjectModel(ObjectModel.STRING, Literal.Text, null));
            }
            else if (Literal.Type == TokenType.word)
            {
                return new ExpressionResult(ResultType.MATH, Memory.GetValue(Literal.Text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)));
            }
            throw new NotImplementedException();
        }
    }
}