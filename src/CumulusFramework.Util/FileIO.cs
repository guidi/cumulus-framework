using System;
using System.IO;
using System.Text;

namespace CumulusFramework.Util
{
    public static class FileIO
    {
        public static String ReadAllTextFromFile(String path, Encoding encoding)
        {
            String text = String.Empty;

            if (File.Exists(path))
            {
                text =  File.ReadAllText(path, encoding);
            }

            return text;
        }

        public static String ReadAllTextFromFileUTF8(String path)
        {
            return ReadAllTextFromFile(path, Encoding.UTF8);
        }

        public static Boolean WriteAllTextToFile(String path, String text, Encoding encoding)
        {
            var writed = false;
            try
            {
                File.WriteAllText(path, text, encoding);
                writed = true;
            }
            catch (Exception){}

            return writed;
        }

        public static Boolean WriteAllTextToFileUTF8(String path, String text)
        {
            return WriteAllTextToFile(path, text, Encoding.UTF8);
        }
    }
}
