using System.Collections.Generic;

namespace FizzlePuzzle.Extension
{
    internal static class ObjectExtension
    {
        private static readonly Dictionary<object, Dictionary<string, bool>> data = new Dictionary<object, Dictionary<string, bool>>();

        internal static T Cast<T>(this object obj)
        {
            return (T) obj;
        }

        internal static bool StatusDetect(this object obj, string key, bool given, bool desire)
        {
            if (!data.ContainsKey(obj))
            {
                data[obj] = new Dictionary<string, bool>();
            }

            if (!data[obj].ContainsKey(key))
            {
                data[obj][key] = false;
            }

            if (given != data[obj][key])
            {
                return (data[obj][key] = given) == desire;
            }

            return false;
        }
    }
}
