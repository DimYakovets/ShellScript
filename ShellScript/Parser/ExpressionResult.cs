using ShellScript.MemoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellScript.Parser
{
    enum ResultType
    {
        EMPTY,
        BREAK,
        RETURN,
        CONTINUE,
        MATH
    }
    class ExpressionResult
    {
        public static readonly ExpressionResult Default = new ExpressionResult(ResultType.EMPTY, null);
        public ResultType Type { get; }
        public ObjectModel Data { get; }
        public ExpressionResult(ResultType type, ObjectModel data)
        {
            Type = type;
            Data = data;
        }
    }
}
