using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzlePythonScript : FizzleBehaviour, FizzleItem
    {
        internal string Code { get; private set; } = string.Empty;

        [SerializeField] internal string m_Path;

        private bool? pathCode;

        public void Generate(FizzleJson data)
        {
            m_Path = data["path"].ToString();
            GenerateCode();
            FizzleDebug.Log($"FizzlePythonScript name = {(object) data["name"] ?? name}, path = {m_Path}, code-length = {Code.Length}");
        }

        internal void Execute()
        {
            GenerateCode();
            FizzleScene.Python.Execute(Code);
        }

        [SuppressMessage("ReSharper", "AssignmentInConditionalExpression")]
        private void GenerateCode()
        {
            if (!string.IsNullOrWhiteSpace(Code))
            {
                return;
            }
            if (pathCode ?? (pathCode = m_Path.StartsWith("?")) ?? false)
            {
                Code = m_Path.Substring(1);
                m_Path = "<string>";
            }
            else
            {
                try
                {
                    Code = File.ReadAllText(m_Path = CommonTools.ConvertPath(m_Path));
                }
                catch (Exception e)
                {
                    FizzleDebug.LogException(e);
                }
            }
        }

        protected override void Start()
        {
            base.Start();
            Execute();
        }
    }
}
