# KiraiLib
Utility library for VRChat

# Loading
**Use the loader instead of the standalone for other mods that may need a more recent version**

There are 6 ways to load the library correctly:
  - (Loader or Standalone) Using Assembly.Load with the bytes
  - (Loader or Standalone) Placing the DLL into the same folder as VRChat.exe
  - (Loader) Adding the mod to MelonHandler.Mods
  - (Loader) Placing the mod into the mods folder
  
Here is an example of the first loading method using a manifest resource stream to get the bytes
```cs
public class KiraiMod : MelonMod
{
    static KiraiMod() // .cctor
    {
        // See https://stackoverflow.com/a/15277711/9281083
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KiraiMod.Lib.KiraiLibLoader.dll");
        MemoryStream mem = new MemoryStream((int)stream.Length);
        stream.CopyTo(mem);

        Assembly.Load(mem.ToArray());

        new Action(() => KiraiLibLoader.Load())(); // This is required although the loader is ready.
    }
}
```

# Logger Usage

All logger functions have documentation, place the XML file in the same folder that you reference the library to see them in Visual Studio.

# Event Usage

KiraiLib supports unloading and reloading, due to this you will need to subscribe to some events.

OnUIUnload is optional but you probably want to subscribe to OnUIReload to remake your managed UI elements.

Example: `KiraiLib.Events.OnUIReload += () => { VRChat_OnUIManagerInit(); };`

# UI Usage

Before using UI, call `KiraiLib.UI.Initialize`

`KiraiLib.UI` has 4 classes, `Toggle`, `Button`, `Slider`, and `Label`. 

Use the `Create` method on these to make a new instance of them rather than using new on the class itself. Example: `KiraiLib.UI.Toggle.Create(...);`

Additionally UI itself has a method called `CreatePage`. Pages are automatically added to a list at `KiraiLib.UI.pages`, your page is located at the return value of `CreatePage`.

If you create a UI element with `managed` to `false` then it will not automatically get unloaded. If you don't delete it yourself you will cause a memory leak when the UI is reloaded.

Note: Buttons may require a `\n` as they often don't line wrap on their own.

# Todo
- [X] Logger
- [X] Button API
- [ ] Attribute based button loader
- [ ] Complete documentation
