using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using Debug = UnityEngine.Debug;

namespace FizzlePuzzle.Utility
{
    internal static class FizzleDebug
    {
        private static readonly string path;

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        static FizzleDebug()
        {
            string dir = CommonTools.ConvertPath("~/Logs/");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            path = dir + DateTime.Now.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo) + ".txt";
        }

        private static string Produce(object content, string prefix)
        {
            string dateTimeBar = ": (" + DateTime.Now.ToString("O") + ")\n";
            return new StringBuilder((content + string.Empty).Trim()).Insert(0, dateTimeBar).Insert(0, prefix).ToString();
        }
        
        private static void Show(object message, FizzleColor color)
        {
            FizzleScene.FizzleView?.fizzleConsole?.ShowMessage($"<color={color}>{message}</color>");
        }

        private static void Write(object message)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message + "\n");
            }
        }
        
        internal static void LogColor(FizzleColor color, object message)
        {
            Show(message = Produce(message, "LOG"), color);
            Write(message);
        }

        internal static void Log(object message)
        {
            Show(message = Produce(message, "LOG"), "#FFFFFF");
            Write(message);
        }

        internal static void LogError(object message)
        {
            Show(message = Produce(message, "ERROR"), "#FF0000");
            Write(message);
        }
        
        internal static void LogException(string name, string stack)
        {
            Show(name = Produce(name, "EXCEPTION"), "#FF0000");
            Write(name + "\n" + (stack + string.Empty).Trim());
        }
        
        internal static void LogException(Exception e)
        {
            Debug.LogException(e);
        }
        
        internal static void LogWarning(object message)
        {
            Show(message = Produce(message, "WARNING"), "#FFFF00");
            Write(message);
        }
    }
}
