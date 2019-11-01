using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Config
{
    public static class FizzleSettings
    {
        private static bool redirected;

        internal static Dictionary<string, List<string>> CommandArgs { get; }

        static FizzleSettings()
        {
            CommandArgs = new Dictionary<string, List<string>>();
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            string index1 = null;
            List<string> stringList = null;
            for (int index2 = 1; index2 < commandLineArgs.Length; ++index2)
            {
                if (commandLineArgs[index2].StartsWith("-") && commandLineArgs[index2].Length > 1)
                {
                    if (index1 != null)
                    {
                        CommandArgs[index1] = stringList;
                    }

                    stringList = new List<string>();
                    index1 = commandLineArgs[index2].Substring(1, commandLineArgs[index2].Length - 1);
                }
                else
                {
                    stringList?.Add(commandLineArgs[index2]);
                }
            }

            if (index1 == null)
            {
                return;
            }

            CommandArgs[index1] = stringList;
        }

        internal static void RedirectException()
        {
            if (redirected)
            {
                return;
            }

            redirected = true;
            Application.logMessageReceived += (Application.LogCallback) ((name, stack, type) =>
            {
                switch (type)
                {
                    case LogType.Exception:
                        FizzleDebug.LogException(name, stack);
                        break;
                    case LogType.Error:
                    case LogType.Assert:
                        FizzleDebug.LogError(name);
                        break;
                    case LogType.Log:
                        FizzleDebug.Log(name);
                        break;
                    case LogType.Warning:
                        FizzleDebug.LogWarning(name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            });
        }

        internal static List<KeyValuePair<string, string>> LoadSceneInfo(string path, out string levelId)
        {
            FizzleJson fizzleJson1 = PythonGenerator.LoadYaml(path + "/level-info.yml");
            Directory.SetCurrentDirectory(new DirectoryInfo(CommonTools.ConvertPath(path)).FullName);
            levelId = fizzleJson1["level-id"].ToString();
            if (levelId == "official")
            {
                levelId = "official0";
            }
            FizzleJson fizzleJson2 = fizzleJson1["level-mapping"];
            List<KeyValuePair<string, string>> keyValuePairList2 = new List<KeyValuePair<string, string>>(fizzleJson2.Count());
            keyValuePairList2.AddRange(InternalLoadSceneInfo(fizzleJson2).CreateEnumerable());
            return keyValuePairList2;
        }

        [SuppressMessage("ReSharper", "LoopCanBePartlyConvertedToQuery")]
        private static IEnumerator<KeyValuePair<string, string>> InternalLoadSceneInfo(FizzleJson levelMapping)
        {
            FizzlePython python = FizzleScene.Python;
            dynamic customLevelGenerator = python.GetVariable("CustomLevelGenerator");
            foreach (FizzleJson fizzleJson in levelMapping)
            {
                string levelConfig = CommonTools.ConvertPath(fizzleJson["level-config"].ToString());
                string levelLayout = CommonTools.ConvertPath(fizzleJson["level-layout"].ToString());
                dynamic gen = customLevelGenerator(levelConfig, levelLayout);
                yield return new KeyValuePair<string, string>(python.to_json(gen.get_config()), python.to_json(gen.get_object_list()));
            }
            python.RemoveVariable("CustomLevelGenerator");
            python.RemoveVariable("xlrd");
        }
    }
}
