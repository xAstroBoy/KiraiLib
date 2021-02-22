namespace KiraiMod
{
    public partial class KiraiLib
    {
        /// <summary> Useful constant values </summary>
        public static class Constants
        {
            /// <summary> Moves back a character in the console when \b doesn't work </summary>
            public const string VT_BACKSPACE = "\x1b[1D";
            /// <summary> Replaces the previous character in the console with a space </summary>
            public const string VT_ERASE = VT_BACKSPACE + " ";
        }
    }
}
