using System.Reflection;
using NativeFileDialogSharp;
using Spectre.Console;

namespace Kiraio.LoveL2D
{
    public class LoveLive2D
    {
        const string VERSION = "1.0.0";
        const string LIVE2D_CUBISM_MAIN = "Live2D_Cubism.jar";
        static readonly string LIVE2D_CUBISM_PACKAGE = Utils.NormalizePath("com/live2d/cubism");
        static string CEAppDef_CLASS = "g.class"; // Will be combined with LIVE2D_CUBISM_PACKAGE later
        static string APP_LIB_PATH = Utils.NormalizePath("app/lib");
        const string MOD_LIB_PATH = "lib";
        readonly static string RLM = "rlm1501.jar";

        static void Main(string[] args)
        {
            AnsiConsole.Background = Color.Grey11;
            PrintHelp();

            ChooseAction:
            string[] actionList = { "Patch Pro License", "Revoke License", "Exit" };
            string actionPrompt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(actionList)
            );
            int choiceIndex = Array.FindIndex(actionList, item => item == actionPrompt);

            string? execDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string? live2dDirectory = string.Empty;
            DialogResult filePicker;

            switch (choiceIndex)
            {
                case 0: // Patch
                case 1: // Revoke
                    AnsiConsole.MarkupLine("Selecting [bold orange1]Live2D Cubism[/] executable...");
                    filePicker = Dialog.FileOpen(
                        "exe",
                        Path.GetDirectoryName(Utils.GetLastOpenedFile()) ?? execDirectory
                    );

                    if (filePicker.IsCancelled)
                        goto ChooseAction;
                    else if (filePicker.IsError)
                    {
                        AnsiConsole.MarkupLine("[red]Something is wrong.[/]");
                        goto ChooseAction;
                    }

                    break;
                case 2: // Exit
                default:
                    return;
            }

            live2dDirectory = Path.GetDirectoryName(filePicker.Path);
            APP_LIB_PATH = Path.Combine(live2dDirectory ?? string.Empty, APP_LIB_PATH);

            if (!Directory.Exists(APP_LIB_PATH))
            {
                AnsiConsole.MarkupLineInterpolated($"No Live2D Cubism data found: {APP_LIB_PATH}");
                return;
            }

            // Save last opened file
            Utils.SaveLastOpenedFile(filePicker.Path ?? string.Empty);

            string rlmPath = Path.Combine(APP_LIB_PATH, RLM);
            string oldRlmHash = Utils.GetSHA256(rlmPath);
            string modRlm = Path.Combine(execDirectory ?? string.Empty, MOD_LIB_PATH, RLM);
            string rlmBackup = $"{rlmPath}.bak";
            string live2dMain = Path.Combine(APP_LIB_PATH, LIVE2D_CUBISM_MAIN);
            string live2dMainBackup = $"{live2dMain}.bak";
            CEAppDef_CLASS = Path.Combine(LIVE2D_CUBISM_PACKAGE, CEAppDef_CLASS);

            switch (choiceIndex)
            {
                case 0:
                    goto Patch;
                case 1:
                    goto Revoke;
            }

            Patch:
            try
            {
                // v5.0.0^
                File.Copy(live2dMain, live2dMainBackup, true); // Backup Live2D_Cubism.jar
                File.Move(rlmPath, rlmBackup, true); // Backup original rlm
                File.Copy(modRlm, rlmPath, true); // Copy MOD rlm to installation path
                string newRlmHash = Utils.GetSHA256(rlmPath);

                AnsiConsole.MarkupLineInterpolated($"Patching [green]{filePicker.Path}[/]...");

                /*
                * Extracting .jar file in a non case-sensitive file system causing problem
                * like they didn't treat (e.g. "G.class" and "g.class") files as different file,
                * so they overwrite each other.
                * To tackle this, we need to handle this in memory.
                */
                // Extract "g.class" then modify the SHA-256 hash.
                string modGClass = Utils.ExtractZipEntry(live2dMain, CEAppDef_CLASS);
                modGClass = Utils.ReplaceStringInBinaryFile(
                    modGClass,
                    $"{modGClass}.mod",
                    oldRlmHash,
                    newRlmHash
                );

                // Store modified contents into a list
                List<(string, byte[])> modifiedFiles =
                    new() { (CEAppDef_CLASS, File.ReadAllBytes(modGClass)) };
                // Ignore these files to bypass jar signing
                List<string> ignoredFiles =
                    new()
                    {
                        Utils.NormalizePath("META-INF/MANIFEST.MF"),
                        Utils.NormalizePath("META-INF/TE-D8685.RSA"),
                        Utils.NormalizePath("META-INF/TE-D8685.SF")
                    };

                Utils.ModifyZipContents(live2dMain, live2dMain, modifiedFiles, ignoredFiles);

                // Cleanup temporary files
                Directory.Delete(
                    Path.Combine(
                        APP_LIB_PATH,
                        CEAppDef_CLASS.Split(Path.DirectorySeparatorChar)[0]
                    ),
                    true
                );

                AnsiConsole.MarkupLine("Done.");
                Console.WriteLine();
                goto ChooseAction;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"Failed to patch [red]{live2dMain}[/]!");
                AnsiConsole.WriteException(ex);

                // Cleanup operation files and return the original files if there's failure.
                if (File.Exists(live2dMain))
                {
                    File.Delete(live2dMain);
                    File.Move(live2dMainBackup, live2dMain, true);
                }

                if (File.Exists(rlmPath))
                {
                    File.Delete(rlmPath);
                    File.Move(rlmBackup, rlmPath, true);
                }

                string gClassRootDirectory = Path.Combine(
                    APP_LIB_PATH,
                    CEAppDef_CLASS.Split(Path.DirectorySeparatorChar)[0]
                );
                if (Directory.Exists(gClassRootDirectory))
                    Directory.Delete(gClassRootDirectory, true);
            }

            Revoke:
            try
            {
                // Should we use SHA-256 to detect if it's the original/modded file?
                if (!File.Exists(live2dMainBackup) || !File.Exists(rlmBackup))
                {
                    AnsiConsole.MarkupLine("[red]No Pro license applied![/]");
                    goto ChooseAction;
                }

                AnsiConsole.MarkupLineInterpolated($"Revoking license [green]{filePicker.Path}[/]...");

                AnsiConsole.MarkupLine("Deleting the patched files...");
                File.Delete(live2dMain);
                File.Delete(rlmPath);

                AnsiConsole.MarkupLine("Getting the backup files...");
                File.Move(live2dMainBackup, $"{live2dMainBackup.Replace(".bak", "")}", true);
                File.Move(rlmBackup, $"{rlmBackup.Replace(".bak", "")}", true);

                AnsiConsole.MarkupLine("Done.");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"Failed to revoke license [red]{live2dMain}[/]");
                AnsiConsole.WriteException(ex);
            }

            Console.WriteLine();
            goto ChooseAction;
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
            AnsiConsole.MarkupLine("[bold]Supported version[/]: 5.0.00+");
            Console.WriteLine();
        }
    }
}
