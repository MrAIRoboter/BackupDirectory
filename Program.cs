using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupDirectory
{
    internal class Program
    {
        static void Main(string[] args) //BackupDirectory.exe destName destDir fromDir
        {
            if (args.Length == 3)
            {
                string backupName = args[0];
                string fromDir = args[1];
                string destDir = args[2];

                if (Directory.Exists(fromDir) == false)
                    Console.WriteLine($"Path '{fromDir}' - doesn't exist!");
                else if (Directory.Exists(destDir) == false)
                    Console.WriteLine($"Path '{destDir}' - doesn't exist!'");
                else
                    Backup(backupName, fromDir, destDir);
            }
            else
            {
                ShowConsoleHelp();
            }
        }

        static void ShowConsoleHelp()
        {
            Console.WriteLine("Using:\n");
            Console.WriteLine("\"{backupName}\" \"{destination}\" \"{password}\"");
        }

        static void Backup(string backupName, string fromDir, string destDir)
        {
            CultureInfo provider = new CultureInfo("en-US");

            destDir += $"/{backupName}";
            string year = DateTime.Now.ToString("yyyy");
            destDir += $"/{year}";
            string month = DateTime.Now.ToString("MMMM", (IFormatProvider)provider);
            destDir += $"/{month}";
            string day = DateTime.Now.ToString("dd");
            destDir += $"/{day}";

            if (Directory.Exists(destDir) == false)
                Directory.CreateDirectory(destDir);

            string hourMinute = DateTime.Now.ToString("HH-mm");
            destDir += $"/{hourMinute}";

            if (Directory.Exists(destDir) == true)
                Directory.Delete(destDir, true);
            Directory.CreateDirectory(destDir);

            CopyDirectoryTo(fromDir, destDir);
        }

        static void CopyDirectoryTo(string fromDir, string destDir)
        {
            fromDir = Path.GetFullPath(fromDir);
            destDir = Path.GetFullPath(destDir);

            string copyArguments = $"xcopy {fromDir} {destDir} /H /Y /R /C /S";

            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C " + copyArguments;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
            process.WaitForExit();
        }
    }
}
