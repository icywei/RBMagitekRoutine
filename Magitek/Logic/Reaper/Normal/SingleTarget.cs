using ff14bot;
using ff14bot.Managers;
using Magitek.Enumerations;
using Magitek.Extensions;
using Magitek.Logic.Roles;
using Magitek.Models.Reaper;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace Magitek.Logic.Reaper
{
    internal static class SingleTarget
    {
        //Expire Check Missing
        //Something like TTK > Current GCD 
        public static async Task<bool> ShadowOfDeath()
        {
            if (!ReaperSettings.Instance.UseShadowOfDeath)
                return false;

            if (Core.Me.HasAura(Auras.SoulReaver))
                return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;

            if (Utilities.Routines.Reaper.EnemiesAroundPlayer5Yards >= ReaperSettings.Instance.WhorlOfDeathTargetCount)
                return false;

            if (Core.Me.CurrentTarget.HasAura(Auras.DeathsDesign, true) && Core.Me.CurrentTarget.HasAura(Auras.DeathsDesign, true, (int)Spells.Slice.AdjustedCooldown.TotalMilliseconds))
                return false;

            if (Utilities.Routines.Reaper.CheckTTDIsEnemyDyingSoon())
                return false;

            return await Spells.ShadowOfDeath.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> ShadowOfDeathIdle()
        {
            if (!ReaperSettings.Instance.UseShadowOfDeath)
                return false;

            if (Core.Me.HasAura(Auras.SoulReaver))
                return false;

            if (Core.Me.HasAura(Auras.Executioner)) return false;

            if (Utilities.Routines.Reaper.EnemiesAroundPlayer5Yards >= ReaperSettings.Instance.WhorlOfDeathTargetCount)
                return false;

            if (Core.Me.CurrentTarget.HasAura(Auras.DeathsDesign, true) && Core.Me.CurrentTarget.HasAura(Auras.DeathsDesign, true, 30000 - (int)Spells.Slice.AdjustedCooldown.TotalMilliseconds))
                return false;

            if (Utilities.Routines.Reaper.CheckTTDIsEnemyDyingSoon())
                return false;

            return await Spells.ShadowOfDeath.Cast(Core.Me.CurrentTarget);
        }

        #region SoulGaugeGenerator
        public static async Task<bool> Slice()
        {
            if (!ReaperSettings.Instance.UseSlice) return false;
            if (!await Spells.Slice.Cast(Core.Me.CurrentTarget)) return false;
            if (Core.Me.HasAura(Auras.SoulReaver)) return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;
            Utilities.Routines.Reaper.CurrentComboStage = ReaperComboStages.WaxingSlice;
            return true;

        }

        public static async Task<bool> WaxingSlice()
        {
            if (!ReaperSettings.Instance.UseWaxingSlice) return false;
            if (Utilities.Routines.Reaper.CurrentComboStage != ReaperComboStages.WaxingSlice) return false;
            if (ActionManager.ComboTimeLeft <= 0) return false;

            if (!await Spells.WaxingSlice.Cast(Core.Me.CurrentTarget)) return false;
            if (Core.Me.HasAura(Auras.SoulReaver)) return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;
            Utilities.Routines.Reaper.CurrentComboStage = ReaperComboStages.InfernalSlice;
            return true;

        }

        public static async Task<bool> InfernalSlice()
        {
            if (!ReaperSettings.Instance.UseInfernalSlice) return false;
            if (Utilities.Routines.Reaper.CurrentComboStage != ReaperComboStages.InfernalSlice) return false;
            if (ActionManager.ComboTimeLeft <= 0) return false;

            if (!await Spells.InfernalSlice.Cast(Core.Me.CurrentTarget)) return false;
            if (Core.Me.HasAura(Auras.SoulReaver)) return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;
            Utilities.Routines.Reaper.CurrentComboStage = ReaperComboStages.Slice;
            return true;

        }

        public static async Task<bool> SoulSlice()
        {
            if (!ReaperSettings.Instance.UseSoulSlice) return false;
            if (ReaperSettings.Instance.UseSoulScythe &&
                Utilities.Routines.Reaper.EnemiesAroundPlayer5Yards >= ReaperSettings.Instance.SoulScytheTargetCount)
                return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;
            if (Core.Me.HasAura(Auras.SoulReaver)) return false;

            //Keep SoulSlice/SoulScythe Charges at a maximum
            /*
            if (Spells.SoulSlice.Charges <= 1) return false;
            if (Spells.SoulSlice.Cooldown > Spells.Slice.Cooldown) return false;
            */
            if (ActionResourceManager.Reaper.SoulGauge > 50) return false;

            return await Spells.SoulSlice.Cast(Core.Me.CurrentTarget);
        }

        #endregion

        #region SoulShroudGenerator

        public static async Task<bool> GibbetAndGallows()
        {
            if (!Core.Me.HasAnyAura(new uint[] { Auras.Executioner, Auras.SoulReaver })) return false;

            ff14bot.Objects.SpellData gibbetspell;
            ff14bot.Objects.SpellData gallowsspell;

            if (Core.Me.HasAura(Auras.Executioner))
            {
                gibbetspell = Spells.ExecutionersGibbet;
                gallowsspell = Spells.ExecutionersGallows;
            }
            else
            {
                gibbetspell = Spells.Gibbet;
                gallowsspell = Spells.Gallows;
            }

            if (Core.Me.HasAura(Auras.EnhancedGibbet) && ReaperSettings.Instance.UseGibbet)
                return await gibbetspell.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.EnhancedGallows) && ReaperSettings.Instance.UseGallows)
                return await gallowsspell.Cast(Core.Me.CurrentTarget);

            bool canUseGallows = ReaperSettings.Instance.UseGallows;
            bool canUseGibbet = ReaperSettings.Instance.UseGibbet;

            if (ReaperSettings.Instance.EnemyIsOmni || (!Core.Me.CurrentTarget.IsBehind && !Core.Me.CurrentTarget.IsFlanking))
            {
                if (canUseGallows) return await gallowsspell.Cast(Core.Me.CurrentTarget);
                if (canUseGibbet) return await gibbetspell.Cast(Core.Me.CurrentTarget);
            }
            else if (Core.Me.CurrentTarget.IsBehind && canUseGallows)
            {
                return await gallowsspell.Cast(Core.Me.CurrentTarget);
            }
            else if (Core.Me.CurrentTarget.IsFlanking && canUseGibbet)
            {
                return await gibbetspell.Cast(Core.Me.CurrentTarget);
            }

            return false;
        }

        #endregion

        #region SoulGaugeSpender

        public static async Task<bool> BloodStalk()
        {
            //Add level check so it doesn't hang here
            if (Core.Me.ClassLevel < Spells.BloodStalk.LevelAcquired)
                return false;
            if (!ReaperSettings.Instance.UseBloodStalk) return false;
            if (Core.Me.ClassLevel >= Spells.Gluttony.LevelAcquired)
            {
                if (Spells.Gluttony.Cooldown.Ticks == 0)
                    return false;
                if (Spells.Gluttony.AdjustedCooldown - Spells.Gluttony.Cooldown <= Spells.Slice.AdjustedCooldown)
                    return false;
                if (ReaperSettings.Instance.GluttonySaveSoulGuage
                    && Spells.Gluttony.Cooldown.TotalSeconds <= ReaperSettings.Instance.GluttonySaveSoulGuageCooldown
                    && ActionResourceManager.Reaper.SoulGauge < 100 && Spells.SoulSlice.Charges < 1)
                    return false;
            }
            if (Core.Me.HasAura(Auras.SoulReaver)) return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;
            if (!Core.Me.CurrentTarget.HasAura(Auras.DeathsDesign, true)) return false;
            if (ActionResourceManager.Reaper.ShroudGauge > 90)
                return false;
            if (Utilities.Routines.Reaper.CheckTTDIsEnemyDyingSoon())
                return false;
            if (Core.Me.ClassLevel >= Spells.Gibbet.LevelAcquired)
                return await Spells.BloodStalk.CastAura(Core.Me.CurrentTarget, Auras.SoulReaver, auraTarget: Core.Me);
            else
                return await Spells.BloodStalk.Cast(Core.Me.CurrentTarget);
        }

        #endregion

        public static async Task<bool> HarvestMoon()
        {

            if (!ReaperSettings.Instance.UseHarvestMoon)
                return false;

            if (!Core.Me.HasAura(Auras.Soulsow))
                return false;

            if (Core.Me.HasAura(Auras.SoulReaver))
                return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;

            //if (!Core.Me.CurrentTarget.HasAura(Auras.DeathsDesign, true))
            //    return false;

            return await Spells.HarvestMoon.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> EnhancedHarpe()
        {
            if (Core.Me.ClassLevel < 20)
                return false;

            if (!ReaperSettings.Instance.UseEnhancedHarpe)
                return false;

            if (!Core.Me.HasAura(Auras.EnhancedHarpe))
                return false;

            if (!GameSettingsManager.FaceTargetOnAction && !Core.Me.CurrentTarget.InView())
                return false;

            if (ReaperSettings.Instance.UseEnhancedHarpeAfterHellEgress && Core.Me.CurrentTarget.Distance(Core.Me) <= 3)
                return false;

            if (ReaperSettings.Instance.UseEnhancedHarpeAfterHellEgress && !Spells.HellsIngress.IsKnownAndReady(18500))
                return false;

            return await Spells.Harpe.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> Harpe()
        {
            if (Core.Me.ClassLevel < 15)
                return false;

            if (!ReaperSettings.Instance.UseEnhancedHarpe)
                return false;

            if (Core.Me.HasAura(Auras.EnhancedHarpe))
                return false;

            if (MovementManager.IsMoving)
                return false;

            if (ReaperSettings.Instance.UseRangeHarpe && Core.Me.CurrentTarget.Distance(Core.Me) <= 3)
                return false;

            if (!ReaperSettings.Instance.UseRangeHarpe && Core.Me.CurrentTarget.Distance(Core.Me) > 3)
                return false;

            if (Core.Me.HasAura(Auras.SoulReaver)) return false;
            if (Core.Me.HasAura(Auras.Executioner)) return false;

            return await Spells.Harpe.Cast(Core.Me.CurrentTarget);
        }

        /**********************************************************************************************
        *                              Limit Break
        * ********************************************************************************************/
        public static bool ForceLimitBreak()
        {
            if (!Core.Me.HasTarget)
                return false;

            return PhysicalDps.ForceLimitBreak(Spells.Braver, Spells.Bladedance, Spells.TheEnd, Spells.Slice);
        }

    }
}
