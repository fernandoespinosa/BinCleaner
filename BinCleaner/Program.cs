using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BinCleaner
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Console.WindowWidth = 100;
                Console.BufferWidth = 200;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Clear();

                var workingDirectory = args.Length > 0 ? args[0] : Environment.CurrentDirectory;

                var assembly = Assembly.GetExecutingAssembly();
                Console.Title = string.Format("Bin Cleaner™ v{0} (built on {1})", assembly.GetName().Version, File.GetLastWriteTime(assembly.Location).ToString("O"));
                Console.WriteLine("Working Directory: {0}", workingDirectory);
                Console.WriteLine();

                string[] directoryNames;
                using (var foo = new Foo())
                {
                    foo.Text = Console.Title;
                    var result = foo.ShowDialog(
                        new DirectoryOption { Name = "bin", Checked = true },
                        new DirectoryOption { Name = "obj", Checked = true },
                        new DirectoryOption { Name = "packages", Checked = false },
                        new DirectoryOption { Name = "App_Data", Checked = false },
                        new DirectoryOption { Name = ".nuget", Checked = false });

                    if (result != DialogResult.OK)
                        Environment.Exit(0);

                    directoryNames = foo.SelectedDirectoryNames.ToArray();
                }

                Console.WriteLine("Looking for directories named:");
                foreach (var dirName in directoryNames)
                    Console.WriteLine("   {0}", dirName);
                Console.WriteLine();

                var directories = FindDirectories(workingDirectory, directoryNames).ToList();

                if (directories.Count == 0)
                {
                    Console.WriteLine("No directories found to be deleted. The END. :)");
                    Console.ReadKey();
                    return;
                }

                Console.Write("The following directories and their contents shall be ");
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("utterly annihilated");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(":");

                foreach (var directory in directories)
                    Console.WriteLine("  {0}", directory);

                Console.WriteLine();
                Console.Write("Are you sure you want to ");
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("completely obliterate");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" the aforementioned directories?");
                Console.WriteLine();
                Console.Write("(Press Enter to confirm)");
                var key = Console.ReadKey(true);
                Console.WriteLine();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Enter)
                {
                    foreach (var directory in directories)
                    {
                        if (Directory.Exists(directory))
                        {
                            try
                            {
                                Console.Write("Deleting {0}...", directory);
                                Directory.Delete(directory, true);
                                Console.WriteLine(" sheer success!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(" no can't do: {0}", ex.Message);
                            }
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Job·Done. :D");
                }
                else
                {
                    Console.WriteLine("No comprendo. Bye. >:(");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        private static IEnumerable<string> FindDirectory(string workingDirectory, string directoryName)
        {
            return Directory.EnumerateDirectories(workingDirectory, directoryName, SearchOption.AllDirectories);
        }

        private static IEnumerable<string> FindDirectories(string workingDirectory, params string[] directoryNames)
        {
            return directoryNames.SelectMany(d => FindDirectory(workingDirectory, d));
        }
    }
}
