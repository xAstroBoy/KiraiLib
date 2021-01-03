using MelonLoader;

[assembly: MelonInfo(typeof(KiraiMod.KiraiLib.KiraiLibModSupport), "KiraiLib", null, "Kirai Chan#8315", "github.com/xKiraiChan/KiraiLib")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace KiraiMod
{
    partial class KiraiLib
    {
        /// <summary>
        /// Supporting class for if the end user places the library into their mods folder
        /// </summary>
        /// <remarks>Note: Using the mods folder isn't the correct way to load the library</remarks>
        public class KiraiLibModSupport : MelonMod
        {
            /// <summary>
            /// Used internally by MelonLoader
            /// </summary>
            public override void OnApplicationStart()
            {
                // This is probably hacky however it forces the cctor to be ran
                KiraiLib.NoOp();
            }
        }
    }
}