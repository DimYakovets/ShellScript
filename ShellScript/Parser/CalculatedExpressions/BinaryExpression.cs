using ShellScript.Lexer;
using ShellScript.MemoryModel;
using ShellScript.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser.CalculatedExpressions
{
    enum OperationType
    {
        Sum,
        Substract,
        Division,
        Multiplication,
        Power,
        Increment,
        Decrement,
        Equals,
        NotEquals,
        Less,
        LessEquals,
        Greater,
        GreaterEquals
    }
    class BinaryExpression : Expression
    {
        private readonly OperationType Operation;
        private readonly MemoryManager Memory;
        private readonly object Operand1;
        private readonly object Operand2;

        public BinaryExpression(OperationType operation, object operand1, object operand2, MemoryManager memory)
        {
            Operation = operation;
            Operand1 = operand1;
            Operand2 = operand2;
            Memory = memory;
        }
        public override ExpressionResult Invoke()
        {
            ObjectModel op1 = Operand1 as ObjectModel;
            ObjectModel op2 = Operand2 as ObjectModel;
            if (Operand1 is Expression exp)
            {
                op1 = exp.Invoke().Data;
            }
            else if (Operand1 is Token token)
            {
                if (token.Type == TokenType.literal_int)
                {
                    op1 = new ObjectModel(ObjectModel.INT, token.Text, null);
                }
                else if (token.Type == TokenType.literal_float)
                {
                    op1 = new ObjectModel(ObjectModel.FLOAT, token.Text, null);
                }
                else if (token.Type == TokenType.literal_bool)
                {
                    op1 = new ObjectModel(ObjectModel.BOOL, token.Text, null);
                }
                else if (token.Type == TokenType.literal_string)
                {
                    op1 = new ObjectModel(ObjectModel.STRING, token.Text, null);
                }
                else if (token.Type == TokenType.word)
                {
                    op1 = Memory.GetValue(token.Text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            if (Operand2 is Expression exp2)
            {
                op2 = exp2.Invoke().Data;
            }
            else if (Operand2 is Token token)
            {
                if (token.Type == TokenType.literal_int)
                {
                    op2 = new ObjectModel(ObjectModel.INT, token.Text, null);
                }
                else if (token.Type == TokenType.literal_float)
                {
                    op2 = new ObjectModel(ObjectModel.FLOAT, token.Text.Replace(".", ","), null);
                }
                else if (token.Type == TokenType.literal_bool)
                {
                    op2 = new ObjectModel(ObjectModel.BOOL, token.Text, null);
                }
                else if (token.Type == TokenType.literal_string)
                {
                    op2 = new ObjectModel(ObjectModel.STRING, token.Text, null);
                }
                else if (token.Type == TokenType.word)
                {
                    op2 = Memory.GetValue(token.Text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }

            if (Operation == OperationType.Multiplication)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.Multiplication(op1, op2));
            }
            else if (Operation == OperationType.Division)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.Division(op1, op2));
            }
            else if (Operation == OperationType.Sum)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.Sum(op1, op2));
            }
            else if (Operation == OperationType.Substract)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.Substract(op1, op2));
            }
            else if (Operation == OperationType.Power)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.Power(op1, op2));
            }
            else if (Operation == OperationType.Equals)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.Equals(op1, op2));
            }
            else if (Operation == OperationType.Greater)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.GreaterThan(op1, op2));
            }
            else if (Operation == OperationType.Less)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.LessThan(op1, op2));
            }
            else if (Operation == OperationType.GreaterEquals)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.GreaterThan(op1, op2));
            }
            else if (Operation == OperationType.LessEquals)
            {
                return new ExpressionResult(ResultType.MATH, ObjectModel.LessThanEquals(op1, op2));
            }
            throw new NotImplementedException();
        }
    }
}