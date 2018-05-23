using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ES.Utility
{
    public class OperationResult
    {
        public OperationResultType ResultType { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public OperationResult(OperationResultType resultType)
        {
            ResultType = resultType;
            Message = null;
            Data = null;
        }
        public OperationResult(OperationResultType resultType, string message)
        {
            ResultType = resultType;
            Message = message;
            Data = null;
        }
        public OperationResult(OperationResultType resultType, string message, object data)
        {
            ResultType = resultType;
            Message = message;
            Data = data;
        }
    }
}
