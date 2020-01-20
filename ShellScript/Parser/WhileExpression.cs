using ShellScript.MemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser
{
    class WhileExpression : Expression
    {
        private readonly MathExpression Condition;
        private readonly Expression Body;

        public WhileExpression(MathExpression condition, Expression body)
        {
            Condition = condition;
            Body = body;
        }

        public override ExpressionResult Invoke()
        {
            var result = Condition.Invoke().Data;
            if (result.Type != ObjectModel.BOOL)
            {
                throw new Exception();
            }
            while (result.Value == "true")
            {
                var res = Body.Invoke();
                if (res.Type == ResultType.RETURN)
                {

                }
                result = Condition.Invoke().Data;
            }

            return ExpressionResult.Default;
        }
    }
}
