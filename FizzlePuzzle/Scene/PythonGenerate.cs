using FizzlePuzzle.Utility;

namespace FizzlePuzzle.Scene
{
    internal static class PythonGenerator
    {
        public static FizzlePython Python { get; } = InternalPython;

        private static FizzlePython InternalPython
        {
            get
            {
                FizzlePython fizzlePython = new FizzlePython();
                fizzlePython.AddAssembly(typeof(FizzleScene).Assembly);
                fizzlePython.AddSearchPath(CommonTools.ConvertPath("~/Plugins/"));
                fizzlePython.AddSearchPath(CommonTools.ConvertPath("~/Plugins/python-stdlib.zip"));
                fizzlePython.AddSearchPath(CommonTools.ConvertPath("~/Plugins/yaml.zip"));
                fizzlePython.AddSearchPath(CommonTools.ConvertPath("~/Plugins/xlrd.zip"));
                fizzlePython.Execute(FizzlePython.ENCODING_DECLARATION + "\nimport fizzle\nfrom fizzle import *\n");
                return fizzlePython;
            }
        }

        public static FizzleJson LoadYaml(string path)
        {
            path = CommonTools.ConvertPath(path);
            return new FizzleJson(Python.to_json(Python.load_yaml(path)));
        }
    }
}
