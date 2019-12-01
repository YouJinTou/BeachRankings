﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Tools
{
    public static class Validator
    {
        public static void ThrowIfNull(object obj, string message = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), message ?? "Parameter is null.");
            }
        }

        public static void ThrowIfNullOrWhiteSpace<T>(T obj, string message = null)
        {
            ThrowIfNull(obj, message);

            if (obj is string && string.IsNullOrWhiteSpace(obj as string))
            {
                throw new ArgumentNullException(
                    nameof(obj), "Object is null or white space." ?? message);
            }
        }

        public static T ReturnOrThrowIfNullOrWhiteSpace<T>(T obj)
        {
            ThrowIfNullOrWhiteSpace(obj);

            return obj;
        }

        public static void ThrowIfAnyNullOrWhiteSpace<T>(params T[] items)
        {
            foreach (var item in items)
            {
                ThrowIfNullOrWhiteSpace(item, "An item in the collection is null or white space.");
            }
        }

        public static bool AllNull<T>(IEnumerable<T> items)
        {
            return items.All(i => i == null);
        }
    }
}
