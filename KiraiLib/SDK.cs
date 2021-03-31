using Harmony;
using MelonLoader;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
                public static Action<Player> OnPlayerJoined = new Action<Player>(_ => { });
                public static Action<Player> OnPlayerLeft = new Action<Player>(_ => { });
            }

            private static void OnRPCHook(ref Player __0, ref VrcEvent __1) => Events.OnRPC(__0, __1);

            internal static void Initialize()
            {
                MelonCoroutines.Start(InitializeNetworkManagerHooks());

                try
                {
                    MethodInfo OnRPCInfo = typeof(VRC_EventDispatcherRFC)
                        .GetMethods()
                        .Where(m => m.Name.Contains("Method_Public_Void_Player_VrcEvent_VrcBroadcastType_Int32_Single_"))
                        .FirstOrDefault(m => m.Name.Length == 66);

                    harmony.Patch(OnRPCInfo, new HarmonyMethod(typeof(SDK).GetMethod(nameof(SDK.OnRPCHook), BindingFlags.Static | BindingFlags.NonPublic)));
                    Logger.Trace("Hooked OnRPC");
                }
                catch { Logger.Error("Failed to hook OnRPC"); }
            }

            private static IEnumerator InitializeNetworkManagerHooks()
            {
                while (NetworkManager.field_Internal_Static_NetworkManager_0 is null) yield return null;

                try
                {
                    NetworkManager
                        .field_Internal_Static_NetworkManager_0
                        .field_Internal_VRCEventDelegate_1_Player_0
                        .field_Private_HashSet_1_UnityAction_1_T_0
                        .Add(Events.OnPlayerJoined);
                    Logger.Trace("Hooked OnPlayerJoined");
                }
                catch { Logger.Error("Failed to hook OnPlayerJoined"); }

                try
                {
                    NetworkManager
                        .field_Internal_Static_NetworkManager_0
                        .field_Internal_VRCEventDelegate_1_Player_1
                        .field_Private_HashSet_1_UnityAction_1_T_0
                        .Add(Events.OnPlayerLeft);
                    Logger.Trace("Hooked OnPlayerLeft");
                }
                catch { Logger.Error("Failed to hook OnPlayerLeft"); }
            }
        }
    }
}
