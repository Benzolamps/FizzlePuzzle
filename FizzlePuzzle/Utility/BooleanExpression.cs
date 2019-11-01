using FizzlePuzzle.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace FizzlePuzzle.Utility
{
    internal class BooleanExpression
    {
        private static readonly char[] operators = {'&', '|', '~', '^'};

        private static readonly Regex blankRegex = new Regex("[\\s ]");

        private readonly string exp;

        private readonly List<string> varsAndOpts;

        private Func<string, bool> test;

        private bool excepted;

        internal BooleanExpression(string exp)
        {
            this.exp = exp;
            varsAndOpts = new List<string>();
            GenerateVariablesAndOperators();
        }

        private void GenerateVariablesAndOperators()
        {
            bool flag1 = false;
            bool flag2 = false;
            int startIndex = 0;
            for (int index = 0; index < exp.Length; ++index)
            {
                if (exp[index] == '\'')
                {
                    if (flag2)
                    {
                        flag2 = false;
                        string str = exp.Substring(startIndex, index - startIndex).Trim();
                        if (str.Length != 0)
                        {
                            varsAndOpts.Add(str);
                        }

                        flag1 = false;
                        startIndex = index + 1;
                    }
                    else
                    {
                        if (index != 0 && !flag1)
                        {
                            excepted = true;
                            throw new FizzleException("需要操作符, 得到变量");
                        }

                        flag2 = true;
                        startIndex = index + 1;
                    }
                }

                if (flag2)
                {
                    continue;
                }
                char ch = exp[index];
                string input = ch.ToString();
                if (blankRegex.IsMatch(input))
                {
                    continue;
                }
                if (operators.Contains(exp[index]))
                {
                    if (exp[index] == '~')
                    {
                        if (index != 0 && !flag1)
                        {
                            excepted = true;
                            throw new FizzleException("需要操作符, 得到变量");
                        }
                    }
                    else if (index == 0 | flag1)
                    {
                        excepted = true;
                        throw new FizzleException("需要变量, 得到操作符");
                    }

                    string str1 = exp.Substring(startIndex, index - startIndex).Trim();
                    if (str1.Length != 0)
                    {
                        varsAndOpts.Add(str1);
                    }

                    ch = exp[index];
                    string str2 = ch.ToString();
                    varsAndOpts.Add(str2);
                    startIndex = index + 1;
                    flag1 = true;
                }
                else
                {
                    flag1 = false;
                }
            }

            if (flag1)
            {
                excepted = true;
                throw new FizzleException("需要操作符, 得到结尾");
            }

            string str3 = exp.Substring(startIndex, exp.Length - startIndex).Trim();
            if (str3.Length == 0)
            {
                return;
            }

            varsAndOpts.Add(str3);
        }

        internal IEnumerable<string> GetVariables()
        {
            return varsAndOpts?.Where(str =>
            {
                if (str.Length == 1)
                {
                    return !operators.Contains(str[0]);
                }

                return true;
            }) ?? new List<string>();
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        internal bool CalcResult(Func<string, bool> mapping)
        {
            if (excepted)
            {
                return false;
            }

            List<string> list = new List<string>(varsAndOpts);
            test = str =>
            {
                try
                {
                    return mapping(str);
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        return bool.Parse(str);
                    }
                    catch (FormatException)
                    {
                        excepted = true;
                        throw new KeyNotFoundException(str);
                    }
                }
            };
            for (int index = 0; index < list.Count; ++index)
            {
                if (list[index] == "~")
                {
                    SingleOperate(ref index, list, b => !b);
                }
            }

            for (int index = 0; index < list.Count; ++index)
            {
                if (list[index] == "&")
                {
                    DoubleOperate(ref index, list, (a, b) => a & b);
                }
            }

            for (int index = 0; index < list.Count; ++index)
            {
                if (list[index] == "|")
                {
                    DoubleOperate(ref index, list, (a, b) => a | b);
                }
                else if (list[index] == "^")
                {
                    DoubleOperate(ref index, list, (a, b) => a ^ b);
                }
            }

            return list.Count == 0 || test(list[0]);
        }

        private void SingleOperate(ref int index, IList<string> list, Func<bool, bool> convert)
        {
            bool flag = convert(test(list[index + 1]));
            list.RemoveAt(index);
            list[index] = flag.ToString();
        }

        private void DoubleOperate(ref int index, IList<string> list, Func<bool, bool, bool> convert)
        {
            bool flag = convert(test(list[index - 1]), test(list[index + 1]));
            list.RemoveAt(index--);
            list.RemoveAt(index);
            list[index] = flag.ToString();
        }
    }
}
