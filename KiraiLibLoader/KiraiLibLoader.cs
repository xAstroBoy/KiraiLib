using MelonLoader;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KiraiMod
{
    public static partial class KiraiLibLoader
    {
        private static bool loaded = false;
        public static bool hasErrored = false;

        static KiraiLibLoader() => Load();
        public static void Load() => LoadEx();
        public static bool LoadEx()
        {
            if (loaded)
                return false;
            else loaded = true;

            if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "KiraiLib"))
                return false;

            MelonLogger.Msg("------------------------------");

            if (!System.IO.Directory.Exists("Dependencies"))
                System.IO.Directory.CreateDirectory("Dependencies");

            HttpClient http = new HttpClient();

            Task<string> hashRequest = http.GetStringAsync("https://github.com/xKiraiChan/KiraiLib/raw/master/Dist/KiraiLib.hash");

            byte[] bytes = null;
            string oHash = null;
            if (System.IO.File.Exists("Dependencies/KiraiLib.dll"))
            {
                bytes = System.IO.File.ReadAllBytes("Dependencies/KiraiLib.dll");
                oHash = SHA256(bytes);
            }

            hashRequest.Wait();
            if (hashRequest.Exception is null)
            {
                string hash = new Regex("[^a-zA-Z0-9]").Replace(hashRequest.Result, "");

                MelonLogger.Msg(oHash);
                MelonLogger.Msg(hash);

                if (hash != oHash)
                {
                    oHash = hash;

                    Task<byte[]> libRequest = http.GetByteArrayAsync("https://github.com/xKiraiChan/KiraiLib/raw/master/Dist/KiraiLib.dll");

                    libRequest.Wait();
                    if (libRequest.Exception is null)
                    {
                        bytes = libRequest.Result;
                        System.IO.File.WriteAllBytes("Dependencies/KiraiLib.dll", bytes);
                        MelonLogger.Msg("KiraiLib updated");
                    }
                    else hasErrored = true;
                }
                else MelonLogger.Msg("KiraiLib up to date");
            }
            else
            {
                MelonLogger.Msg("Failed to check hash");
                hasErrored = true;
            }

            if (bytes != null)
            {
                try
                {
                    Assembly.Load(bytes);

                    new Action(() =>
                    {
                        MelonLogger.Msg($"KiraiLib: ({oHash})");
                    })();
                }
                catch (Exception ex)
                {
                    hasErrored = true;

                    MelonLogger.Msg($"Failed to load KiraiLib: {ex.Message}");
                }
            }

            MelonLogger.Msg("------------------------------");

            return hasErrored;
        }

        public static string SHA256(byte[] bytes)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(bytes);
            foreach (byte theByte in crypto)
                hash.Append(theByte.ToString("X2"));
            return hash.ToString();
        }
    }
}
