using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser.CalculatedExpressions
{
    class UnaryExpression : Expression
    {
        private OperationType Type;
        private object Operand;
        public override ExpressionResult Invoke()
        {
            throw new NotImplementedException();
        }
    }
}
