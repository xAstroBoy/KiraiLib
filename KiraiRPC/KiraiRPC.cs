using MelonLoader;
using System.Globalization;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using static KiraiMod.KiraiLib;
using static VRC.SDKBase.VRC_EventHandler;
using Logger = KiraiMod.KiraiLib.Logger;

namespace KiraiLib
{
    public static partial class KiraiRPC
    {
        static KiraiRPC()
        {
            SDK.Events.OnSceneLoad += OnLoad;
            SDK.Events.OnRPC += OnRPC;
        }

        private static VRC_EventHandler handler;
        private static object token;

        public static System.Action<RPCData> Callback = new System.Action<RPCData>((_) => { });

        private static void OnLoad(int index, string name)
        {
            if (index != -1) return;
            handler = null;

            if (token != null) MelonCoroutines.Stop(token);

            token = MelonCoroutines.Start(WaitForLevelToLoad());
        }

        private static System.Collections.IEnumerator WaitForLevelToLoad()
        {
            int sleep = 0;

            while ((VRCPlayer.field_Internal_Static_VRCPlayer_0 == null ||
                (handler = Object.FindObjectOfType<VRC_EventHandler>()) == null) &&
                sleep < 60)
            {
                sleep++;
                yield return new WaitForSeconds(1);
            }

            if (sleep >= 60) yield break;
            token = null;

            Logger.Debug($"Found Event listener after {sleep} seconds");

            SendRPC(0xD00);
        }

        private static void OnRPC(Player sender, VrcEvent ev)
        {
            if (sender?.field_Private_APIUser_0 is null || ev is null) return;

            if (ev.EventType == VrcEventType.SendRPC && ev.ParameterString == "UdonSyncRunProgramAsRPC")
                ProcessRPC(sender, System.Text.Encoding.UTF8.GetString(ev.ParameterBytes).Substring(6));
        }

        private static void ProcessRPC(Player player, string rpc)
        {
            // 6 bytes is the minimum rpc size
            if (rpc.Length < 6) return;
            if (rpc[0] == 'k')
            {
                if (player != Player.prop_Player_0)
                    MelonLogger.Msg($"Recieved {rpc}");

                // get the event id
                // get the length of the mod name
                if (!uint.TryParse(rpc.Substring(1, 3), NumberStyles.HexNumber, null, out uint id) || 
                    !uint.TryParse(rpc.Substring(4, 1), NumberStyles.HexNumber, null, out uint len))
                    return;

                // check if we are in bounds to read the name
                if (rpc.Length < 6 + len) return;

                string name = rpc.Substring(5, (int)len);

                // grab the amount of parameters
                if (!uint.TryParse(rpc.Substring(5 + (int)len, 1), NumberStyles.HexNumber, null, out uint pcount)) return;

                if (rpc.Length < 6 + len + pcount * 2) return;

                int[] plens = new int[pcount];
                for (int i = 0; i < pcount; i++)
                {
                    if (!uint.TryParse(rpc.Substring(6 + (int)len + i * 2, 2), NumberStyles.HexNumber, null, out uint plen))
                        return;

                    plens[i] = (int)plen;
                }

                if (rpc.Length < 6 + len + pcount * 2 + plens.Sum())
                    return;

                int inc = 0;
                string[] parameters = new string[pcount];
                for (int i = 0; i < plens.Length; i++)
                {
                    parameters[i] = rpc.Substring(6 + (int)len + (int)pcount * 2 + inc, plens[i]);
                    inc += plens[i];
                }

                RPCData data = new RPCData(player.field_Private_APIUser_0.displayName, (int)id, name, parameters);

                if (len == 0) 
                    TransformRPCData(ref data);

                Callback(data);
            }
        }

        private static void TransformRPCData(ref RPCData data)
        {
            data.target = "KiraiRPC";

            switch (data.id) {
                case 0xD00:
                    data.id = (int)RPCEventIDs.OnInit;
                    break;
            }
        } 
    }
}
