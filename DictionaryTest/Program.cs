using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DictionaryTest
{
    public static class PathSearch
    {
        static public void PrintFileNamesContainingText(string path, string text)
        {
            string[] files = Directory.GetFiles(path, "*.*");

            foreach (string file in files)
            {
                if (File.ReadLines(file).Any(line => line.Contains(text)))
                {
                    Console.WriteLine(file.Substring(file.LastIndexOf('\\') + 1));
                    continue;
                }
            }
        }


        static public void PrintMostUsedWords(string path, int count)
        {
            string _text = System.IO.File.ReadAllText(path);
            _text = Regex.Replace(_text, @"[^\sа-яА-ЯіІїЇєЄ']", String.Empty);

            string[] _splittext = _text.Split(new Char[] { ' ', '\n' });
            Dictionary<string, int> _words = new Dictionary<string, int>();

            foreach (string word in _splittext)
            {
                string tmp = word.ToLower();

                if (!_words.ContainsKey(tmp) && tmp.Length >= 3 && tmp.Length <= 20)
                {
                    _words.Add(tmp, 1);
                }
                else if (_words.ContainsKey(tmp))
                {
                    _words[tmp] += 1;
                }
            }

            int i = 1;
            foreach (var word in _words.OrderByDescending(key => key.Value))
            {
                Console.WriteLine("{0}: {1}", i, word);
                i++;
                if (i > count)
                    break;
            }
        }


        static public void PrintMostUsedExtensions(string root)
        {
            var _extensionDict= new Dictionary<string, Tuple<int, long>>(); // Key, <Count, Size>
            string[] files = Directory.GetFiles(root, "*.*", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true });

            for (int i = 0; i < files.Length; i++)
            {
                if (!Path.HasExtension(files[i]))
                    continue;

                string tmpExt = Path.GetExtension(files[i]).Replace(".", "");
                long tmpSize = new FileInfo(files[i]).Length;
            
                if (_extensionDict.ContainsKey(tmpExt))
                {
                    _extensionDict[tmpExt] = Tuple.Create(_extensionDict[tmpExt].Item1 + 1, _extensionDict[tmpExt].Item2 + tmpSize); // Example: .png <count + 1, size + newsize>
                }
                else
                {
                    _extensionDict.Add(tmpExt, Tuple.Create(1, tmpSize));
                }
            }
            
            foreach (var ext in _extensionDict.OrderByDescending(key => key.Value.Item1))
            {
                Console.WriteLine("{0}\t\t\t{1} files\t\t\t{2} bytes", ext.Key, ext.Value.Item1, ext.Value.Item2);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            string path;
            string text;

            Console.WriteLine("Task 1: Print file names, containing text");
            path = @"C:\example";
            text = "solution";
            PathSearch.PrintFileNamesContainingText(path, text);

            Console.ReadKey();
            
            Console.WriteLine("\nTask 2: Print most used words");
            path = @"C:\example\kobzar.txt";
            PathSearch.PrintMostUsedWords(path, 50);

            Console.ReadKey();

            path = @"C:\";
            Console.WriteLine("\nTask 3: Print most used extensions");
            PathSearch.PrintMostUsedExtensions(path);

            Console.ReadKey();
        }
    }
}