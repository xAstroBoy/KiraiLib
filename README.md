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
    public KiraiMod()
    {
        System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KiraiMod.Lib.KiraiLib.dll");
        System.IO.MemoryStream mem = new System.IO.MemoryStream((int)stream.Length);
        stream.CopyTo(mem);
        
        Assembly.Load(mem.ToArray());
    }
}
```

Once the assembly is loaded you can use it freely. The process of loading it by any of these methods will make it instantiate itself.

# Usage
All functions have documentation, place the XML file in the same folder that you reference the library to see them in Visual Studio. 

# Todo
- [X] Logger
- [ ] Button API
