using System.IO;
using System;

namespace Friends.Helpers
{
    internal static class DataHandler
    {
        internal static string Read(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(Constants.PathDoesNotExist);
            };
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }

        }
        internal static void Write(string content, string filePath)
        {
            if (filePath != null)
            {
                using (var writer = new StreamWriter(filePath, append: false))
                {
                    writer.Write(content);
                }
            }
        }
    }
}
