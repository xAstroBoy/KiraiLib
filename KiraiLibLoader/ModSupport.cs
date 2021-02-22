using MelonLoader;

[assembly: MelonInfo(typeof(KiraiMod.KiraiLibLoader.ModSupport), "KiraiLibLoader", "\x1b[1D ", "Kirai Chan#8315", "github.com/xKiraiChan/KiraiLib")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace KiraiMod
{
    partial class KiraiLibLoader
    {
        public class ModSupport : MelonMod
        {
            public override void OnApplicationStart() => Load();
        }
    }
}