using System.Reflection;
using NativeFileDialogSharp;
using Spectre.Console;

namespace Kiraio.LoveL2D
{
    public class LoveLive2D
    {
        const string VERSION = "1.1.0";
        const string LIVE2D_CUBISM_MAIN = "Live2D_Cubism.jar";
        static readonly string LIVE2D_CUBISM_PACKAGE = Utils.NormalizePath("com/live2d/cubism");
        static string CEAppDef_CLASS = "g.class";
        static string APP_LIB_PATH = Utils.NormalizePath("app/lib");
        const string MOD_LIB_PATH = "lib";
        static readonly string RLM = "rlm1501.jar";

        static void Main(string[] args)
        {
            AnsiConsole.Background = Color.Grey11;
            PrintHelp();

            while (true)
            {
                int choice = ShowMenu();
                if (choice == 3) // Exit
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
                string originalRlmHash = Utils.GetSHA256(originalRlmFile);
                string patchedRlmFile = Path.Combine(
                    execDirectory ?? string.Empty,
                    MOD_LIB_PATH,
                    RLM
                );
                string rlmFileBackup = $"{originalRlmFile}.bak";
                string live2dCoreFile = Path.Combine(APP_LIB_PATH, LIVE2D_CUBISM_MAIN);
                string live2dCoreFileBackup = $"{live2dCoreFile}.bak";
                CEAppDef_CLASS = choice == 0 ? "g.class" : "h.class";
                CEAppDef_CLASS = Path.Combine(LIVE2D_CUBISM_PACKAGE, CEAppDef_CLASS);

                switch (choice)
                {
                    case 0:
                    case 1:
                        Patch(
                            filePicker,
                            originalRlmFile,
                            originalRlmHash,
                            patchedRlmFile,
                            rlmFileBackup,
                            live2dCoreFile,
                            live2dCoreFileBackup
                        );
                        break;
                    case 2:
                        Revoke(
                            filePicker,
                            live2dCoreFile,
                            live2dCoreFileBackup,
                            originalRlmFile,
                            rlmFileBackup
                        );
                        break;
                }
            }
        }

        static int ShowMenu()
        {
            string[] actionList =
            {
                "Patch Live2D Cubism Editor v5.0",
                "Patch Live2D Cubism Editor v5.1",
                "Revoke License",
                "Exit"
            };
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
            string originalRlmHash,
            string pacthedRlmFile,
            string rlmFileBackup,
            string live2dCoreFile,
            string live2dCoreFileBackup
        )
        {
            try
            {
                File.Copy(live2dCoreFile, live2dCoreFileBackup, true); // Backup Live2D_Cubism.jar
                File.Move(rlmFile, rlmFileBackup, true); // Backup original rlm
                File.Copy(pacthedRlmFile, rlmFile, true); // Copy patched rlm to the installation path
                string patchedRlmHash = Utils.GetSHA256(rlmFile);

                AnsiConsole.MarkupLineInterpolated($"Patching [green]{filePicker.Path}[/]...");

                string patchedGClass = Utils.ExtractZipEntry(live2dCoreFile, CEAppDef_CLASS);
                patchedGClass = Utils.ReplaceStringInBinaryFile(
                    patchedGClass,
                    $"{patchedGClass}.temp",
                    originalRlmHash,
                    patchedRlmHash
                );

                List<(string, byte[])> modifiedFiles = new List<(string, byte[])>
                {
                    (CEAppDef_CLASS, File.ReadAllBytes(patchedGClass))
                };
                List<string> ignoredFiles = new List<string>
                {
                    Utils.NormalizePath("META-INF/MANIFEST.MF"),
                    Utils.NormalizePath("META-INF/TE-D8685.RSA"),
                    Utils.NormalizePath("META-INF/TE-D8685.SF")
                };

                Utils.ModifyZipContents(
                    live2dCoreFile,
                    live2dCoreFile,
                    modifiedFiles,
                    ignoredFiles
                );

                Directory.Delete(
                    Path.Combine(
                        APP_LIB_PATH,
                        CEAppDef_CLASS.Split(Path.DirectorySeparatorChar)[0]
                    ),
                    true
                );

                AnsiConsole.MarkupLine("Done. Enjoy UwU");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"Failed to patch [red]{live2dCoreFile}[/]!");
                AnsiConsole.WriteException(ex);

                if (File.Exists(live2dCoreFile))
                {
                    File.Delete(live2dCoreFile);
                    File.Move(live2dCoreFileBackup, live2dCoreFile, true);
                }

                if (File.Exists(rlmFile))
                {
                    File.Delete(rlmFile);
                    File.Move(rlmFileBackup, rlmFile, true);
                }

                string gClassRootDirectory = Path.Combine(
                    APP_LIB_PATH,
                    CEAppDef_CLASS.Split(Path.DirectorySeparatorChar)[0]
                );
                if (Directory.Exists(gClassRootDirectory))
                    Directory.Delete(gClassRootDirectory, true);
            }
        }

        static void Revoke(
            DialogResult filePicker,
            string live2dCoreFile,
            string live2dCoreFileBackup,
            string rlmFile,
            string rlmFileBackup
        )
        {
            try
            {
                if (!File.Exists(live2dCoreFileBackup) || !File.Exists(rlmFileBackup))
                {
                    AnsiConsole.MarkupLine("[red]Pro license is'nt applied![/]");
                    return;
                }

                AnsiConsole.MarkupLineInterpolated(
                    $"Revoking license [green]{filePicker.Path}[/]..."
                );

                AnsiConsole.MarkupLine("Deleting the patched files...");
                File.Delete(live2dCoreFile);
                File.Delete(rlmFile);

                AnsiConsole.MarkupLine("Restoring original files from the backup...");
                if (!File.Exists(live2dCoreFileBackup) || !File.Exists(rlmFileBackup))
                {
                    AnsiConsole.MarkupLine(
                        $"[red]One or more backup files ({live2dCoreFileBackup}, {rlmFileBackup}) doesn\'t exists![/] Did you delete them? If yes, just reinstall Live2D Cubism, no need to revoke the license."
                    );
                    return;
                }

                File.Move(live2dCoreFileBackup, live2dCoreFile, true);
                File.Move(rlmFileBackup, rlmFile, true);

                AnsiConsole.MarkupLine("Done.");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated(
                    $"Failed to revoke license [red]{live2dCoreFile}[/]"
                );
                AnsiConsole.WriteException(ex);
            }

            Console.WriteLine();
        }

        static void PrintHelp()
        {
            AnsiConsole.MarkupLineInterpolated($"[bold pink3]Love Live2D v{VERSION}[/]");
            Console.WriteLine();
            AnsiConsole.MarkupLine(
                "Unlock the full power of [link=https://www.live2d.com/en/]Live2D Cubism[/] for free!"
            );
            AnsiConsole.MarkupLine(
                "For more information, visit: [link]https://github.com/kiraio-moe/LoveLive2D[/]"
            );
            Console.WriteLine();
            AnsiConsole.MarkupLine("[bold]Supported versions[/]: 5.0, 5.1");
            Console.WriteLine();
        }
    }
}
