using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace KiraiMod
{
    partial class KiraiLib
    {
        /// <summary> Extend KiraiLib dynamically </summary>
        public static class Libraries
        {
            private static string[] loaded = { "KiraiLib" };
            private static System.Collections.Generic.List<string> loading = new System.Collections.Generic.List<string>();

            /// <summary> Load and cache a dynamic library from a registry </summary>
            /// <param name="name"> Name of the library </param>
            /// <param name="registry"> Optional URL base to search for modules from </param>
            /// <returns> 0: Failure | 1: Success | 2: Already loaded </returns>
            public static async Task<short> LoadLibrary(string name, [Optional] string registry)
            {
                if (!loaded.Contains(name) && !loading.Contains(name))
                {
                    loading.Add(name);

                    if (string.IsNullOrWhiteSpace(registry))
                        registry = "https://raw.githubusercontent.com/xKiraiChan/KiraiLibs/master/Dist/";

                    Task<string> hashReq = http.GetStringAsync($"{registry}{name}.hash");

                    byte[] bytes = null;
                    string oHash = null;
                    string _path = $"Dependencies/{name}.dll";
                    if (File.Exists(_path))
                    {
                        bytes = File.ReadAllBytes(_path);
                        oHash = SHA256(bytes, true);
                    }

                    string hash = null;

                    try
                    {
                         hash = new string(hashReq.Result.ToCharArray().Where(char.IsLetterOrDigit).ToArray());

                        Logger.Debug($"Loading {name}:");
                        Logger.Debug($"Cached: {oHash}");
                        Logger.Debug($"Latest: {hash}");
                    } 
                    catch (Exception ex)
                    {
                        Logger.Warn($"Failed to get hash for module {name} ({ex.Message})");
                    }


                    if (oHash != hash || (oHash is null && hash == null)) // no update needed
                    {
                        Task<byte[]> dllReq = http.GetByteArrayAsync($"{registry}{name}.dll");

                        try
                        {
                            bytes = await dllReq;

                            File.WriteAllBytes(_path, bytes);

                            Logger.Info($"Updated {name}");
                        } 
                        catch (Exception ex)
                        {
                            if (bytes is null)
                            {
                                Logger.Warn($"Failed to update {name}, aborting ({ex.Message})");
                                loading.Remove(name);
                                return 0;
                            }
                            else Logger.Warn($"Failed to update {name}, using cached verion ({ex.Message})");
                        }
                    }

                    try
                    {
                        Assembly asm = Assembly.Load(bytes);

                        Type entry = asm.ExportedTypes.FirstOrDefault(t => t.Name == name);
                        if (entry != null)
                            RuntimeHelpers.RunClassConstructor(entry.TypeHandle);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Failed to load library {name} ({ex.Message})");
                        loading.Remove(name);
                        return 0;
                    }

                    string[] tmp = new string[loaded.Length + 1];
                    loaded.CopyTo(tmp, 0);
                    tmp[loaded.Length] = name;
                    loaded = tmp;
                    loading.Remove(name);

                    return 1;
                }
                else return 2;
            }
        }
    }
}