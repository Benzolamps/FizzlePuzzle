using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FizzlePuzzle.Utility
{
    internal class FizzleJson : IEnumerable<FizzleJson>
    {
        private readonly JsonData data;

        internal FizzleJson(JsonData data)
        {
            this.data = data;
        }

        internal FizzleJson(string json)
        {
            this.data = JsonMapper.ToObject(json);
        }

        internal bool IsArray => data.IsArray;

        internal FizzleJson this[int index] => new FizzleJson(data[index]);

        internal FizzleJson this[string key]
        {
            get
            {
                try
                {
                    return new FizzleJson(data[key]);
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException(key);
                }
            }
        }

        public IEnumerator<FizzleJson> GetEnumerator()
        {
            for (int i = 0; i < data.Count; ++i)
            {
                yield return new FizzleJson(data[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal T GetOrDefault<T>(string key, T defaultValue)
        {
            try
            {
                return this[key].ToObject<T>();
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        internal string ToJson()
        {
            return this.data.ToJson();
        }

        public override string ToString()
        {
            return data.ToString();
        }

        internal T ToObject<T>()
        {
            if (typeof(T).IsPrimitive)
            {
                MethodInfo method = typeof(T).GetMethod("Parse", new[] {typeof(string)});
                return (T) ((method != null) ? method.Invoke(null, new object[] {ToString()}) : null);
            }

            if (typeof(T) == typeof(string))
            {
                return (T) (object) ToString();
            }

            return JsonMapper.ToObject<T>(ToJson());
        }
    }
}