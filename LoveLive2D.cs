using System.Reflection;
using NativeFileDialogSharp;
using Spectre.Console;

namespace Kiraio.LoveL2D
{
    public class LoveLive2D
    {
        const string LIVE2D_CUBISM_MAIN = "Live2D_Cubism.jar";
        static readonly string LIVE2D_CUBISM_PACKAGE = Utils.NormalizePath("com/live2d/cubism");
        // static string APP_LIB_PATH = Utils.NormalizePath("app/lib");
        // const string MOD_LIB_PATH = "lib";
        static readonly string RLM = "rlm1501.jar";

        static void Main(string[] args)
        {
            AnsiConsole.Background = Color.Grey11;
            PrintHelp();

            while (true)
            {
                int actionChoice;
                string pickedFile = "";

                if (!(args.Length > 0))
                {
                    string[] actionList =
                    {
                        "Patch Live2D Cubism Editor v5",
                        "Revoke Live2D Cubism Editor v5 license",
                        "Exit"
                    };
                    actionChoice = ShowMenu(actionList);
                    if (actionChoice == actionList.Length - 1) // Exit
                        return;

                    DialogResult filePicker = SelectFile();
                    if (filePicker.IsCancelled)
                    {
                        AnsiConsole.MarkupLine("Cancelled.");
                        continue;
                    }
                    if (filePicker.IsError)
                    {
                        AnsiConsole.WriteException(new Exception($"Something went wrong! Can\'t open File Picker."));
                        continue;
                    }
                    pickedFile = filePicker.Path;
                }
                else
                {
                    Console.WriteLine(Directory.Exists(args[0]));
                    pickedFile = Directory.Exists(args[0]) ? args[0] : pickedFile;
                    //! REVERSED! 1 = patch, 0 = revoke
                    actionChoice = int.Parse(args[1]) == 0 ? 1 : 0;
                }

                string execDirectory = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location
                ) ?? string.Empty;
                string live2dDirectory = Path.GetDirectoryName(pickedFile) ?? string.Empty;
                string resourceDirectory = Path.Combine(live2dDirectory, "app/lib");
                resourceDirectory = Directory.Exists(resourceDirectory) ? resourceDirectory : Path.Combine(live2dDirectory, "res");

                // APP_LIB_PATH = Path.Combine(live2dDirectory ?? string.Empty, APP_LIB_PATH);
                Utils.SaveLastOpenedFile(pickedFile ?? string.Empty);

                if (!Directory.Exists(resourceDirectory))
                {
                    AnsiConsole.WriteException(new Exception(
                        $"No Live2D Cubism data found in {resourceDirectory}!")
                    );
                    return;
                }

                string originalRlmFile = Path.Combine(resourceDirectory, RLM);
                string rlmFileBackup = $"{originalRlmFile}.bak";
                string patchedRlmFile = Path.Combine(
                    execDirectory ?? string.Empty,
                    "lib",
                    RLM
                );
                string live2dMainFile = Path.Combine(resourceDirectory, LIVE2D_CUBISM_MAIN);

                switch (actionChoice)
                {
                    case 0:
                        Patch(
                            originalRlmFile,
                            patchedRlmFile,
                            rlmFileBackup,
                            live2dMainFile
                        );
                        break;
                    case 1:
                        Revoke(live2dMainFile, originalRlmFile, rlmFileBackup);
                        break;
                    default:
                        break;
                }

                if (args.Length > 0)
                {
                    Console.WriteLine();
                    AnsiConsole.MarkupLine("Press any key to exit.");
                    Console.ReadKey();
                    return;
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
                null,
                Path.GetDirectoryName(Utils.GetLastOpenedFile()) ?? execDirectory
            );
        }

        static void Patch(
            string rlmFile,
            string patchedRlmFile,
            string rlmFileBackup,
            string live2dMainFile
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

                PatchCubismCore(live2dMainFile, originalRlmHash, patchedRlmHash);

                AnsiConsole.MarkupLine("Done. Enjoy UwU");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"Failed to patch [red]{live2dMainFile}[/]!");
                AnsiConsole.WriteException(ex);
            }
        }

        static void Revoke(
            string live2dMainFile,
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
                    $"Revoking license..."
                );

                PatchCubismCore(
                    live2dMainFile,
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
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
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
