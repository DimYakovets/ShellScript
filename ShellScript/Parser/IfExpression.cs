using ShellScript.MemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser
{
    class IfExpression : Expression
    {
        private readonly MathExpression Condition;
        private readonly Expression Body;
        private readonly Expression ElseBody;

        public IfExpression(MathExpression condition, Expression body, Expression elseBody)
        {
            Condition = condition;
            Body = body;
            ElseBody = elseBody;
        }
        public IfExpression(MathExpression condition, Expression body)
        {
            Condition = condition;
            Body = body;
        }

        public override ExpressionResult Invoke()
        {
            var result = Condition.Invoke().Data;
            if (result.Type == ObjectModel.BOOL)
            {
                if (result.Value == "true")
                {
                    return Body.Invoke();
                }
                else
                {
                    return ElseBody == null ? ExpressionResult.Default : ElseBody.Invoke();
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
