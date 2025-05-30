﻿using ff14bot;
using ff14bot.Objects;
using Magitek.Extensions;
using Magitek.Logic.Roles;
using Magitek.Models.Machinist;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Auras = Magitek.Utilities.Auras;
using System.Collections.Generic;
using System;

namespace Magitek.Logic.Machinist
{
    internal static class Pvp
    {
        private static GameObject WildfireTarget { get; set; }
        private static int WildfireStacks { get; set; }

        private static async Task IncrementWildfireStacks(int stacks = 1)
        {
            if (WildfireTarget != null && Casting.SpellTarget == WildfireTarget)
                WildfireStacks += stacks;
        }

        public static async Task<bool> BlastedCharge()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.BlastChargePvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.BlastChargePvp.Range))
                return false;

            return await Spells.BlastChargePvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks());
        }

        public static async Task<bool> BlazingShot()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.BlazingShotPvp.CanCast())
                return false;

            if (!Core.Me.HasAura(Auras.PvpOverheated))
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.BlazingShotPvp.Range))
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.BlazingShotPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks());
        }

        public static async Task<bool> WildFire()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!MachinistSettings.Instance.Pvp_Wildfire)
                return false;

            if (Core.Me.HasAura(Auras.PvpWildfireBuff))
                return false;

            if (!Spells.WildfirePvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.WildfirePvp.Range))
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.WildfirePvp.CastAura(Core.Me.CurrentTarget, aura: Auras.PvpWildfireBuff, auraTarget: Core.Me,
                callback: async () =>
                {
                    WildfireTarget = Casting.SpellTarget;
                    WildfireStacks = 0;
                });
        }

        public static async Task<bool> Detonator()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!MachinistSettings.Instance.Pvp_Wildfire)
                return false;

            if (!Spells.DetonatorPvp.CanCast())
                return false;

            if (WildfireTarget == null || !WildfireTarget.HasAura(Auras.PvpWildfire))
            {
                WildfireTarget = null;
                WildfireStacks = 0;
                return false;
            }

            if (CommonPvp.GuardCheck(MachinistSettings.Instance, WildfireTarget))
            {
                return false;
            }

            // Estimate damage based on stacks (4000 potency per stack)
            // Using a conservative damage conversion factor for PvP
            const double PvpDamageConversionFactor = 0.8;
            var estimatedDamage = 4000 * WildfireStacks * PvpDamageConversionFactor;

            // If target is a tank, reduce expected damage
            if (WildfireTarget.IsTank())
            {
                estimatedDamage *= 0.8;
            }

            // If target has Chain Saw vulnerability, increase damage by 20%
            if (WildfireTarget.HasAura(Auras.PvpChainSaw))
            {
                estimatedDamage *= 1.2;
            }

            var targetCurrentHp = WildfireTarget.CurrentHealth;
            var wouldKill = targetCurrentHp <= estimatedDamage;

            if (wouldKill || (WildfireStacks > 0 && !WildfireTarget.WithinSpellRange(Spells.BlastChargePvp.Range + 5)))
            {
                return await Spells.DetonatorPvp.Cast(Core.Me);
            }

            return false;
        }

        public static async Task<bool> FullMetalField()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!MachinistSettings.Instance.Pvp_FullMetalField)
                return false;

            if (!Spells.FullMetalFieldPvp.CanCast())
                return false;

            if (MachinistSettings.Instance.Pvp_SaveFullMetalForWildfire)
            {
                var wildfireReady = Spells.WildfirePvp.Cooldown.TotalSeconds;
                if (wildfireReady > 0 && wildfireReady <= 8)
                    return false;
            }

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.FullMetalFieldPvp.Range))
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.FullMetalFieldPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks(2));
        }

        public static async Task<bool> Scattergun()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!MachinistSettings.Instance.Pvp_Scattergun)
                return false;

            if (!Spells.ScattergunPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.ScattergunPvp.Range))
                return false;

            if (Combat.Enemies.Count(x => x.WithinSpellRange(Spells.ScattergunPvp.Range)) < 1)
                return false;

            return await Spells.ScattergunPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks(2));
        }

        public static async Task<bool> Analysis()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.AnalysisPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            if (Core.Me.HasAura(Auras.PvpAnalysis))
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.AnalysisPvp.Range))
                return false;

            if (!MachinistSettings.Instance.Pvp_UsedAnalysisOnDrill && Core.Me.HasAura(Auras.PvpDrillPrimed))
                return false;

            if (!MachinistSettings.Instance.Pvp_UsedAnalysisOnBio && Core.Me.HasAura(Auras.PvpBioPrimed))
                return false;

            if (!MachinistSettings.Instance.Pvp_UsedAnalysisOnAA && Core.Me.HasAura(Auras.PvpAirAnchorPrimed))
                return false;

            if (!MachinistSettings.Instance.Pvp_UsedAnalysisOnChainSaw && Core.Me.HasAura(Auras.PvpChainSawPrimed))
                return false;

            return await Spells.AnalysisPvp.Cast(Core.Me);
        }

        public static async Task<bool> Drill()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.DrillPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.DrillPvp.Range))
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.DrillPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks(1));
        }

        public static async Task<bool> BioBlaster()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.BioblasterPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.BioblasterPvp.Range))
            {
                var nearby = Combat.Enemies
                    .Where(e => e.WithinSpellRange(Spells.BioblasterPvp.Range)
                            && e.ValidAttackUnit()
                            && e.InLineOfSight()
                            && !e.IsWarMachina())
                    .OrderBy(e => e.Distance(Core.Me));
                var nearbyTarget = nearby.FirstOrDefault();

                if (nearbyTarget != null)
                {
                    return await Spells.BioblasterPvp.Cast(nearbyTarget);
                }

                return false;
            }

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.BioblasterPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks(1));
        }

        public static async Task<bool> AirAnchor()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.AirAnchorPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.AirAnchorPvp.Range))
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.AirAnchorPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks(1));
        }

        public static async Task<bool> ChainSaw()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!Spells.ChainSawPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.ChainSawPvp.Range))
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await Spells.ChainSawPvp.Cast(Core.Me.CurrentTarget, callback: async () => await IncrementWildfireStacks(1));
        }

        public static async Task<bool> BishopAutoturret()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!MachinistSettings.Instance.Pvp_BishopAutoturret)
                return false;

            if (!Spells.BishopAutoturretPvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.WithinSpellRange(Spells.BishopAutoturretPvp.Range))
                return false;

            if (Combat.Enemies.Count(x => x.Distance(Core.Me.CurrentTarget) < Spells.BishopAutoturretPvp.Radius) < MachinistSettings.Instance.Pvp_BishopAutoturretNumberOfEnemy)
                return false;

            return await Spells.BishopAutoturretPvp.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> MarksmansSpite()
        {
            if (Core.Me.HasAura(Auras.PvpGuard))
                return false;

            if (!MachinistSettings.Instance.Pvp_UseMarksmansSpite)
                return false;

            if (!Spells.MarksmansSpitePvp.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            if (Core.Me.CurrentTarget.CurrentHealthPercent > MachinistSettings.Instance.Pvp_UseMarksmansSpiteHealthPercent)
            {
                if (MachinistSettings.Instance.Pvp_UseMarksmansSpiteAnyTarget)
                {
                    var nearby = Combat.Enemies
                        .Where(e => e.WithinSpellRange(Spells.MarksmansSpitePvp.Range)
                                && e.ValidAttackUnit()
                                && e.InLineOfSight()
                                && e.CurrentHealthPercent <= MachinistSettings.Instance.Pvp_UseMarksmansSpiteHealthPercent
                                && !e.IsWarMachina()
                                && !CommonPvp.GuardCheck(MachinistSettings.Instance, e)
                                // Check if too many allies are targeting this enemy
                                && !CommonPvp.TooManyAlliesTargeting(MachinistSettings.Instance, e))
                        .OrderBy(e => e.Distance(Core.Me));

                    var nearbyTarget = nearby.FirstOrDefault();

                    if (nearbyTarget != null)
                        return await Spells.MarksmansSpitePvp.Cast(nearbyTarget);
                }

                return false;
            }

            // Check if too many allies are targeting current target
            if (CommonPvp.TooManyAlliesTargeting(MachinistSettings.Instance))
                return false;

            return await Spells.MarksmansSpitePvp.Cast(Core.Me.CurrentTarget);
        }
    }
}
