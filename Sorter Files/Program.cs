using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sorter_Files
{
    internal class Program
    {
        public static List<string> GetFilesInDirectory(string directoryPath)
        {
            List<string> directoryFiles = new List<string>();
            List<string> filesPathInDirectory = new List<string>(Directory.GetFiles(directoryPath));
            try
            {
                foreach (string file in filesPathInDirectory)
                {
                    if (File.Exists(file))
                    {
                        directoryFiles.Add(Path.GetFileName(file));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"# File enumeration error\n{ex.Message}");
                Console.ResetColor();
                Console.Write("~ Press enter to retry or escape to reselect directory ~");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        Console.Clear();
                        GetFilesInDirectory(directoryPath);
                        break;
                    case ConsoleKey.Escape:
                        Main();
                        break;
                }
            }

            if (directoryFiles.Count == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("# ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Current directory is empty");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(". . .");
                Console.ResetColor();
                Console.WriteLine("\n~ Press any key to reselect directory ~");
                Console.ReadKey();
                Main();
            }
            return directoryFiles;
        }
        public static void CreateDirectories(List<string> directoryFiles, Dictionary<string, List<string>> directoryExtensions, string directoryPath)
        {
            foreach (string file in directoryFiles)
            {
                string extentionFile = Path.GetExtension(file);
                bool categoryForFileFind = false;
                foreach (var category in directoryExtensions)
                {
                    try
                    {
                        if (category.Value.Contains(extentionFile))
                        {
                            string newDirectory = Path.Combine(directoryPath, category.Key);
                            if (!Directory.Exists(newDirectory))
                            {
                                Directory.CreateDirectory(newDirectory);
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(" ~ ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("create directory");
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(" -- ");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine($"{category.Key}");
                                Console.ResetColor();
                            }
                            categoryForFileFind = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error in process creating derictory -- {category.Key}\n{ex.Message}");
                        Console.ResetColor();
                    }
                }
                if (!categoryForFileFind)
                {
                    try
                    {
                        string newDirectory = Path.Combine(directoryPath, "Other");
                        if (!Directory.Exists(newDirectory))
                        {
                            Directory.CreateDirectory(newDirectory);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" ~ ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("create directory");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" -- ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Other");
                            Console.ResetColor();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error in process creating derictory -- Other\n{ex.Message}");
                        Console.ResetColor();
                    }
                }
            }
        }
        public static void CategorizeFiles(List<string> directoryFiles, Dictionary<string, List<string>> directoryExtensions, string directoryPath)
        {
            foreach (var file in directoryFiles)
            {
                string extentionFile = Path.GetExtension(file);
                bool fileDirectoryFound = false;
                foreach (var category in directoryExtensions)
                {
                    if (category.Value.Contains(extentionFile))
                    {
                        try
                        {
                            fileDirectoryFound = true;
                            string newLocationFile = Path.Combine(directoryPath, category.Key, file);
                            string currentLocationFile = Path.Combine(directoryPath, file);
                            File.Move(currentLocationFile, newLocationFile);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" file ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write($"{file}");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" move to ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"{category.Key}");
                            Console.ResetColor();
                            break;

                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Error in process moving file -- {file} to {category.Key}\n{ex.Message}");
                            Console.ResetColor();
                        }
                    }
                }
                if (!fileDirectoryFound)
                {
                    string newLocationFile = Path.Combine(directoryPath, "Other", file);
                    string currentLocationFile = Path.Combine(directoryPath, file);
                    File.Move(currentLocationFile, newLocationFile);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" file ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{file}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" move to ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Other");
                    Console.ResetColor();
                }
            }
        }
        public static void RestartOrOut()
        {
            Console.WriteLine("~ Press enter to retry or escape to exit ~");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    Console.Clear();
                    Main();
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }


        public static string GetUserDirectoryPath()
        {
            string path = "";
            var ofd = new FolderBrowserDialog
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + "\\"
            };
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("# ");
            Console.ResetColor();
            Console.Write("Select a folder in the window that opens");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(". . .");
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error opening a dialog window\n{ex}");
                Console.ResetColor();
            }
            Console.Clear();
            if (!Directory.Exists(path))
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.DarkGray;
                Console.Write("# ");
                Console.ForegroundColor= ConsoleColor.Yellow;
                if (string.IsNullOrEmpty(path))
                {
                    Console.Write("Path is not selected");
                }
                else
                {
                    Console.Write("Path is not correct");
                }
                Console.ForegroundColor= ConsoleColor.DarkGray;
                Console.WriteLine(". . .");
                Console.ResetColor();
                Console.WriteLine("~ Press any key to reselect path ~");
                Console.ReadKey();
                return GetUserDirectoryPath();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("# ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Path received successfully");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("  Path: ");
                Console.ResetColor();
                Console.WriteLine($"{path}\n");
            }
            return path;
        }

        [STAThread]
        public static void Main()
        {
            Dictionary<string, List<string>> directoryExtensions = new Dictionary<string, List<string>>
            {
                 { "Video", new List<string>
                     { ".mp4", ".mov", ".avi", ".mkv", ".wmv", ".3gp", ".3g2",
                       ".mpg", ".mpeg", ".m4v", ".h264", ".flv", ".rm", ".swf", ".vob",
                       ".mpe", ".mpv", ".mp2", ".m2v", ".ogv", ".qt", ".asf", ".avchd" } },

                 { "Audio", new List<string>
                     { ".mp3", ".wav", ".ogg", ".flac", ".aif", ".mid", ".midi",
                       ".mpa", ".wma", ".wpl", ".cda", ".aac", ".m4a", ".m3u",
                       ".pls", ".wv", ".aiff", ".ra", ".ram", ".mka", ".opus" } },

                 { "Image", new List<string>
                     { ".jpg", ".png", ".bmp", ".ai", ".psd", ".ico", ".jpeg",
                       ".ps", ".svg", ".tif", ".tiff", ".gif", ".eps",
                       ".raw", ".cr2", ".nef", ".orf", ".sr2", ".webp", ".heic",
                       ".dng", ".arw", ".rw2", ".rwl", ".raf" } },

                 { "Document", new List<string>
                     { ".pdf", ".txt", ".doc", ".docx", ".rtf", ".tex", ".wpd",
                       ".odt", ".md", ".wps", ".dot", ".dotx", ".pptx", ".ppt",
                       ".pps", ".key", ".odp", ".ppsx", ".potx", ".pot", ".pptm", ".ppsm" } },

                 { "Code_Data", new List<string>
                     { ".py", ".js", ".html", ".json", ".java", ".cpp", ".css",
                       ".php", ".cs", ".rb", ".pl", ".sh", ".swift", ".vb", ".r", ".go",
                       ".sql", ".sqlite", ".sqlite3", ".csv", ".dat", ".db", ".log",
                       ".mdb", ".sav", ".tar", ".xml", ".xlsx", ".xls", ".xlsm", ".ods",
                       ".json", ".dbf", ".tab", ".tsv", ".parquet", ".h5", ".hdf5",
                       ".feather", ".dta", ".sav", ".zsav", ".por" } },

                 { "Archive", new List<string>
                     { ".zip", ".rar", ".7z", ".z", ".gz", ".rpm", ".arj", ".pkg", ".jar",
                       ".deb", ".tar.gz", ".tar.bz2", ".iso", ".img", ".dmg", ".xz",
                       ".tgz", ".tbz2", ".txz", ".lz", ".tlz", ".s7z", ".cab", ".lha" } },

                 { "Installer", new List<string>
                     { ".torrent", ".msi", ".exe", ".deb", ".rpm", ".dmg", ".pkg", ".bin", ".apk", ".ipa" } }
            };

            string? directoryPath = GetUserDirectoryPath();

            List<string> directoryFiles = GetFilesInDirectory(directoryPath);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();

            CreateDirectories(directoryFiles, directoryExtensions, directoryPath);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();

            CategorizeFiles(directoryFiles, directoryExtensions, directoryPath);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();

            RestartOrOut();
        }
    }
}