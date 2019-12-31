using BR.Core.Models;
using System;

namespace BR.Core.Extensions
{
    public static class NoSqlQueryOperatorExtensions
    {
        public static string AsDynamoString(this NoSqlQueryOperator op)
        {
            switch (op)
            {
                case NoSqlQueryOperator.BeginsWith:
                    return "begins_with";
                default:
                    throw new InvalidOperationException("Operator not found.");
            }
        }
    }
}
