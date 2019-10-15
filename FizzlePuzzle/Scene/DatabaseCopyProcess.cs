using System.IO;
using FizzlePuzzle.Utility;

namespace FizzlePuzzle.Scene
{
    internal class DatabaseCopyProcess
    {
        private static readonly string path = CommonTools.ConvertPath("~/Resources/db.fizzle");
        private readonly byte[] bytes;
        private readonly bool overrided;

        public DatabaseCopyProcess(byte[] bytes, bool overrided = false)
        {
            this.bytes = bytes;
            this.overrided = overrided;
        }

        public void Process()
        {
            if (File.Exists(path) && !overrided)
            {
                return;
            }
            FileStream fileStream = new FileStream(path, FileMode.Create);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }
    }
}