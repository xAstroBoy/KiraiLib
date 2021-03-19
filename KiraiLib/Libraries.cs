using MelonLoader;
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

            /// <summary> Load and cache a dynamic library from a registry </summary>
            /// <param name="name"> Name of the library </param>
            /// <param name="registry"> Optional URL base to search for modules from </param>
            /// <returns> True if success or already loaded 1</returns>
            public static async Task<bool> LoadLibrary(string name, [Optional] string registry)
            {
                if (!loaded.Contains(name))
                {
                    if (string.IsNullOrWhiteSpace(registry))
                        registry = "https://raw.githubusercontent.com/xKiraiChan/KiraiLib/master/Dist/";

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
                        hash = new string((await hashReq)
                            .ToCharArray()
                            .Where(char.IsLetterOrDigit)
                            .ToArray());
                    } 
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"Failed to get hash for module {name} ({ex.Message})");
                    }


                    if (oHash != hash || (oHash is null && hash == null)) // no update needed
                    {
                        Task<byte[]> dllReq = http.GetByteArrayAsync($"{registry}{name}.dll");

                        try
                        {
                            bytes = await dllReq;

                            File.WriteAllBytes($"Dependencies/{name}.dll", bytes);
                        } 
                        catch (Exception ex)
                        {
                            if (bytes is null)
                            {
                                MelonLogger.Warning($"Failed to update {name}, aborting ({ex.Message})");
                                return false;
                            }
                            else MelonLogger.Warning($"Failed to update {name}, using cached verion ({ex.Message})");
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
                        MelonLogger.Error($"Failed to load library {name} ({ex.Message})");
                        return false;
                    }

                    string[] tmp = new string[loaded.Length + 1];
                    loaded.CopyTo(tmp, 0);
                    tmp[loaded.Length] = name;
                    loaded = tmp;

                    return true;
                }
                else return true;
            }
        }
    }
}