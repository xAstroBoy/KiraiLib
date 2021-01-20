using MelonLoader;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace KiraiMod
{
    public static partial class KiraiLibLoader
    {
        private static bool loaded = false;
        public static bool hasErrored = false;

        static KiraiLibLoader()
        {
            Load();
        }

        public static bool LoadEx()
        {
            if (!loaded)
            {
                loaded = true;

                if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == "KiraiLib")) 
                    return hasErrored;

                Task<byte[]> req = new HttpClient().GetByteArrayAsync("https://cdn.jsdelivr.net/gh/xKiraiChan/KiraiLib@master/Dist/KiraiLib.dll");

                try
                {
                    byte[] bytes = req.Result;

                    Assembly.Load(bytes);

                    new Action(() => { 
                        MelonLogger.Log($"KiraiLib: ({KiraiLib.CRC32.Hash(bytes)})");
                    })();
                }
                catch (AggregateException)
                {
                    hasErrored = true;

                    MelonLogger.Log($"Failed to load KiraiLib: {req.Exception.Message}");
                }
                catch (Exception ex)
                {
                    hasErrored = true;

                    MelonLogger.Log($"Failed to load KiraiLib: {ex.Message}");
                }

                if (!hasErrored)
                    MelonLogger.Log($"Successfully loaded KiraiLib");

                MelonLogger.Log("------------------------------");
            }

            return hasErrored;
        }

        public static void Load() => LoadEx();
    }
}
