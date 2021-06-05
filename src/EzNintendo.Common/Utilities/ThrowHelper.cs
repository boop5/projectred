using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace EzNintendo.Common.Utilities
{
    public sealed class ThrowHelper
    {
        public ThrowHelper IfAny<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source.Any(predicate))
            {
                throw new ArgumentOutOfRangeException();
            }

            return this;
        }

        public ThrowHelper IfNull(object value, string parameter)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameter, $"The value of '{parameter}' is null.");
            }

            return this;
        }

        public ThrowHelper IfNullOrEmpty(string value, string parameter)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameter, $"The value of '{parameter}' is null or empty.");
            }

            return this;
        }

        public ThrowHelper IfMoreThan<TType>(IEnumerable<TType> enumerable, int count, string parameter)
        {
            if (enumerable.Count() > count)
            {
                throw new ArgumentOutOfRangeException(parameter, $"List cant contain more than {count} items");
            }

            return this;
        }

        public ThrowHelper IfLessThan(IEnumerable<object> enumerable, int count, string parameter)
        {
            if (enumerable.Count() < count)
            {
                throw new ArgumentOutOfRangeException(parameter, $"List cant contain less than {count} items");
            }

            return this;
        }

        public ThrowHelper IfOutOfRange(int number, string name, int min, int max)
        {
            if (number < min || number > max)
            {
                throw new ArgumentOutOfRangeException($"Value {name} is out of Range ({min}-{max})");
            }

            return this;
        }

        public void IfPathDoesNotExist(string path, IFileSystem fs = null)
        {
            if (fs == null)
            {
                fs = new FileSystem();
            }

            if (!fs.Directory.Exists(path))
            {
                throw new PathNotFoundException($"Path not found {path}");
            }
        }
    }
}