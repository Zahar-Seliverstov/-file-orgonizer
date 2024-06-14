using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sorter_Files
{
    internal class Program
    {
        public static List<string> GetFilesInDirectory(string directoryPath)
        {
            List<string> directoryFiles = new List<string>();
            List<string> filesPathInDirectory = new List<string>(Directory.GetFiles(directoryPath));
            foreach (string file in filesPathInDirectory)
            {
                if (File.Exists(file))
                {
                    directoryFiles.Add(Path.GetFileName(file));
                }
            }
            if (directoryFiles.Count == 0)
            {
                Console.WriteLine("Current directory is empty\n_Press any key to continue_");
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
                foreach (var category in directoryExtensions)
                {
                    if (category.Value.Contains(extentionFile))
                    {
                        try
                        {
                            string newDirectory = Path.Combine(directoryPath, category.Key);
                            if (!Directory.Exists(newDirectory))
                            {
                                Directory.CreateDirectory(newDirectory);
                                Console.WriteLine($"~create directory -- {category.Key}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in process creating derictory -- {category.Key}\n{ex.Message}");
                        }
                    }
                }
            }
        }
        public static void CategorizeFiles(List<string> directoryFiles, Dictionary<string, List<string>> directoryExtensions, string directoryPath)
        {
            foreach (var file in directoryFiles)
            {
                string extentionFile = Path.GetExtension(file);
                foreach (var category in directoryExtensions)
                {
                    if (category.Value.Contains(extentionFile))
                    {
                        try
                        {
                            string newLocationFile = Path.Combine(directoryPath, category.Key, file);
                            string currentLocationFile = Path.Combine(directoryPath, file);

                            File.Move(currentLocationFile, newLocationFile);
                            Console.WriteLine($"file {file} move to {category.Key}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in process moving file -- {file} to {category.Key}\n{ex.Message}");
                        }
                    }
                }
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
            Console.Write("Select folder to open window. . .");
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error to opening dialog window\n{ex}");
            }
            Console.Clear();
            if (!Directory.Exists(path) || string.IsNullOrEmpty(path))
            {
                Console.Clear();
                Console.WriteLine("Path is not correct\nPress any key to continue");
                Console.ReadKey();
                GetUserDirectoryPath();
            }
            else Console.WriteLine($"Path successfully chosen\nPath: {path}");
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

            string directoryPath = GetUserDirectoryPath();

            List<string> directoryFiles = GetFilesInDirectory(directoryPath);
            CreateDirectories(directoryFiles, directoryExtensions, directoryPath);
            CategorizeFiles(directoryFiles, directoryExtensions, directoryPath);

        }
    }
}