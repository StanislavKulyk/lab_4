using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
        {
            ShowHelp();
            Environment.Exit(0);
        }

        if (args.Length < 3)
        {
            Console.WriteLine("Error: there are insufficient arguments in the function!");
            ShowHelp();
            Environment.Exit(1);
        }

        string sourceDirectory = args[0];
        string targetDirectory = args[1];
        string filePattern = args[2];

        try
        {
            Console.WriteLine($"Checking if source directory exists: {sourceDirectory}");
            if (!Directory.Exists(sourceDirectory))
            {
                Console.WriteLine($"Error: Such directory does not exist. Path: {sourceDirectory}");
                Environment.Exit(2);
            }

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            CopyAll(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory), filePattern);

            Console.WriteLine("Сopy completed successfully!");
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(3);
        }
    }

    static void CopyAll(DirectoryInfo source, DirectoryInfo target, string filePattern)
    {
        foreach (FileInfo file in source.GetFiles(filePattern))
        {
            FileInfo targetFile = new FileInfo(Path.Combine(target.FullName, file.Name));
            if ((file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden &&
                (file.Attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly &&
                (file.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                file.CopyTo(targetFile.FullName, true);
                Console.WriteLine($"Copied: {file.FullName} to {targetFile.FullName}");
            }
        }

        foreach (DirectoryInfo subdirectory in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subdirectory.Name);
            CopyAll(subdirectory, nextTargetSubDir, filePattern);
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("Usage: FileCopyUtility <sourceDirectory> <targetDirectory> <filePattern>");
        Console.WriteLine("Example:\"D:\\Labs\" \"D:\\Copyed_Directory\" \"*.exe\"");
    }
}

