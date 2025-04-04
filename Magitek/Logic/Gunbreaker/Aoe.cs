﻿using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.Gunbreaker;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;
using static ff14bot.Managers.ActionResourceManager.Gunbreaker;
using GunbreakerRoutine = Magitek.Utilities.Routines.Gunbreaker;

namespace Magitek.Logic.Gunbreaker
{
    internal static class Aoe
    {

        /*************************************************************************************
         *                                    Combo
         * ***********************************************************************************/
        public static async Task<bool> DemonSlice()
        {
            if (!GunbreakerSettings.Instance.UseAoe)
                return false;

            if (Combat.Enemies.Count(r => r.Distance(Core.Me) <= 5 + r.CombatReach) < GunbreakerSettings.Instance.UseAoeEnemies)
                return false;

            if (GunbreakerRoutine.IsAurasForComboActive())
                return false;

            return await Spells.DemonSlice.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> DemonSlaughter()
        {
            if (!GunbreakerSettings.Instance.UseAoe)
                return false;

            if (!GunbreakerRoutine.CanContinueComboAfter(Spells.DemonSlice))
                return false;

            if (GunbreakerRoutine.IsAurasForComboActive())
                return false;

            if (Combat.Enemies.Count(r => r.Distance(Core.Me) <= 5 + r.CombatReach) < GunbreakerSettings.Instance.UseAoeEnemies)
                return false;

            if (Cartridge == GunbreakerRoutine.MaxCartridge)
                return false;

            return await Spells.DemonSlaughter.Cast(Core.Me.CurrentTarget);
        }

        /*************************************************************************************
         *                                    GCD
         * ***********************************************************************************/
        public static async Task<bool> FatedCircle()
        {
            if (!GunbreakerSettings.Instance.UseAoe)
                return false;

            if (!GunbreakerSettings.Instance.UseFatedCircle)
                return false;

            if (Cartridge < GunbreakerRoutine.RequiredCartridgeForFatedCircle)
                return false;

            if (GunbreakerRoutine.IsAurasForComboActive())
                return false;

            if (Combat.Enemies.Count(r => r.Distance(Core.Me) <= 5 + r.CombatReach) < GunbreakerSettings.Instance.UseAoeEnemies)
                return false;

            if (Core.Me.HasAura(Auras.NoMercy) && Cartridge > 0)
            {
                if (Spells.DoubleDown.IsKnownAndReady())
                    return false;

                return await Spells.FatedCircle.Cast(Core.Me.CurrentTarget);
            }

            //Delay if nomercy ready soon
            //   if (Spells.NoMercy.IsKnownAndReady(16000) && Cartridge < GunbreakerRoutine.MaxCartridge - 1)
            //       return false;
            //   if (Spells.NoMercy.IsKnownAndReady(8000) && Cartridge < GunbreakerRoutine.MaxCartridge)
            //       return false;

            //Delay if GnashingFang ready soon and there are less than GunbreakerSettings.Instance.PrioritizeFatedCircleOverGnashingFangEnemies
            //    if (Combat.Enemies.Count(r => r.Distance(Core.Me) <= 5 + r.CombatReach) < GunbreakerSettings.Instance.PrioritizeFatedCircleOverGnashingFangEnemies)
            //   {
            //       if (Spells.GnashingFang.IsKnownAndReady(8000) && Cartridge <= GunbreakerRoutine.RequiredCartridgeForGnashingFang)
            //           return false;
            //   }

            //Delay if DoubleDown ready soon
            //   if (Spells.DoubleDown.IsKnownAndReady(4000) && Cartridge <= GunbreakerRoutine.RequiredCartridgeForDoubleDown)
            //       return false;

            if (Cartridge == GunbreakerRoutine.MaxCartridge && ActionManager.LastSpell.Id != Spells.DemonSlice.Id)
                return false;

            if (Cartridge < GunbreakerRoutine.MaxCartridge)
                return false;

            return await Spells.FatedCircle.Cast(Core.Me.CurrentTarget);
        }

        /*************************************************************************************
         *                                    oGCD
         * ***********************************************************************************/
        public static async Task<bool> BowShock()
        {
            if (!GunbreakerSettings.Instance.UseBowShock)
                return false;

            if (!Core.Me.HasAura(Auras.NoMercy) && GunbreakerSettings.Instance.BurstLogicHoldBurst && !GunbreakerSettings.Instance.BurstLogicExcludeBowShock)
                return false;

            if (GunbreakerRoutine.IsAurasForComboActive())
                return false;

            if (Combat.Enemies.Count(r => r.Distance(Core.Me) <= 5 + r.CombatReach) < GunbreakerSettings.Instance.BowShockEnemies)
                return false;

            if (Core.Me.HasAura(Auras.NoMercy) && Spells.DoubleDown.IsKnownAndReady() && Cartridge > 1)
                return false;

            return await Spells.BowShock.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> FatedBrand()
        {
            if (Core.Me.ClassLevel < 96)
                return false;

            if (!GunbreakerSettings.Instance.UseAoe)
                return false;

            if (Combat.Enemies.Count(r => r.Distance(Core.Me) <= 5 + r.CombatReach) < GunbreakerSettings.Instance.UseAoeEnemies)
                return false;

            return await Spells.FatedBrand.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> DoubleDown()
        {
            if (!GunbreakerSettings.Instance.UseDoubleDown)
                return false;

            if (!Core.Me.HasAura(Auras.NoMercy))
                return false;

            if (Cartridge < GunbreakerRoutine.RequiredCartridgeForDoubleDown)
                return false;

            if (Spells.GnashingFang.IsKnownAndReady(1000) && Cartridge >= 1)
                return false;

            return await Spells.DoubleDown.Cast(Core.Me.CurrentTarget);
        }
    }
}