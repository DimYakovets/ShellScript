using ShellScript.Lexer;
using ShellScript.MemoryModel;
using System.Collections.Generic;
using System.Linq;

namespace ShellScript.Parser
{
    class ExpressionParser
    {
        private MemoryManager Memory;
        private int Index = 0;
        public ExpressionParser(MemoryManager memory)
        {
            Memory = memory;
        }

        public Expression[] Parse(Token[] Tokens, int count = -1)
        {
            var expressions = new List<Expression>();

            for (int i = 0; i < Tokens.Length; i++)
            {
                if (count > 0 && expressions.Count == count)
                {
                    Index = i;
                    return expressions.ToArray();
                }
                if (Tokens[i].Type == TokenType.word && Tokens[i].Text == "if")
                {
                    i += 2;
                    var condition = new List<Token>();
                    int layer = 0;
                    while (!(Tokens[i].Type == TokenType.op_parentheses_close && layer == 0))
                    {
                        if (Tokens[i].Type == TokenType.op_parentheses_open)
                        {
                            layer++;
                        }
                        else if (Tokens[i].Type == TokenType.op_parentheses_close)
                        {
                            layer--;
                        }
                        condition.Add(Tokens[i++]);
                    }
                    i++;
                    var body = Parse(Tokens.Skip(i).ToArray(), 1).First();
                    i += Index;
                    if (Tokens[i].Type == TokenType.word && Tokens[i].Text == "else")
                    {
                        i++;
                        var elseBody = Parse(Tokens.Skip(i).ToArray(), 1).First();
                        expressions.Add(new IfExpression(new MathExpression(condition.ToArray(), Memory), body, elseBody));
                        i += Index;
                    }
                    else
                    {
                        expressions.Add(new IfExpression(new MathExpression(condition.ToArray(), Memory), body));
                    }
                }
                else if (Tokens[i].Type == TokenType.word && Tokens[i].Text == "while")
                {
                    i += 2;
                    var condition = new List<Token>();
                    int layer = 0;
                    while (!(Tokens[i].Type == TokenType.op_parentheses_close && layer == 0))
                    {
                        if (Tokens[i].Type == TokenType.op_parentheses_open)
                        {
                            layer++;
                        }
                        else if (Tokens[i].Type == TokenType.op_parentheses_close)
                        {
                            layer--;
                        }
                        condition.Add(Tokens[i++]);
                    }
                    i++;
                    var body = Parse(Tokens.Skip(i).ToArray(), 1).First();
                    i += Index;
                    expressions.Add(new WhileExpression(new MathExpression(condition.ToArray(), Memory), body));
                }
                else if (Tokens[i].Type == TokenType.word)
                {
                    i++;
                    if (Tokens[i].Type == TokenType.word)
                    {
                        var type = Tokens[i - 1];
                        var name = Tokens[i++];
                        var expression = new Token[0];
                        if (Tokens[i++].Type == TokenType.op_equals)
                        {
                            var exp = new List<Token>();
                            while (Tokens[i].Type != TokenType.op_semicolon)
                            {
                                exp.Add(Tokens[i++]);
                            }
                            expression = exp.ToArray();
                        }
                        expressions.Add(new DefineVariableExpression(name, type, expression, Memory));
                    }
                    else if (Tokens[i].Type == TokenType.op_equals)
                    {
                        var name = Tokens[i++ - 1];
                        var exp = new List<Token>();
                        while (Tokens[i].Type != TokenType.op_semicolon)
                        {
                            exp.Add(Tokens[i++]);
                        }
                        expressions.Add(new SetValueExpression(name, exp.ToArray(), Memory));
                    }
                }
                else if (Tokens[i].Type == TokenType.op_brace_open)
                {
                    var exp = new List<Token>();
                    i++;
                    int layer = 1;
                    while (!(Tokens[i].Type == TokenType.op_brace_close && layer == 1))
                    {
                        if (Tokens[i].Type == TokenType.op_brace_open)
                        {
                            layer++;
                        }
                        else if (Tokens[i].Type == TokenType.op_brace_close)
                        {
                            layer--;
                        }
                        exp.Add(Tokens[i++]);
                    }
                    expressions.Add(new BodyExpression(Parse(exp.ToArray())));
                }
            }
            return expressions.ToArray();
        }
    }
}
