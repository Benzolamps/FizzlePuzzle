using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class CommonUtility
    {
        public static void echo(string msg, string color = "#FFFFFF")
        {
            FizzleDebug.LogColor(color, FizzleUnicode.Decode(msg));
        }

        public static void error(string msg)
        {
            FizzleDebug.LogError(FizzleUnicode.Decode(msg));
        }

        public static void warn(string msg)
        {
            FizzleDebug.LogWarning(FizzleUnicode.Decode(msg));
        }

        public static void subtitle(string msg, string color = "#FFFFFF", float seconds = 2.0F)
        {
            FizzleScene.FizzleView.fizzleSubtitle.ShowSubtitle(FizzleUnicode.Decode(msg), color, seconds);
        }

        public static WorldInfo world_info { get; } = new WorldInfo();

        public static IEnumerator delay(float seconds)
        {
            for (float i = 0.0F; i < seconds; i += 0.02F)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        public static IEnumerator while_(Func<bool> cond)
        {
            return new WaitWhile(cond);
        }

        public static IEnumerator until(Func<bool> cond)
        {
            return new WaitUntil(cond);
        }

        public static void run_async(IEnumerator coroutine)
        {
            FizzleScene.StartOneCoroutine(coroutine);
        }

        public static string convert_path(string path)
        {
            return CommonTools.ConvertPath(path);
        }

        public abstract class FizzleCoroutine : IEnumerator
        {
            private IEnumerator __enumerator;

            [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
            public abstract IEnumerator wait();

            public bool MoveNext()
            {
                __enumerator = __enumerator ?? wait();
                return __enumerator.MoveNext();
            }

            public void Reset()
            {
                __enumerator = wait();
                __enumerator.Reset();
            }

            [SuppressMessage("ReSharper", "UnusedParameter.Global")]
            public object __call__(params object[] args)
            {
                return wait();
            }

            public object Current
            {
                get
                {
                    __enumerator = __enumerator ?? wait();
                    return __enumerator.Current;
                }
            }
        }
    }
}
