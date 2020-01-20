using ShellScript.Lexer;
using ShellScript.MemoryModel;
using ShellScript.Parser.CalculatedExpressions;
using System;
using System.Collections;
using System.Linq;

namespace ShellScript.Parser
{
    class MathExpression : Expression
    {
        MemoryManager Memory;
        private readonly Expression Expression;
        public MathExpression(Token[] tokens, MemoryManager memory)
        {
            Memory = memory;
            ParenthesesCheck(ref tokens);
            if (tokens.Length > 1)
            {
                Expression = GetExpression(tokens);
            }
            else
            {
                Expression = new LiteralExpression(tokens[0], Memory);
            }
        }
        private Expression GetExpression(Token[] tokens)
        {
            int layer = 0;
            //> < >= <= ==
            for (int i = tokens.Length - 1; i > -1; i--)
            {
                if (tokens[i].Type == TokenType.op_parentheses_open)
                {
                    layer++;
                }
                else if (tokens[i].Type == TokenType.op_parentheses_close)
                {
                    layer--;
                }
                if ((tokens[i].Type == TokenType.op_double_equals ||
                     tokens[i].Type == TokenType.op_greater_equals ||
                     tokens[i].Type == TokenType.op_less_equals ||
                     tokens[i].Type == TokenType.op_angle_breackets_close ||
                     tokens[i].Type == TokenType.op_angle_breackets_open) && layer == 0)
                {
                    OperationType operation = OperationType.Equals;
                    if (tokens[i].Type == TokenType.op_double_equals)
                    {
                        operation = OperationType.Equals;
                    }
                    else if(tokens[i].Type == TokenType.op_greater_equals)
                    {
                        operation = OperationType.GreaterEquals;
                    }
                    else if (tokens[i].Type == TokenType.op_less_equals)
                    {
                        operation = OperationType.LessEquals;
                    }
                    else if (tokens[i].Type == TokenType.op_angle_breackets_close)
                    {
                        operation = OperationType.Greater;
                    }
                    else if (tokens[i].Type == TokenType.op_angle_breackets_open)
                    {
                        operation = OperationType.Less;
                    }
                    var leftpart = tokens.Take(i).ToArray();
                    var rightpart = tokens.Skip(i + 1).ToArray();
                    ParenthesesCheck(ref leftpart);
                    ParenthesesCheck(ref rightpart);
                    object op1 = leftpart.Length > 1 ? (object)GetExpression(leftpart) : (object)leftpart[0];
                    object op2 = rightpart.Length > 1 ? (object)GetExpression(rightpart) : (object)rightpart[0];
                    return new BinaryExpression(operation, op1, op2, Memory);
                }
            }
            //-+
            for (int i = tokens.Length - 1; i > -1; i--)
            {
                if (tokens[i].Type == TokenType.op_parentheses_open)
                {
                    layer++;
                }
                else if (tokens[i].Type == TokenType.op_parentheses_close)
                {
                    layer--;
                }
                if ((tokens[i].Type == TokenType.op_plus || tokens[i].Type == TokenType.op_minus) && layer == 0)
                {
                    var leftpart = tokens.Take(i).ToArray();
                    var rightpart = tokens.Skip(i + 1).ToArray();
                    ParenthesesCheck(ref leftpart);
                    ParenthesesCheck(ref rightpart);
                    object op1 = leftpart.Length > 1 ? (object)GetExpression(leftpart) : (object)leftpart[0];
                    object op2 = rightpart.Length > 1 ? (object)GetExpression(rightpart) : (object)rightpart[0];
                    return new BinaryExpression(tokens[i].Type == TokenType.op_plus ? OperationType.Sum : OperationType.Substract, op1, op2, Memory);
                }
            }
            //*/
            layer = 0;
            for (int i = tokens.Length - 1; i > -1; i--)
            {
                if (tokens[i].Type == TokenType.op_parentheses_open)
                {
                    layer++;
                }
                else if (tokens[i].Type == TokenType.op_parentheses_close)
                {
                    layer--;
                }
                if ((tokens[i].Type == TokenType.op_asterisk || tokens[i].Type == TokenType.op_slash) && layer == 0)
                {
                    var leftpart = tokens.Take(i).ToArray();
                    var rightpart = tokens.Skip(i + 1).ToArray();
                    ParenthesesCheck(ref leftpart);
                    ParenthesesCheck(ref rightpart);
                    object op1 = leftpart.Length > 1 ? (object)GetExpression(leftpart) : (object)leftpart[0];
                    object op2 = rightpart.Length > 1 ? (object)GetExpression(rightpart) : (object)rightpart[0];
                    return new BinaryExpression(tokens[i].Type == TokenType.op_asterisk ? OperationType.Multiplication : OperationType.Division, op1, op2, Memory);
                }
            }
            //^
            layer = 0;
            for (int i = tokens.Length - 1; i > -1; i--)
            {
                if (tokens[i].Type == TokenType.op_parentheses_open)
                {
                    layer++;
                }
                else if (tokens[i].Type == TokenType.op_parentheses_close)
                {
                    layer--;
                }
                if (tokens[i].Type == TokenType.op_caret && layer == 0)
                {
                    var leftpart = tokens.Take(i).ToArray();
                    var rightpart = tokens.Skip(i + 1).ToArray();
                    ParenthesesCheck(ref leftpart);
                    ParenthesesCheck(ref rightpart);
                    object op1 = leftpart.Length > 1 ? (object)GetExpression(leftpart) : (object)leftpart[0];
                    object op2 = rightpart.Length > 1 ? (object)GetExpression(rightpart) : (object)rightpart[0];
                    return new BinaryExpression(OperationType.Power, op1, op2, Memory);
                }
            }
            throw new Exception();
        }
        private static void ParenthesesCheck(ref Token[] expression)
        {
            if (expression[0].Type == TokenType.op_parentheses_open && expression[expression.Length - 1].Type == TokenType.op_parentheses_close)
            {
                var stack = new Stack();
                var exp = expression.Skip(1).Take(expression.Length - 2).ToArray();
                foreach (var item in exp)
                {
                    if (item.Type == TokenType.op_parentheses_open)
                    {
                        stack.Push(item);
                    }
                    if (item.Type == TokenType.op_parentheses_close && stack.Count == 0)
                    {
                        return;
                    }
                    else if (item.Type == TokenType.op_parentheses_close && (stack.Peek() as Token).Type == TokenType.op_parentheses_open)
                    {
                        stack.Pop();
                    }
                }
                if (stack.Count == 0)
                {
                    expression = exp;
                }
            }
        }
        public override ExpressionResult Invoke()
        {
            return Expression.Invoke();
        }
    }
}