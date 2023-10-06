using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class LongLiveL2D
{
    static void Main(string[] args)
    {
        string filePath = args[0];

        try
        {
            // using (FileStream fileStream = File.OpenRead(filePath))
            // {
            //     using (SHA256 hash = SHA256.Create())
            //     {
            //         fileStream.Position = 0;
            //         byte[] hashBytes = hash.ComputeHash(fileStream);
            //         Console.WriteLine($"{BitConverter.ToString(hashBytes).Replace("-", "").ToLower()}");
            //     }
            // }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        Console.ReadLine();
    }

    static void ReplaceStringInFile(string filePath, string searchString, string replacementString)
    {
        StringBuilder stringBuilder = new();

        using (StreamReader reader = new(filePath))
        {
            string? line;
            while ((line = reader?.ReadLine()) != null)
            {
                // Replace the string while reading
                line = line.Replace(searchString, replacementString);
                stringBuilder.AppendLine(line);
            }
        }

        // Write the modified content back to the file
        using StreamWriter writer = new(filePath);
        writer.Write(stringBuilder.ToString());
    }
}
