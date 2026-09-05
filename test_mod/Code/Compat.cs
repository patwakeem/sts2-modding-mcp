using System.Linq;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MCPTest
{
    // Shims for game API that moved after this mod was written. Keeping them in one
    // place means the next time the game shifts, the build points here rather than at
    // nine call sites.
    public static class Compat
    {
        // CombatManager.IsPlayPhase is gone. The phase now lives on the player's own
        // PlayerCombatState, which is also where multiplayer keeps it per side, so the
        // question "can the player act" has to be asked of a specific player.
        public static bool IsPlayPhase(CombatManager? cm)
        {
            try
            {
                if (cm == null || !cm.IsInProgress) return false;
                CombatState? st = cm.DebugOnlyGetState();
                if (st == null) return false;

                foreach (Creature c in st.PlayerCreatures)
                {
                    Player? p = c.Player;
                    if (p?.PlayerCombatState == null) continue;
                    if (p.PlayerCombatState.Phase == PlayerTurnPhase.Play) return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
