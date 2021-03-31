using System;
using VRC;
using static VRC.SDKBase.VRC_EventHandler;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        [Obsolete("Use Events instead", false)]
        public static class Callbacks
        {
            public static Action OnUIUnload = new Action(() => { });
            public static Action OnUIReload = new Action(() => { });
        }

        /// <summary> All the events that can be subscribed to </summary>
        /// <example> Callbacks.OnReload += new Action(() => { /* ... */ }) </example>
        public static class Events
        {
            /// <summary> Called after all UI elements have been destroyed </summary>
            public static Action OnUIUnload = new Action(() => Callbacks.OnUIUnload());

            /// <summary> Called when it is time to recreate UI elements </summary>
            public static Action OnUIReload = new Action(() => Callbacks.OnUIReload());

            public static Action<int, string> OnSceneLoad = new Action<int, string>((_, __) => { });
            public static Action<Player, VrcEvent> OnRPC = new Action<Player, VrcEvent>((_, __) => { });
            public static Action<Player> OnPlayerJoined = new Action<Player>(_ => { });
            public static Action<Player> OnPlayerLeft = new Action<Player>(_ => { });
            public static Action<Player> OnOwnershipTransferred = new Action<Player>(_ => { });
        }
    }
}
