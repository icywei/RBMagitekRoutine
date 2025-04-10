using ff14bot;
using ff14bot.Managers;
using Magitek.Enumerations;
using Magitek.Extensions;
using Magitek.Logic.Roles;
using Magitek.Models.Account;
using Magitek.Models.Gunbreaker;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;
using static ff14bot.Managers.ActionResourceManager.Gunbreaker;
using GunbreakerRoutine = Magitek.Utilities.Routines.Gunbreaker;

namespace Magitek.Logic.Gunbreaker
{
    internal static class Buff
    {
        public static async Task<bool> RoyalGuard() //Tank stance
        {
            switch (GunbreakerSettings.Instance.UseRoyalGuard)
            {
                case true:
                    if (!Core.Me.HasAura(Auras.RoyalGuard))
                        return await Spells.RoyalGuard.CastAura(Core.Me, Auras.RoyalGuard);
                    break;

                case false:
                    if (Core.Me.HasAura(Auras.RoyalGuard))
                        return await Spells.RoyalGuard.Cast(Core.Me);
                    break;
            }
            return false;
        }

        public static async Task<bool> NoMercy() // Damage Buff +20%
        {
            if (!GunbreakerSettings.Instance.UseNoMercy || !Spells.NoMercy.IsKnownAndReady())
                return false;

            if (GunbreakerSettings.Instance.BurstLogicHoldBurst)
                return false;

            if (GunbreakerSettings.Instance.BurstLogicHoldBurstWhenMoving && MovementManager.IsMoving)
                return false;

            //Force Delay CD
            //if (Spells.KeenEdge.Cooldown.TotalMilliseconds > Globals.AnimationLockMs + BaseSettings.Instance.UserLatencyOffset + 100)
            //    return false;

            //Force Delay when pulling
            if (Casting.LastSpell == Spells.LightningShot)
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit())
                return false;

            if (GunbreakerSettings.Instance.GunbreakerStrategy.Equals(GunbreakerStrategy.SlowGCD))
            {
                if (Cartridge < 2)
                    return false;
            }

            if (GunbreakerSettings.Instance.GunbreakerStrategy.Equals(GunbreakerStrategy.FastGCD))
            {
                if (Cartridge < 2 && ActionManager.LastSpell.Id == Spells.KeenEdge.Id)
                    return false;
            }

            return await Spells.NoMercy.Cast(Core.Me);
        }

        public static async Task<bool> Bloodfest() // +2 or +3 cartrige
        {
            if (!GunbreakerSettings.Instance.UseBloodfest || !Spells.Bloodfest.IsKnownAndReady())
                return false;

            if (Cartridge > GunbreakerRoutine.MaxCartridge - GunbreakerRoutine.AmountCartridgeFromBloodfest)
                return false;

            if (GunbreakerSettings.Instance.BurstLogicHoldBurst)
                return false;

            if (GunbreakerSettings.Instance.BurstLogicHoldBurstWhenMoving && MovementManager.IsMoving)
                return false;

            return await Spells.Bloodfest.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> UsePotion()
        {
            if (Spells.NoMercy.IsKnown() && !Spells.NoMercy.IsReady(3000))
                return false;

            return await Tank.UsePotion(GunbreakerSettings.Instance);
        }
    }
}