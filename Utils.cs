using System.Security.Cryptography;
using System.Text;
using Spectre.Console;
using ICSharpCode.SharpZipLib.Zip;

namespace Kiraio.LoveL2D
{
    public static class Utils
    {
        const string LAST_OPEN_FILE = "last_open.txt";

        /// <summary>
        /// Get SHA-256 hash of a file.
        /// </summary>
        /// <param name="filePath">Source file.</param>
        /// <returns>SHA-256 hash.</returns>
        internal static string GetSHA256(string filePath)
        {
            try
            {
                using FileStream fileStream = File.OpenRead(filePath);
                using SHA256 hash = SHA256.Create();

                fileStream.Position = 0; // Make sure position at 0
                byte[] hashBytes = hash.ComputeHash(fileStream);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // Normalize the return
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

            return string.Empty;
        }

        internal static void ReplaceStringInFile(
            string filePath,
            string oldString,
            string newString
        )
        {
            StringBuilder stringBuilder = new();

            using (StreamReader reader = new(filePath))
            {
                string? line;
                while ((line = reader?.ReadLine()) != null)
                {
                    // Replace the string while reading
                    line = line.Replace(oldString, newString);
                    stringBuilder.AppendLine(line);
                }
            }

            // Write the modified content back to the file
            using StreamWriter writer = new(filePath);
            writer.Write(stringBuilder.ToString());
        }

        internal static string UnZip(string filePath, string? outputFolder = null)
        {
            outputFolder ??= Path.Combine(
                Path.GetDirectoryName(filePath) ?? string.Empty,
                Path.GetFileNameWithoutExtension(filePath)
            );

            try
            {
                using ZipFile zipFile = new(filePath);
                Directory.CreateDirectory(outputFolder);

                if (zipFile.Count > 0)
                {
                    foreach (ZipEntry entry in zipFile)
                    {
                        string? entryPath = Path.Combine(outputFolder, entry.Name);
                        Directory.CreateDirectory(Path.GetDirectoryName(entryPath) ?? string.Empty);

                        if (!entry.IsDirectory)
                        {
                            try
                            {
                                using Stream inputStream = zipFile.GetInputStream(entry);
                                using FileStream outputStream = File.Create(entryPath);
                                inputStream.CopyTo(outputStream);
                            }
                            catch (Exception ex)
                            {
                                AnsiConsole.WriteException(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

            return outputFolder;
        }

        internal static string Zip(string sourceFolder, string? outputFile = null)
        {
            outputFile ??= $"{Path.GetFileName(sourceFolder)}.zip";
            string[] files = GetFilesRecursive(sourceFolder);

            try
            {
                using ZipOutputStream zipStream = new(File.Create(outputFile));

                foreach (string filePath in files)
                {
                    FileInfo file = new(filePath);
                    string entryName = filePath[sourceFolder.Length..].TrimStart('\\'); // Keep file structure by including the folder
                    ZipEntry entry = new(entryName) { DateTime = DateTime.Now, Size = file.Length };
                    zipStream.PutNextEntry(entry);

                    using FileStream fileStream = file.OpenRead();
                    byte[] buffer = new byte[4096]; // Optimum size
                    int bytesRead;

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        zipStream.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

            return outputFile;
        }

        /// <summary>
        /// Get all files from a folder.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <returns>File paths.</returns>
        internal static string[] GetFilesRecursive(string sourceFolder)
        {
            return Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
        }

        internal static void SaveLastOpenedDirectory(string directoryPath)
        {
            try
            {
                // Write the last opened directory to a text file
                using StreamWriter writer = new(LAST_OPEN_FILE);
                writer.WriteLine($"last_opened={directoryPath}");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }
        }

        internal static string GetLastOpenedDirectory()
        {
            if (!File.Exists(LAST_OPEN_FILE))
                return string.Empty;

            string lastOpenedDirectory = string.Empty;

            try
            {
                // Read the last opened directory from the text file
                using StreamReader reader = new(LAST_OPEN_FILE);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("last_opened="))
                    {
                        lastOpenedDirectory = line["last_opened=".Length..];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

            return lastOpenedDirectory;
        }
    }
}
