using System;

namespace ShellScript.Parser
{
    class BodyExpression : Expression
    {
        private readonly Expression[] Body;
        public BodyExpression(Expression[] body)
        {
            Body = body;
        }
        public override ExpressionResult Invoke()
        {
            foreach (var item in Body)
            {
                var result = item.Invoke();
                if (result.Type != ResultType.EMPTY)
                {
                    return result;
                }
            }
            return ExpressionResult.Default;
        }
    }
}