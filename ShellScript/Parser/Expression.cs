namespace ShellScript.Parser
{
    abstract class Expression
    {
        public Expression Operator { get; set; }
        abstract public ExpressionResult Invoke();
    }
}
