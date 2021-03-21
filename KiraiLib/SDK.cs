using Harmony;
using MelonLoader;
using System;
using System.Linq;
using System.Reflection;
using VRC;
using static VRC.SDKBase.VRC_EventHandler;

namespace KiraiMod
{
    public partial class KiraiLib
    {
        /// <summary> Utilities for KiraiLibs </summary>
        public static class SDK
        {
            public static HarmonyInstance harmony = HarmonyInstance.Create("KiraiLib");

            public static class Events
            {
                public static Action<int, string> OnSceneLoad = new Action<int, string>((_, __) => { });
                public static Action<Player, VrcEvent> OnRPC = new Action<Player, VrcEvent>((_, __) => { }); 
            }

            internal static void Initialize()
            {
                try
                {
                    harmony.Patch(typeof(MelonHandler)
                        .GetMethod("OnSceneWasLoaded", BindingFlags.Static | BindingFlags.NonPublic),
                        new HarmonyMethod(typeof(SDK).GetMethod(nameof(SDK.OnSceneLoadHook), BindingFlags.Static | BindingFlags.NonPublic)));
                } catch { Logger.Error("Failed to hook OnSceneLoad"); }

                try
                {
                    MethodInfo OnRPCInfo = typeof(VRC_EventDispatcherRFC)
                        .GetMethods()
                        .Where(m => m.Name.Contains("Method_Public_Void_Player_VrcEvent_VrcBroadcastType_Int32_Single_"))
                        .FirstOrDefault(m => m.Name.Length == 66);

                    harmony.Patch(OnRPCInfo, new HarmonyMethod(typeof(SDK).GetMethod(nameof(SDK.OnRPCHook), BindingFlags.Static | BindingFlags.NonPublic)));
                } catch { Logger.Error("Failed to hook OnRPC"); }
            }

            private static void OnRPCHook(ref Player __0, ref VrcEvent __1) => Events.OnRPC(__0, __1); 
            private static void OnSceneLoadHook(ref int __0, ref string __1) => Events.OnSceneLoad(__0, __1); 
        }
    }
}
