using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FizzlePuzzle.Utility
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    internal class FizzlePython
    {
        internal const string ENCODING_DECLARATION = "# coding=utf-8\n";
        internal dynamic to_json => GetVariable("to_json");
        internal dynamic load_yaml => GetVariable("load_yaml");
        internal dynamic load_yaml_str => GetVariable("load_yaml_str");

        internal FizzlePython()
        {
            Engine = Python.CreateEngine();
            Scope = Engine.CreateScope();
            Runtime = Engine.Runtime;
            SearchPaths = new List<string> { "." };
        }

        private ScriptScope Scope { get; }

        private ScriptRuntime Runtime { get; }

        private ScriptEngine Engine { get; }

        private List<string> SearchPaths { get; }

        internal void AddSearchPath(string path)
        {
            if (".".Equals(path))
            {
                return;
            }
            SearchPaths.Add(path);
            Engine.SetSearchPaths(SearchPaths);
        }

        internal dynamic Execute(string expression)
        {
            return string.IsNullOrWhiteSpace(expression) ? null : Engine.CreateScriptSourceFromString(expression).Execute(Scope);
        }

        internal dynamic ExecuteFile(string file)
        {
            return Engine.CreateScriptSourceFromFile(file).Execute(Scope);
        }

        internal void ImportModule(string module)
        {
            Scope.ImportModule(module);
        }

        internal void AddAssembly(Assembly assembly)
        {
            Runtime.LoadAssembly(assembly);
        }

        internal dynamic GetVariable(string name)
        {
            return Scope.GetVariable(name);
        }
        
        internal bool ContainsVariable(string name)
        {
            return Scope.ContainsVariable(name);
        }
        
        internal bool RemoveVariable(string name)
        {
            return Scope.RemoveVariable(name);
        }
        
        internal void SetVariable(string name, object value)
        {
            Scope.SetVariable(name, value);
        }
    }
}
