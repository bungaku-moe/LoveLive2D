using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NativeFileDialogSharp;
using Spectre.Console;

namespace Kiraio.LoveL2D
{
    public class LoveLive2D
    {
        const string VERSION = "1.0.0";
        const string LIVE2D_CUBISM_MAIN = "Live2D_Cubism.jar";
        const string LIVE2D_CUBISM_PACKAGE = "com/live2d/cubism";
        const string CEAppDef_CLASS = "g.class";
        static string APP_LIB_PATH = "app/lib";
        const string MOD_LIB_PATH = "lib";
        readonly static string[] RLMS = { "rlm1221.jar", "rlm1501.jar" };

        static void Main(string[] args)
        {
            AnsiConsole.Background = Color.Grey11;
            PrintHelp();

            ChooseAction:
            string[] actionList = { "Patch Pro License", "Revoke License", "Cancel" };
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
                case 0:
                case 1:
                    filePicker = Dialog.FileOpen("exe", Utils.GetLastOpenedDirectory() ?? execDirectory);

                    if (filePicker.IsCancelled)
                        goto ChooseAction;
                    else if (filePicker.IsError)
                    {
                        AnsiConsole.MarkupLine("[red]Something is wrong.[/]");
                        goto ChooseAction;
                    }

                    break;
                case 2:
                default:
                    return;
            }

            live2dDirectory = Path.GetDirectoryName(filePicker.Path);
            APP_LIB_PATH = Path.Combine(live2dDirectory ?? string.Empty, APP_LIB_PATH);

            Utils.SaveLastOpenedDirectory(live2dDirectory ?? string.Empty);

            string rlmPath = Path.Combine(APP_LIB_PATH, RLMS[1]);
            string oldRlmHash = Utils.GetSHA256(rlmPath);
            string rlmBackup = $"{rlmPath}.bak";
            string modRlm = Path.Combine(execDirectory ?? string.Empty, MOD_LIB_PATH, RLMS[1]);

            switch (choiceIndex)
            {
                case 0:
                    goto Patch;
                case 1:
                    goto Revoke;
            }

            Patch:
            // v5.0.0^
            File.Move(rlmPath, rlmBackup, true); // Backup original rlm
            File.Copy(modRlm, rlmPath, true);

            string newRlmHash = Utils.GetSHA256(rlmPath);
            string live2dMain = Path.Combine(APP_LIB_PATH, LIVE2D_CUBISM_MAIN);

            AnsiConsole.MarkupLineInterpolated($"Patching [green]{live2dMain}[/]...");

            /*
            * Extracting .jar file in non case-sensitive file system causing problem
            * because they didn't treat (e.g. "G.class" and "g.class") files as different file.
            */
            string live2dMainUnpacked = Utils.UnZip(live2dMain); // unpacked...
            // Utils.ReplaceStringInFile(Path.Combine(live2dMainUnpacked, LIVE2D_CUBISM_PACKAGE, CEAppDef_CLASS),  oldRlmHash, newRlmHash); // then patch "g.class"

            File.Copy(live2dMain, $"{live2dMain}.bak", true);
            // Utils.Zip(live2dMainUnpacked, live2dMain);
            // Directory.Delete(live2dMainUnpacked, true);

            AnsiConsole.MarkupLine("Done.");
            goto ChooseAction;

            Revoke:
            goto ChooseAction;

            // Console.ReadLine();
        }

        static void PrintHelp()
        {
            AnsiConsole.MarkupLineInterpolated($"[bold red]Love Live2D v{VERSION}[/]");
        }
    }
}
