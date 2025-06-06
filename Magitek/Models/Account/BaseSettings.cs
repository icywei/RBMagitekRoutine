﻿using ff14bot.Enums;
using PropertyChanged;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Windows.Input;

namespace Magitek.Models.Account
{
    [AddINotifyPropertyChangedInterface]
    public class BaseSettings : JsonSettings
    {
        public BaseSettings() : base(CharacterSettingsDirectory + "/Magitek/BaseSettings.json") { }

        private static BaseSettings _instance;

        public static BaseSettings Instance => _instance ?? (_instance = new BaseSettings());

        private const double DefaultOverlayPosX = 60;
        private const double DefaultOverlayPosY = 60;
        private const double DefaultCombatMessageOverlayWidth = -1;
        private const double DefaultCombatMessageOverlayHeight = -1;
        private const double DefaultCombatMessageOverlayPosX = -1;
        private const double DefaultCombatMessageOverlayPosY = -1;

        public static void ResetOverlayPositions()
        {
            Instance.OverlayPosX = DefaultOverlayPosX;
            Instance.OverlayPosY = DefaultOverlayPosY;
            Instance.CombatMessageOverlayWidth = DefaultCombatMessageOverlayWidth;
            Instance.CombatMessageOverlayHeight = DefaultCombatMessageOverlayHeight;
            Instance.CombatMessageOverlayPosX = DefaultCombatMessageOverlayPosX;
            Instance.CombatMessageOverlayPosY = DefaultCombatMessageOverlayPosY;
            Instance.Save();
        }

        /// <summary>
        /// Validates and corrects the settings window position to ensure it appears on screen.
        /// For single monitor setups, negative coordinates will be reset to default values.
        /// For multi-monitor setups, negative coordinates are preserved to allow positioning on secondary monitors.
        /// </summary>
        /// <param name="windowWidth">The width of the settings window</param>
        /// <param name="windowHeight">The height of the settings window</param>
        public static void ValidateSettingsWindowPosition(double windowWidth = 1000, double windowHeight = 700)
        {
            var screens = Screen.AllScreens;
            var isMultiMonitor = screens.Length > 1;

            var currentX = Instance.SettingsWindowPosX;
            var currentY = Instance.SettingsWindowPosY;
            var needsCorrection = false;

            // For single monitor setups, don't allow negative coordinates
            if (!isMultiMonitor)
            {
                if (currentX < 0 || currentY < 0)
                {
                    needsCorrection = true;
                }
                else
                {
                    // Also check if the window would be completely off-screen on the right/bottom
                    var primaryScreen = Screen.PrimaryScreen;
                    if (currentX > primaryScreen.WorkingArea.Width - 100 ||
                        currentY > primaryScreen.WorkingArea.Height - 100)
                    {
                        needsCorrection = true;
                    }
                }
            }
            else
            {
                // For multi-monitor setups, check if the top-left corner is on any screen
                // This ensures the title bar is accessible for moving the window
                var isTopLeftOnAnyScreen = false;
                foreach (var screen in screens)
                {
                    var screenBounds = screen.WorkingArea;
                    // Check if the top-left corner is within this screen's bounds
                    if (currentX >= screenBounds.Left &&
                        currentX < screenBounds.Right &&
                        currentY >= screenBounds.Top &&
                        currentY < screenBounds.Bottom)
                    {
                        isTopLeftOnAnyScreen = true;
                        break;
                    }
                }

                if (!isTopLeftOnAnyScreen)
                {
                    needsCorrection = true;
                }
            }

            // Apply correction if needed
            if (needsCorrection)
            {
                Instance.SettingsWindowPosX = 60; // Default position
                Instance.SettingsWindowPosY = 60; // Default position
                Instance.Save();
            }
        }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys UseOpenersModkey { get; set; }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys UseOpenersKey { get; set; }

        [Setting]
        [DefaultValue(ModifierKeys.None)]
        public ModifierKeys ResetOpenersModkey { get; set; }

        [Setting]
        [DefaultValue(Keys.None)]
        public Keys ResetOpenersKey { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool AllowGambits { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseOpeners { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool ResetOpeners { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool UseFightLogic { get; set; }

        [Setting]
        [DefaultValue(20.0f)]
        public float FightLogicResponseDelay { get; set; }

        [Setting]
        public string ContributorKey { get; set; }

        [Setting]
        [DefaultValue(60)]
        public double SettingsWindowPosX { get; set; }

        [Setting]
        [DefaultValue(60)]
        public double SettingsWindowPosY { get; set; }

        [Setting]
        [DefaultValue(10)]
        public double FontSize { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool ZoomHack { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool VersionCheck { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool SettingsWindowTopMost { get; set; }

        #region Debug
        [Setting]
        [DefaultValue(false)]
        public bool DebugPlayerCasting { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugEnemyInfo { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugHealingLists { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugHealingListsPrintToLog { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugSpellCastHistory { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool EnemySpellCastHistory { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool EnemyAuraHistory { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool PartyMemberAuraHistory { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool EnemyTargetHistory { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugCastingCallerMemberName { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugCastingCallerMemberNameIncludePath { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugFightLogic { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DebugFightLogicFound { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DebugActionLockWait2 { get; set; }
        #endregion

        #region Overlay
        [Setting]
        [DefaultValue(DefaultOverlayPosX)]
        public double OverlayPosX { get; set; }

        [Setting]
        [DefaultValue(DefaultOverlayPosY)]
        public double OverlayPosY { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseOverlay { get; set; }

        [Setting]
        [DefaultValue(176)]
        public int OverlayWidth { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UsePositionalOverlay { get; set; }

        [Setting]
        [DefaultValue(100)]
        public double PositionalOverlayWidth { get; set; }

        [Setting]
        [DefaultValue(0.8)]
        public double PositionalOverlayOpacity { get; set; }

        [Setting]
        [DefaultValue(60)]
        public double PositionalOverlayPosX { get; set; }

        [Setting]
        [DefaultValue(60)]
        public double PositionalOverlayPosY { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseCombatMessageOverlay { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool CombatMessageOverlayAdjustable { get; set; }

        [Setting]
        [DefaultValue(DefaultCombatMessageOverlayPosX)]
        public double CombatMessageOverlayPosX { get; set; }

        [Setting]
        [DefaultValue(DefaultCombatMessageOverlayPosY)]
        public double CombatMessageOverlayPosY { get; set; }

        [Setting]
        [DefaultValue(DefaultCombatMessageOverlayWidth)]
        public double CombatMessageOverlayWidth { get; set; }

        [Setting]
        [DefaultValue(DefaultCombatMessageOverlayHeight)]
        public double CombatMessageOverlayHeight { get; set; }
        #endregion

        [Setting]
        [DefaultValue(false)]
        public bool UseCastOrQueue { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool UseAdvancedSpellHistory { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAdvancedSpellHistory2 { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseAutoFaceChecks { get; set; }

        #region chocobo
        [Setting]
        [DefaultValue(true)]
        public bool SummonChocobo { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool ChocoboStanceDance { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float ChocoboStanceDanceHealthPercent { get; set; }

        [Setting]
        [DefaultValue(CompanionStance.Free)]
        public CompanionStance ChocoboStance { get; set; }
        #endregion

        #region General Settings
        [Setting]
        [DefaultValue(0)]
        public int UserLatencyOffset { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool ActiveCombatRoutine { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool ActivePvpCombatRoutine { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool EnableBaseToggle { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool ForceLimitBreak { get; set; }
        #endregion

        [Setting]
        [DefaultValue(false)]
        public bool AssumeFaceTargetOnAction { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool UseWeightedHealingPriority { get; set; }

        [Setting]
        [DefaultValue(.95f)]
        public float WeightedTankRole { get; set; }

        [Setting]
        [DefaultValue(.9f)]
        public float WeightedHealerRole { get; set; }

        [Setting]
        [DefaultValue(1.0f)]
        public float WeightedRezMageRole { get; set; }

        [Setting]
        [DefaultValue(1.01f)]
        public float WeightedDpsRole { get; set; }

        [Setting]
        [DefaultValue(.99f)]
        public float WeightedSelf { get; set; }

        [Setting]
        [DefaultValue(.9f)]
        public float WeightedDebuff { get; set; }

        [Setting]
        [DefaultValue(1.01f)]
        public float WeightedBuff { get; set; }

        [Setting]
        [DefaultValue(1.15f)]
        public float WeightedRegen { get; set; }

        [Setting]
        [DefaultValue(1.15f)]
        public float WeightedShield { get; set; }

        [Setting]
        [DefaultValue(.95f)]
        public float WeightedWeakness { get; set; }

        [Setting]
        [DefaultValue(.99f)]
        public float WeightedDistanceMin { get; set; }

        [Setting]
        [DefaultValue(1.01f)]
        public float WeightedDistanceMax { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool MagitekMovement { get; set; }

    }
}