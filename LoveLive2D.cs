using System.Collections.Generic;
using System.Reflection;
using NativeFileDialogSharp;
using Spectre.Console;

namespace Kiraio.LoveL2D
{
    public class LoveLive2D
    {
        const string LIVE2D_CUBISM_MAIN = "Live2D_Cubism.jar";
        static readonly string LIVE2D_CUBISM_PACKAGE = Utils.NormalizePath("com/live2d/cubism");
        static string APP_LIB_PATH = Utils.NormalizePath("app/lib");
        const string MOD_LIB_PATH = "lib";
        static readonly string RLM = "rlm1501.jar";

        static void Main(string[] args)
        {
            AnsiConsole.Background = Color.Grey11;
            PrintHelp();

            while (true)
            {
                string[] actionList =
                {
                    "Patch Live2D Cubism Editor v5",
                    "Revoke Live2D Cubism Editor v5 license",
                    "Exit"
                };
                int choice = ShowMenu(actionList);
                if (choice == actionList.Length - 1) // Exit
                    return;

                DialogResult filePicker = SelectFile();
                if (filePicker.IsCancelled || filePicker.IsError)
                    continue;

                string execDirectory = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location
                );
                string live2dDirectory = Path.GetDirectoryName(filePicker.Path);
                APP_LIB_PATH = Path.Combine(live2dDirectory ?? string.Empty, APP_LIB_PATH);

                if (!Directory.Exists(APP_LIB_PATH))
                {
                    AnsiConsole.MarkupLineInterpolated(
                        $"No Live2D Cubism data found: {APP_LIB_PATH}"
                    );
                    return;
                }

                Utils.SaveLastOpenedFile(filePicker.Path ?? string.Empty);

                string originalRlmFile = Path.Combine(APP_LIB_PATH, RLM);
                string rlmFileBackup = $"{originalRlmFile}.bak";
                string patchedRlmFile = Path.Combine(
                    execDirectory ?? string.Empty,
                    MOD_LIB_PATH,
                    RLM
                );
                string live2dCoreFile = Path.Combine(APP_LIB_PATH, LIVE2D_CUBISM_MAIN);

                switch (choice)
                {
                    case 0:
                        Patch(
                            filePicker,
                            originalRlmFile,
                            patchedRlmFile,
                            rlmFileBackup,
                            live2dCoreFile
                        );
                        break;
                    case 1:
                        Revoke(filePicker, live2dCoreFile, originalRlmFile, rlmFileBackup);
                        break;
                }
            }
        }

        static int ShowMenu(string[] actionList)
        {
            string actionPrompt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(actionList)
            );
            return Array.FindIndex(actionList, item => item == actionPrompt);
        }

        static DialogResult SelectFile()
        {
            AnsiConsole.MarkupLine("Selecting [bold orange1]Live2D Cubism Editor[/] executable...");
            var execDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Dialog.FileOpen(
                "exe",
                Path.GetDirectoryName(Utils.GetLastOpenedFile()) ?? execDirectory
            );
        }

        static void Patch(
            DialogResult filePicker,
            string rlmFile,
            string patchedRlmFile,
            string rlmFileBackup,
            string live2dCoreFile
        )
        {
            try
            {
                string originalRlmHash = Utils.GetSHA256(rlmFile);
                string patchedRlmHash = Utils.GetSHA256(patchedRlmFile);

                if (!File.Exists(rlmFileBackup))
                {
                    File.WriteAllText($"{rlmFile}.hash", originalRlmHash);
                    File.Copy(rlmFile, rlmFileBackup, true); // Backup the original rlm
                }

                if (Utils.GetSHA256(patchedRlmFile) == Utils.GetSHA256(rlmFile))
                {
                    AnsiConsole.MarkupLine("[green]Patch already applied.[/]");
                    return;
                }

                AnsiConsole.MarkupLineInterpolated(
                    $"Copy patched [green]{Path.GetFileName(rlmFile)}[/] to [green]{rlmFile}[/]..."
                );
                File.Copy(patchedRlmFile, rlmFile, true); // Copy patched rlm to the app/lib

                PatchCubismCore(live2dCoreFile, originalRlmHash, patchedRlmHash);

                AnsiConsole.MarkupLine("Done. Enjoy UwU");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"Failed to patch [red]{live2dCoreFile}[/]!");
                AnsiConsole.WriteException(ex);
            }
        }

        static void Revoke(
            DialogResult filePicker,
            string live2dCoreFile,
            string rlmFile,
            string rlmFileBackup
        )
        {
            try
            {
                if (!File.Exists($"{rlmFile}.hash") && !File.Exists(rlmFileBackup))
                {
                    AnsiConsole.MarkupLine("[red]Pro license is'nt applied![/]");
                    return;
                }

                if (!File.Exists(rlmFileBackup))
                {
                    AnsiConsole.MarkupLine(
                        $"[red]{rlmFileBackup}) doesn\'t exists![/] Did you delete them? If yes, just reinstall Live2D Cubism, no need to revoke the license."
                    );
                    return;
                }

                AnsiConsole.MarkupLineInterpolated(
                    $"Revoking license [green]{filePicker.Path}[/]..."
                );

                PatchCubismCore(
                    live2dCoreFile,
                    Utils.GetSHA256(rlmFile),
                    File.ReadAllText($"{rlmFile}.hash")
                );

                File.Delete(rlmFile);
                File.Delete($"{rlmFile}.hash");
                File.Move(rlmFileBackup, rlmFile, true);

                AnsiConsole.MarkupLine("Done.");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Failed to revoke license![/]");
                AnsiConsole.WriteException(ex);
            }

            Console.WriteLine();
        }

        static void PatchCubismCore(string filePath, string originalRlmHash, string patchedRlmHash)
        {
            List<string> ignoredFiles = new List<string>
            {
                Utils.NormalizePath("META-INF/MANIFEST.MF"),
                Utils.NormalizePath("META-INF/TE-D8685.RSA"),
                Utils.NormalizePath("META-INF/TE-D8685.SF")
            };

            AnsiConsole.MarkupLineInterpolated($"Patching [green]{filePath}[/]...");
            Utils.ModifyZipContents(
                filePath,
                filePath,
                originalRlmHash,
                patchedRlmHash,
                ignoredFiles
            );
        }

        static void PrintHelp()
        {
            string versionPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "version.txt"
            );
            string version = File.Exists(versionPath) ? File.ReadAllText("version.txt") : "UNKNOWN";
            AnsiConsole.MarkupLineInterpolated($"[bold pink3]Love Live2D v{version}[/]");
            Console.WriteLine();
            AnsiConsole.MarkupLine(
                "Unlock the full power of [link=https://www.live2d.com/en/]Live2D Cubism[/] for free!"
            );
            AnsiConsole.MarkupLine(
                "For more information, visit: [link]https://github.com/kiraio-moe/LoveLive2D[/]"
            );
            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold]Supported versions[/]: 5.0+");
            Console.WriteLine();
        }
    }
}
