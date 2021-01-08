using System;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        /// <summary> All the events that can be subscribed to </summary>
        /// <example> Callbacks.OnReload += new Action(() => { /* ... */ }) </example>
        public static class Callbacks
        {
            /// <summary> Called after all UI elements have been destroyed </summary>
            public static Action OnUIUnload = new Action(() => { });

            /// <summary> Called when it is time to recreate UI elements </summary>
            public static Action OnUIReload = new Action(() => { });
        }
    }
}
