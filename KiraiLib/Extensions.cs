using System.Linq;
using UnityEngine;
using VRC.Core;

namespace KiraiMod
{
    /// <summary> Helper methods </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the user trust rank as a string
        /// </summary>
        /// <param name="user"> The user you want to convert </param>
        /// <returns> Legendary | Veteran | Trusted | Known | User | New | Visitor | Nuisance | Unknown </returns>
        public static string GetTrustLevel(this APIUser user)
        {
            if (user is null) return "Unknown";

            if (user.hasLegendTrustLevel)
            {
                if (user.tags.Contains("system_legend"))
                    return "Legendary";
                return "Veteran";
            }
            else if (user.hasVeteranTrustLevel) return "Trusted";
            else if (user.hasTrustedTrustLevel) return "Known";
            else if (user.hasKnownTrustLevel) return "User";
            else if (user.hasBasicTrustLevel) return "New";
            else if (user.isUntrusted)
            {
                if (user.tags.Contains("system_probable_troll"))
                    return "Nuisance";
                return "Visitor";
            }
            else return "Unknown";
        }

        /// <summary> Returns a color for the player's rank </summary>
        /// <param name="user"> The user you want to convert </param>
        /// <returns> Color as defined by KiraiLib.Config </returns>
        public static Color GetTrustColor(this APIUser user)
        {
            if (user is null) return Configuration.Unknown;

            if (user.hasLegendTrustLevel)
            {
                if (user.tags.Contains("system_legend"))
                    return Configuration.Legendary;
                return Configuration.Veteran;
            }
            else if (user.hasVeteranTrustLevel) return Configuration.Trusted;
            else if (user.hasTrustedTrustLevel) return Configuration.Known;
            else if (user.hasKnownTrustLevel) return Configuration.User;
            else if (user.hasBasicTrustLevel) return Configuration.NewUser;
            else if (user.isUntrusted)
            {
                if (user.tags.Contains("system_probable_troll"))
                    return Configuration.Nuisance;
                return Configuration.Visitor;
            }
            else return Configuration.Unknown;
        }

        /// <summary> Converts a color to 6 byte wide hex </summary>
        /// <param name="color"> The color to convert </param>
        /// <returns> 6 Byte wide hex starting with # </returns>
        public static string ToHex(this Color color)
        {
            return $"#{(int)(color.r * 255):X2}{(int)(color.g * 255):X2}{(int)(color.b * 255):X2}{(int)(color.a * 255):X2}";
        }
    }
}
