using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace Phalanx.Patches
{
    [HarmonyPatch(typeof(SaveHandler), "TryAutoSave")]
    public static class AutoSave
    {
        private static bool Prefix()
        {
            // Disable auto save if we've joined a game.
            return Phalanx.Host;
        }
    }
}
