using VRC;
using VRC.SDKBase;
using static KiraiMod.KiraiLib;
using static VRC.SDKBase.VRC_EventHandler;

namespace KiraiLib
{
    partial class KiraiRPC
    {
        public static void SendRPC(int id) => SendRPC("", id, new string[0] { });
        public static void SendRPC(int id, string[] payload) => SendRPC("", id, payload);
        public static void SendRPC(string name, int id) => SendRPC(name, id, new string[0] { });
        public static void SendRPC(string name, int id, string[] payload)
        {
            string sid = id.ToString("X");
            if (sid.Length > 3) return;

            string header = "";
            string body = "";

            foreach (string param in payload)
            {
                header += param.Length.ToString("X").PadLeft(2, '0');
                body += param;
            }

            SendRPC($"k{sid.PadLeft(3, '0')}{name.Length:X}{name}{payload.Length:X}{payload.Length:X}{header}{body}");
        }

        public static void SendRPC(string raw)
        {
#if DEBUG
            MelonLogger.Msg($"Sending {raw}");
#endif

            if (handler == null)
            {
                Logger.Debug("Canceling RPC because handler is null.");
                return;
            }

            handler.TriggerEvent(
            new VrcEvent
            {
                EventType = VrcEventType.SendRPC,
                Name = "SendRPC",
                ParameterObject = handler.gameObject,
                ParameterInt = Player.prop_Player_0.field_Private_VRCPlayerApi_0.playerId,
                ParameterFloat = 0f,
                ParameterString = "UdonSyncRunProgramAsRPC",
                ParameterBoolOp = VrcBooleanOp.Unused,
                ParameterBytes = Networking.EncodeParameters(new Il2CppSystem.Object[] {
                        raw
                })
            },
            VrcBroadcastType.AlwaysUnbuffered, VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject, 0f);
        }
    }
}
