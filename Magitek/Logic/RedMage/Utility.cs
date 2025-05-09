﻿using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.RedMage;
using Magitek.Utilities;
using System.Linq;
using static ff14bot.Managers.ActionResourceManager.RedMage;
using RedMageRoutine = Magitek.Utilities.Routines.RedMage;

namespace Magitek.Logic.RedMage
{
    internal static class Utility
    {
        public static bool InCombo()
        {
            if (ActionManager.ComboTimeLeft <= 0)
                return false;

            if (Core.Me.ClassLevel < 35)
                return false;

            if (Core.Me.ClassLevel >= 35
                && Core.Me.ClassLevel < 50)
            {
                if (RedMageRoutine.CanContinueComboAfter(Spells.Riposte) || RedMageRoutine.CanContinueComboAfter(Spells.EnchantedRiposte))
                    return true;
            }

            if (Core.Me.ClassLevel >= 50)
            {
                if (RedMageRoutine.CanContinueComboAfter(Spells.Riposte) || RedMageRoutine.CanContinueComboAfter(Spells.EnchantedRiposte)
                || RedMageRoutine.CanContinueComboAfter(Spells.Zwerchhau) || RedMageRoutine.CanContinueComboAfter(Spells.EnchantedZwerchhau))
                    return true;
            }
            return false;
        }
        public static int ManaStacks()
        {

            return ActionResourceManager.RedMage.ManaStacks;
        }

        public static bool InAoeCombo()
        {

            if (Core.Me.ClassLevel < Spells.Moulinet.LevelAcquired)
                return false;

            if (!RedMageSettings.Instance.UseAoe)
                return false;

            if (Core.Me.ClassLevel <= Spells.EnchantedMoulinet.LevelAcquired)
                return false;

            if (!RedMageRoutine.CanContinueComboAfter(Spells.EnchantedMoulinet)
                && !RedMageRoutine.CanContinueComboAfter(Spells.EnchantedMoulinetDeux)
                && !RedMageRoutine.CanContinueComboAfter(Spells.EnchantedMoulinetTrois))
                return false;

            if (ManaStacks() == 3)
                return false;

            return true;
        }

        public static bool InComboEnder()
        {
            if (Core.Me.ClassLevel < 80)
                return false;

            if (Core.Me.ClassLevel >= 80 && Core.Me.ClassLevel < 90)
            {
                if ((Casting.SpellCastHistory.Take(3).Any(s => s.Spell == Spells.Verholy)
                    || Casting.SpellCastHistory.Take(3).Any(s => s.Spell == Spells.Verflare))
                    && !Casting.SpellCastHistory.Take(3).Any(s => s.Spell == Spells.Scorch || s.Spell == Spells.Jolt || s.Spell == Spells.Resolution))
                    return true;
            }

            if ((Casting.SpellCastHistory.Take(6).Any(s => s.Spell == Spells.Verholy)
                || Casting.SpellCastHistory.Take(6).Any(s => s.Spell == Spells.Verflare))
                && (Casting.SpellCastHistory.Take(3).Count(s => s.Spell == Spells.Scorch || s.Spell == Spells.Jolt || s.Spell == Spells.Resolution) < 2))
                return true;

            return false;
        }

        public static bool ShouldApproachForCombo()
        {
            if (!Core.Me.HasTarget)
                return false;

            if (Core.Me.HasAura(Auras.MagickedSwordplay, true) || InAoeCombo() || InCombo() || !(WhiteMana < 50 || BlackMana < 50))
                return (Core.Me.CurrentTarget.Distance() > (3 + Core.Me.CurrentTarget.CombatReach));

            return false;

        }
    }
}
