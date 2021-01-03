# KiraiLib
Utility library for VRChat

# Loading
There are 4 ways to load the library correctly:
  1. Using Assembly.Load with the bytes
  2. Placing the DLL into the same folder as VRChat.exe
  3. Adding the mod to MelonHandler.Mods
  4. Placing the mod into the mods folder
  
Here is an example of the first loading method using a manifest resource stream to get the bytes
```cs
public class KiraiMod : MelonMod
{
    static KiraiMod() // .cctor
    {
        System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KiraiMod.Lib.KiraiLib.dll");
        System.IO.MemoryStream mem = new System.IO.MemoryStream((int)stream.Length);
        stream.CopyTo(mem);
        
        Assembly.Load(mem.ToArray());
    }
}
```

Once the assembly is loaded you can use it freely. The process of loading it by any of these methods will make it instantiate itself.

Note: I would recommend checking if KiraiLib is already loaded and if it is not fetching the latest version of KiraiLib and loading it to preserve compatibility with other mods that need a newer version but can't load it because an older version is already loaded. 

# Logger Usage
All logger functions have documentation, place the XML file in the same folder that you reference the library to see them in Visual Studio.

# Callback Usage

KiraiLib supports unloading and reloading, due to this you will need to subscribe to some callbacks.

OnUIUnload is optional but you probably want to subscribe to OnUIReload to remake your managed UI elements.

Example: `KiraiLib.Callbacks.OnUIReload += () => { VRChat_OnUIManagerInit(); };`

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
