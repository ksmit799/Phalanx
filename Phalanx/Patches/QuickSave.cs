using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace Phalanx.Patches
{
    [HarmonyPatch(typeof(SaveHandler), "QuickSaveCurrentGame")]
    public static class QuickSave
    {
        private static bool Prefix()
        {
            // Disable quick save if we've joined a game.
            return Phalanx.Host;
        }
    }
}
