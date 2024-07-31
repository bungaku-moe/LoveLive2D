using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Spectre.Console;

namespace Kiraio.LoveL2D
{
    public static class Utils
    {
        const string LAST_OPEN_FILE = "last_open.txt";

        internal static string NormalizePath(string path)
        {
            string[] pathSegments = path.Split(
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            );
            List<string> normalizedSegments = new();

            foreach (string segment in pathSegments)
            {
                if (segment == "..")
                {
                    if (normalizedSegments.Count > 0)
                    {
                        normalizedSegments.RemoveAt(normalizedSegments.Count - 1);
                    }
                }
                else if (segment != "." && !string.IsNullOrEmpty(segment))
                {
                    normalizedSegments.Add(segment);
                }
            }

            return string.Join(Path.DirectorySeparatorChar.ToString(), normalizedSegments);
        }

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

        internal static string ReplaceStringInFile(
            string filePath,
            string oldString,
            string newString,
            string? outputFile = null
        )
        {
            outputFile ??= filePath;
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
            using StreamWriter writer = new(outputFile);
            writer.Write(stringBuilder.ToString());

            return outputFile;
        }

        /// <summary>
        /// Copy data from a file to another, replacing search term, ignoring case.
        /// </summary>
        /// <param name="originalFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="searchTerm"></param>
        /// <param name="replaceTerm"></param>
        internal static bool ReplaceStringInBinaryFile(
            string originalFile,
            string outputFile,
            string searchTerm,
            string replaceTerm
        )
        {
            byte b;
            //UpperCase bytes to search
            byte[] searchBytes = Encoding.UTF8.GetBytes(searchTerm.ToUpper());
            //LowerCase bytes to search
            byte[] searchBytesLower = Encoding.UTF8.GetBytes(searchTerm.ToLower());
            //Temporary bytes during found loop
            byte[] bytesToAdd = new byte[searchBytes.Length];
            //Search length
            int searchBytesLength = searchBytes.Length;
            //First Upper char
            byte searchByte0 = searchBytes[0];
            //First Lower char
            byte searchByte0Lower = searchBytesLower[0];
            //Replace with bytes
            byte[] replaceBytes = Encoding.UTF8.GetBytes(replaceTerm);
            int counter = 0;

            bool isSuccess = false;

            using (FileStream inputStream = File.OpenRead(originalFile))
            {
                //input length
                long srcLength = inputStream.Length;
                using BinaryReader inputReader = new(inputStream);
                using FileStream outputStream = File.OpenWrite(outputFile);
                using BinaryWriter outputWriter = new(outputStream);
                for (int nSrc = 0; nSrc < srcLength; ++nSrc)
                    //first byte
                    if ((b = inputReader.ReadByte()) == searchByte0 || b == searchByte0Lower)
                    {
                        bytesToAdd[0] = b;
                        int nSearch = 1;

                        //next bytes
                        for (; nSearch < searchBytesLength; ++nSearch)
                        {
                            //get byte, save it and test
                            if (
                                (b = bytesToAdd[nSearch] = inputReader.ReadByte())
                                    != searchBytes[nSearch]
                                && b != searchBytesLower[nSearch]
                            )
                            {
                                break; //fail
                            }
                        }

                        //Avoid overflow. No need, in my case, because no chance to see searchTerm at the end.
                        //else if (nSrc + nSearch >= srcLength)
                        //    break;

                        if (nSearch == searchBytesLength)
                        {
                            //success
                            ++counter;
                            outputWriter.Write(replaceBytes);
                            nSrc += nSearch - 1;

                            isSuccess = true;
                            AnsiConsole.MarkupLineInterpolated(
                                $"Successfully replaced [green]string[/] in [green]{originalFile}[/]: [green]{counter}[/]"
                            );
                        }
                        else
                        {
                            //failed, add saved bytes
                            outputWriter.Write(bytesToAdd, 0, nSearch + 1);
                            nSrc += nSearch;
                        }
                    }
                    else
                        outputWriter.Write(b);
            }

            return isSuccess;
        }

        internal static string ExtractZipEntry(
            string zipFilePath,
            string entryName,
            string? outputFilePath = null
        )
        {
            outputFilePath ??= Path.Combine(
                Path.GetDirectoryName(zipFilePath) ?? string.Empty,
                entryName
            );

            try
            {
                using ZipFile zipFile = new(zipFilePath);
                // Log all entry names in the ZIP file for debugging purposes
                // Console.WriteLine("Entries in ZIP file:");
                // foreach (ZipEntry e in zipFile)
                // {
                //     Console.WriteLine(e.Name);
                // }

                entryName = ZipEntry.CleanName(entryName); // Normalize entry name path
                ZipEntry entry = zipFile.GetEntry(entryName);

                if (entry != null)
                {
                    // Ensure the output directory exists
                    string outputDirectory = Path.GetDirectoryName(outputFilePath) ?? string.Empty;
                    if (!string.IsNullOrEmpty(outputDirectory))
                        Directory.CreateDirectory(outputDirectory);

                    using Stream zipStream = zipFile.GetInputStream(entry);
                    using FileStream outputFileStream = File.Create(outputFilePath);
                    zipStream.CopyTo(outputFileStream);

                    // AnsiConsole.MarkupLineInterpolated(
                    //     $"Zip Entry [green]{entryName}[/] extracted as [green]{outputFilePath}[/]."
                    // );
                }
                // else
                //     AnsiConsole.MarkupLineInterpolated(
                //         $"Zip Entry [red]{entryName}[/] not found in the ZIP file."
                //     );
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

            return outputFilePath;
        }

        internal static List<string> ExtractZipEntries(
            string zipFilePath,
            string folderPath,
            string outputDirectory
        )
        {
            List<string> entries = new();

            // Ensure the folderPath ends with a separator
            if (!folderPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                folderPath += "/";

            try
            {
                using ZipFile zipFile = new(zipFilePath);

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);

                foreach (ZipEntry entry in zipFile)
                {
                    // Check if the entry is a file and is within the specified folderPath
                    if (
                        !entry.IsDirectory
                        && entry.Name.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        // Remove the folderPath from the entry name
                        string relativePath = entry.Name.Substring(folderPath.Length);

                        // Ensure the file is directly within the specified folderPath (no further slashes)
                        if (!relativePath.Contains("/"))
                        {
                            // Construct the output file path
                            string outputFilePath = Path.Combine(outputDirectory, relativePath);

                            // Ensure the output directory for the file exists
                            string fileDirectory =
                                Path.GetDirectoryName(outputFilePath) ?? string.Empty;
                            if (!Directory.Exists(fileDirectory))
                            {
                                Directory.CreateDirectory(fileDirectory);
                            }

                            using Stream zipStream = zipFile.GetInputStream(entry);
                            using FileStream outputFileStream = File.Create(outputFilePath);
                            zipStream.CopyTo(outputFileStream);

                            entries.Add(outputFilePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

            return entries;
        }

        // internal static string ModifyZipContents(
        //     string inputFile,
        //     string outputFile,
        //     List<(string entryName, byte[] entryData)> modifiedFiles,
        //     List<string>? ignoredFiles = null
        // )
        // {
        //     try
        //     {
        //         // AnsiConsole.MarkupLineInterpolated(
        //         //     $"Modifying Zip archive: [green]{inputFile}[/]..."
        //         // );

        //         // Read the input ZIP file into memory
        //         byte[] zipBytes = File.ReadAllBytes(inputFile);

        //         using (MemoryStream zipStream = new(zipBytes))
        //         {
        //             // Unpack the ZIP file into memory
        //             using ZipFile inputZipFile = new(zipStream);
        //             // Create a new in-memory stream to store the modified ZIP file
        //             using MemoryStream modifiedZipStream = new();
        //             // Create a ZipOutputStream to write the modified contents
        //             using (ZipOutputStream zipOutputStream = new(modifiedZipStream))
        //             {
        //                 //     AnsiConsole.MarkupLineInterpolated(
        //                 //         $"Rewriting Zip archive [green]{outputFile}[/]..."
        //                 //     );

        //                 foreach (ZipEntry entry in inputZipFile)
        //                 {
        //                     // Check if the entry should be modified and is not in the ignored list
        //                     (string? entryName, byte[] entryData) = modifiedFiles.FirstOrDefault(
        //                         f => ZipEntry.CleanName(f.entryName) == entry.Name
        //                     );
        //                     bool ignored =
        //                         ignoredFiles?.Any(file => ZipEntry.CleanName(file) == entry.Name)
        //                         ?? false;

        //                     if (!ignored)
        //                     {
        //                         if (entryName != null)
        //                         {
        //                             // AnsiConsole.MarkupLineInterpolated(
        //                             //     $"Modifying Zip Entry: [green]{entryName}[/]..."
        //                             // );
        //                             // Modify the entry content
        //                             byte[] modifiedContentBytes = entryData;
        //                             ZipEntry modifiedEntry = new(entry.Name);
        //                             zipOutputStream.PutNextEntry(modifiedEntry);
        //                             zipOutputStream.Write(
        //                                 modifiedContentBytes,
        //                                 0,
        //                                 modifiedContentBytes.Length
        //                             );
        //                             zipOutputStream.CloseEntry();
        //                         }
        //                         else
        //                         {
        //                             // Copy unchanged entries to the modified ZIP
        //                             using Stream entryStream = inputZipFile.GetInputStream(entry);
        //                             // Create a new entry and copy the content
        //                             ZipEntry newEntry = new(entry.Name);
        //                             zipOutputStream.PutNextEntry(newEntry);

        //                             byte[] buffer = new byte[4096];
        //                             int bytesRead;
        //                             while (
        //                                 (bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0
        //                             )
        //                             {
        //                                 zipOutputStream.Write(buffer, 0, bytesRead);
        //                             }

        //                             zipOutputStream.CloseEntry();
        //                         }
        //                     }
        //                     // else
        //                     //     AnsiConsole.MarkupLineInterpolated(
        //                     //         $"Skipping Zip Entry: [green]{entry.Name}[/]"
        //                     //     );
        //                 }
        //             }

        //             // Save the modified ZIP file to disk
        //             File.WriteAllBytes(outputFile, modifiedZipStream.ToArray());
        //         }

        //         // AnsiConsole.MarkupLineInterpolated($"Zip archive modified [green]successfully[/].");
        //     }
        //     catch (Exception ex)
        //     {
        //         AnsiConsole.WriteException(ex);
        //     }

        //     return outputFile;
        // }

        internal static void ModifyZipContents(
            string inputFile,
            string outputFile,
            string originalRlmHash,
            string patchedRlmHash,
            List<string>? ignoredFiles = null
        )
        {
            try
            {
                // Read the input ZIP file into memory
                byte[] zipBytes = File.ReadAllBytes(inputFile);

                using (MemoryStream zipStream = new(zipBytes))
                {
                    // Unpack the ZIP file into memory
                    using ZipFile inputZipFile = new(zipStream);
                    // Create a new in-memory stream to store the modified ZIP file
                    using MemoryStream modifiedZipStream = new();
                    // Create a ZipOutputStream to write the modified contents
                    using (ZipOutputStream zipOutputStream = new(modifiedZipStream))
                    {
                        foreach (ZipEntry entry in inputZipFile)
                        {
                            bool ignored =
                                ignoredFiles?.Any(file => ZipEntry.CleanName(file) == entry.Name)
                                ?? false;

                            if (!ignored)
                            {
                                using Stream entryStream = inputZipFile.GetInputStream(entry);
                                using MemoryStream entryMemoryStream = new();
                                entryStream.CopyTo(entryMemoryStream);
                                byte[] entryData = entryMemoryStream.ToArray();

                                bool replaced = ReplaceStringInMemory(
                                    entryData,
                                    originalRlmHash,
                                    patchedRlmHash,
                                    out byte[] modifiedEntryData
                                );

                                if (replaced)
                                {
                                    ZipEntry modifiedEntry = new(entry.Name);
                                    zipOutputStream.PutNextEntry(modifiedEntry);
                                    zipOutputStream.Write(
                                        modifiedEntryData,
                                        0,
                                        modifiedEntryData.Length
                                    );
                                    zipOutputStream.CloseEntry();
                                }
                                else
                                {
                                    ZipEntry newEntry = new(entry.Name);
                                    zipOutputStream.PutNextEntry(newEntry);
                                    zipOutputStream.Write(entryData, 0, entryData.Length);
                                    zipOutputStream.CloseEntry();
                                }
                            }
                        }
                    }

                    // Save the modified ZIP file to disk
                    File.WriteAllBytes(outputFile, modifiedZipStream.ToArray());
                }

                // AnsiConsole.MarkupLineInterpolated($"Zip archive modified [green]successfully[/].");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }
        }

        internal static bool ReplaceStringInMemory(
            byte[] data,
            string searchTerm,
            string replaceTerm,
            out byte[] modifiedData
        )
        {
            bool replaced = false;

            byte[] searchBytesUpper = Encoding.UTF8.GetBytes(searchTerm.ToUpper());
            byte[] searchBytesLower = Encoding.UTF8.GetBytes(searchTerm.ToLower());
            byte[] replaceBytes = Encoding.UTF8.GetBytes(replaceTerm);
            int searchLength = searchBytesUpper.Length;

            using MemoryStream outputStream = new();
            int i = 0;

            while (i < data.Length)
            {
                if (IsMatch(data, i, searchBytesUpper, searchBytesLower))
                {
                    replaced = true;
                    outputStream.Write(replaceBytes, 0, replaceBytes.Length);
                    i += searchLength;
                }
                else
                {
                    outputStream.WriteByte(data[i]);
                    i++;
                }
            }

            modifiedData = outputStream.ToArray();
            return replaced;
        }

        private static bool IsMatch(
            byte[] buffer,
            int offset,
            byte[] searchBytesUpper,
            byte[] searchBytesLower
        )
        {
            for (int i = 0; i < searchBytesUpper.Length; i++)
            {
                if (
                    offset + i >= buffer.Length
                    || (
                        buffer[offset + i] != searchBytesUpper[i]
                        && buffer[offset + i] != searchBytesLower[i]
                    )
                )
                {
                    return false;
                }
            }
            return true;
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

        internal static void SaveLastOpenedFile(string directoryPath)
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

        internal static string GetLastOpenedFile()
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
