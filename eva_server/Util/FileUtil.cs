using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace eva_server
{
    public static class FileUtil
    {
        public static bool IsExists(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static Task<string[]> AsyncGetFiles(string path)
        {
            return Task.Run(() => GetFiles(path));
        }

        public static string[] GetFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                return Directory.GetFiles(path, searchPattern, searchOption);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static Task<bool> AsyncWriteFile(string path, byte[] bytes)
        {
            return Task.Run(() => WriteFile(path, bytes));
        }

        public static bool WriteFile(string path, byte[] bytes)
        {
            try
            {
                CreateDirectory(path);
                File.WriteAllBytes(path, bytes);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool WriteText(string path, string text)
        {
            try
            {
                CreateDirectory(path);
                File.WriteAllText(path, text);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static byte[] ReadFile(string path)
        {
            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static string ReadText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }
        }

        public static void ClearDirectory(string path, string searchPattern = "*")
        {
            try
            {
                var files = Directory.GetFiles(path, searchPattern);
                RemoveFiles(files);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void RemoveFiles(IEnumerable<string> filePathList)
        {
            foreach (var path in filePathList)
            {
                try
                {
                    RemoveFile(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void RemoveFile(string path)
        {
            File.Delete(path);
        }

        private static void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            var directoryName = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directoryName)) return;

            Directory.CreateDirectory(directoryName);
        }
    }
}
