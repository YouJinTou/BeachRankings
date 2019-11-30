﻿using Amazon.DynamoDBv2.DocumentModel;
using System;

namespace BR.Core.System
{
    public static class Types
    {
        public static readonly Type String = typeof(string);
        public static readonly Type Int = typeof(int);
        public static readonly Type Long = typeof(long);
        public static readonly Type DateTime = typeof(DateTime);
        public static readonly Type Guid = typeof(Guid);
        public static readonly Type NullDouble = typeof(double?);
        public static readonly Type ByteArray = typeof(byte[]);
        public static readonly DynamoDBNull DynamoNull = new DynamoDBNull();
    }
}
