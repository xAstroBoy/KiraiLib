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

            // from https://stackoverflow.com/a/41407246/9281083
            public const string CC_Reset      = "\x1b[0m";
            public const string CC_Bright     = "\x1b[1m";
            public const string CC_Dim        = "\x1b[2m";
            public const string CC_Underscore = "\x1b[4m";
            public const string CC_Blink      = "\x1b[5m";
            public const string CC_Reverse    = "\x1b[7m";
            public const string CC_Hidden     = "\x1b[8m";
            public const string CC_FgBlack    = "\x1b[30m";
            public const string CC_FgRed      = "\x1b[31m";
            public const string CC_FgGreen    = "\x1b[32m";
            public const string CC_FgYellow   = "\x1b[33m";
            public const string CC_FgBlue     = "\x1b[34m";
            public const string CC_FgMagenta  = "\x1b[35m";
            public const string CC_FgCyan     = "\x1b[36m";
            public const string CC_FgWhite    = "\x1b[37m";
            public const string CC_BgBlack    = "\x1b[40m";
            public const string CC_BgRed      = "\x1b[41m";
            public const string CC_BgGreen    = "\x1b[42m";
            public const string CC_BgYellow   = "\x1b[43m";
            public const string CC_BgBlue     = "\x1b[44m";
            public const string CC_BgMagenta  = "\x1b[45m";
            public const string CC_BgCyan     = "\x1b[46m";
            public const string CC_BgWhite    = "\x1b[47m";
        }
    }
}
