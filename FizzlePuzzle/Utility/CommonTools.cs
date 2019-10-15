using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FizzlePuzzle.Utility
{
    internal static class CommonTools
    {
        private static char Separator => Path.DirectorySeparatorChar;

        internal static string ConvertPath(string path)
        {
            path = path.Trim();
            if (path.StartsWith("~"))
            {
                path = Application.dataPath + path.Remove(0, 1);
            }
            const string pattern = @"[\\\/]+";
            path = Regex.Replace(path, pattern, Separator.ToString());
            return new DirectoryInfo(path).FullName;
        }

        internal static void PlayRandomSound(AudioSource audioSource, List<AudioClip> audioClips)
        {
            if (audioClips.Count == 0)
            {
                return;
            }

            int index = UnityEngine.Random.Range(0, audioClips.Count);
            PlayOneSound(audioSource, audioClips[index]);
        }

        internal static void PlayOneSound(AudioSource audioSource, AudioClip audioClip)
        {
            if (!audioClip)
            {
                return;
            }
            audioSource.clip = audioClip;
            audioSource.time = audioSource.pitch >= 0.0 ? 0.0f : audioClip.length;
            audioSource.Play();
        }

        internal static MethodBase GetCurrentMethod()
        {
            return GetCallerMethod();
        }

        internal static string GetCurrentLineInfo()
        {
            return GetCallerLineInfo();
        }

        internal static string GetCallerLineInfo()
        {
            StackTrace stackTrace = new StackTrace(true);
            if (stackTrace.FrameCount < 3)
            {
                return null;
            }

            StackFrame frame = stackTrace.GetFrame(2);
            Type declaringType = frame.GetMethod().DeclaringType;
            string str1 = declaringType != null ? declaringType.FullName : null;
            string name = frame.GetMethod().Name;
            int fileLineNumber = frame.GetFileLineNumber();
            string fileName = frame.GetFileName();
            string str2 = fileName?.Split(Separator).Last();
            return $"{str2}:{fileLineNumber} -> {str1}:{name}";
        }

        internal static MethodBase GetCallerMethod()
        {
            StackTrace stackTrace = new StackTrace();
            return stackTrace.FrameCount >= 3 ? stackTrace.GetFrame(2).GetMethod() : null;
        }
    }
}