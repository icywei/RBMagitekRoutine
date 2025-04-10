﻿using Magitek.Models.Roles;
using PropertyChanged;
using System.ComponentModel;
using System.Configuration;

namespace Magitek.Models.Astrologian
{
    [AddINotifyPropertyChangedInterface]
    public class AstrologianSettings : HealerSettings, IRoutineSettings
    {
        public AstrologianSettings() : base(CharacterSettingsDirectory + "/Magitek/Astrologian/AstrologianSettings.json") { }

        public static AstrologianSettings Instance { get; set; } = new AstrologianSettings();

        #region Combat

        [Setting]
        [DefaultValue(true)]
        public bool Malefic { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool InterruptHealing { get; set; }

        [Setting]
        [DefaultValue(90.0f)]
        public float InterruptHealingHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool InterruptDamageToHeal { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DoDamage { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Combust { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool CombustMultipleTargets { get; set; }

        [Setting]
        [DefaultValue(3050)]
        public int CombustRefreshMSeconds { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseTTDForCombust { get; set; }

        [Setting]
        [DefaultValue(21)]
        public int DontCombustIfEnemyDyingWithin { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DontDotIfMoreEnemies { get; set; }

        [Setting]
        [DefaultValue(5)]
        public int DontDotIfMoreEnemiesThan { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Gravity { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int GravityEnemies { get; set; }

        [Setting]
        [DefaultValue(30.0f)]
        public float MinimumManaPercentToDoDamage { get; set; }

        [Setting]
        [DefaultValue(20)]
        public int DoDamageIfTimeLeftLessThan { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool SmartAoe { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool AggroAst { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool Oracle { get; set; }

        [Setting]
        [DefaultValue(1)]
        public int OracleEnemies { get; set; }

        #endregion

        #region Buffs

        [Setting]
        [DefaultValue(true)]
        public bool Lightspeed { get; set; }

        [Setting]
        [DefaultValue(40f)]
        public float LightspeedHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool LightspeedWithDivination { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool LightspeedWithNeutralSect { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool LucidDreaming { get; set; }

        [Setting]
        [DefaultValue(80.0f)]
        public float LucidDreamingManaPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Exaltation { get; set; }

        [Setting]
        [DefaultValue(40f)]
        public float ExaltationHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Divination { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int DivinationAllies { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool NeutralSect { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float NeutralSectHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool SunSign { get; set; }

        #endregion

        #region Heals

        [Setting]
        [DefaultValue(2)]
        public int AoeNeedHealingLightParty { get; set; }

        [Setting]
        [DefaultValue(3)]
        public int AoeNeedHealingFullParty { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DisableSingleHealWhenNeedAoeHealing { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float AoEHealHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Synastry { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float SynastryHealthPercent { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int SynastryAmountOfPeople { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool SynastryTankOnly { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool EssentialDignity { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool EssentialDignityTankOnly { get; set; }

        [Setting]
        [DefaultValue(40.0f)]
        public float EssentialDignityHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Helios { get; set; }

        [Setting]
        [DefaultValue(60)]
        public float HeliosHealthPercent { get; set; }

        [Setting]
        [DefaultValue(20)]
        public float HeliosMinManaPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalHelios { get; set; }

        [Setting]
        [DefaultValue(80.0f)]
        public float DiurnalHeliosHealthPercent { get; set; }

        [Setting]
        [DefaultValue(30.0f)]
        public float DiurnalHeliosMinManaPercent { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DiurnalHeliosNoSwiftcast { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Horoscope { get; set; }

        [Setting]
        [DefaultValue(70.0f)]
        public float HoroscopeHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool CelestialOpposition { get; set; }

        [Setting]
        [DefaultValue(75)]
        public float CelestialOppositionHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalBenefic { get; set; }

        [Setting]
        [DefaultValue(40.0f)]
        public float DiurnalBeneficMinMana { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalBeneficOnTanks { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalBeneficOnHealers { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DiurnalBeneficOnDps { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DiurnalBeneficKeepUpOnTanks { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DiurnalBeneficKeepUpOnHealers { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DiurnalBeneficKeepUpOnDps { get; set; }

        [Setting]
        [DefaultValue(80.0f)]
        public float DiurnalBeneficHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalBeneficWhileMoving { get; set; }

        [Setting]
        [DefaultValue(40.0f)]
        public float DiurnalBeneficWhileMovingMinMana { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool DiurnalBeneficDontBeneficUnlessUnderTank { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalBeneficDontBeneficUnlessUnderHealer { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DiurnalBeneficDontBeneficUnlessUnderDps { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float DiurnalBeneficDontBeneficUnlessUnderHealth { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Benefic { get; set; }

        [Setting]
        [DefaultValue(55.0f)]
        public float BeneficHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Benefic2 { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool NoBeneficIfBenefic2Available { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float Benefic2HealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Benefic2AlwaysWithEnhancedBenefic2 { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool CelestialIntersection { get; set; }

        [Setting]
        [DefaultValue(90.0f)]
        public float CelestialIntersectionHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool CelestialIntersectionTankOnly { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool CollectiveUnconscious { get; set; }

        [Setting]
        [DefaultValue(4)]
        public int CollectiveUnconsciousAllies { get; set; }

        [Setting]
        [DefaultValue(70.0f)]
        public float CollectiveUnconsciousHealth { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool EarthlyStar { get; set; }

        [Setting]
        [DefaultValue(1)]
        public int EarthlyStarEnemiesNearTarget { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int EarthlyStarPartyMembersNearTarget { get; set; }

        [Setting]
        [DefaultValue(95f)]
        public float EarthlyStarPartyMembersNearTargetHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool StellarDetonation { get; set; }

        [Setting]
        [DefaultValue(4)]
        public int EarthlyDominanceCount { get; set; }

        [Setting]
        [DefaultValue(70f)]
        public float EarthlyDominanceHealthPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        public int GiantDominanceCount { get; set; }

        [Setting]
        [DefaultValue(60)]
        public float GiantDominanceHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Macrocosmos { get; set; }

        [Setting]
        [DefaultValue(65f)]
        public float MacrocosmosHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool WeaveOGCDHeals { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DontLetTheDRKDie { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogic_NeutralSectAspectedHelios { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogic_Exaltation { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogic_Macrocosmos { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogic_CollectiveUnconscious { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogic_Lightspeed { get; set; }

        #endregion

        #region Dispels

        [Setting]
        [DefaultValue(true)]
        public bool Dispel { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool DispelOnlyAbove { get; set; }

        [Setting]
        [DefaultValue(75.0f)]
        public float DispelOnlyAboveHealth { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool AutomaticallyDispelAnythingThatsDispellable { get; set; }


        #endregion

        #region AlliancesAndPets

        [Setting]
        [DefaultValue(false)]
        public bool IgnoreAlliance { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool HealAllianceHealers { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool HealAllianceTanks { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool HealAllianceDps { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool ResAllianceHealers { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool ResAllianceTanks { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool ResAllianceDps { get; set; }

        [Setting]
        [DefaultValue(false)]
        public bool HealAllianceOnlyBenefic { get; set; }

        #endregion

        #region Cards

        [Setting]
        [DefaultValue(true)]
        public bool AstralDraw { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UmbralDraw { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool UseMinorArcana { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Play { get; set; }

        [Setting]
        [DefaultValue(25)]
        public int DontPlayWhenCombatTimeIsLessThan { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool CardRuleDefaultToMinorArcana { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool LadyOfCrowns { get; set; }

        [Setting]
        [DefaultValue(80.0f)]
        public float LadyOfCrownsHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool LordOfCrowns { get; set; }

        [Setting]
        [DefaultValue(1)]
        public int LordOfCrownsEnemies { get; set; }
        #endregion

        #region Card Weights

        #region Tanks
        [Setting]
        [DefaultValue(1)]
        public int PldCardWeight { get; set; }
        [Setting]
        [DefaultValue(2)]
        public int WarCardWeight { get; set; }
        [Setting]
        [DefaultValue(3)]
        public int DrkCardWeight { get; set; }
        [Setting]
        [DefaultValue(4)]
        public int GnbCardWeight { get; set; }

        #endregion

        #region heals
        [Setting]
        [DefaultValue(5)]
        public int WhmCardWeight { get; set; }
        [Setting]
        [DefaultValue(6)]
        public int SchCardWeight { get; set; }
        [Setting]
        [DefaultValue(7)]
        public int AstCardWeight { get; set; }
        [Setting]
        [DefaultValue(8)]
        public int SgeCardWeight { get; set; }

        #endregion

        #region meleeDPS
        [Setting]
        [DefaultValue(9)]
        public int MnkCardWeight { get; set; }
        [Setting]
        [DefaultValue(10)]
        public int DrgCardWeight { get; set; }
        [Setting]
        [DefaultValue(11)]
        public int NinCardWeight { get; set; }
        [Setting]
        [DefaultValue(12)]
        public int SamCardWeight { get; set; }
        [Setting]
        [DefaultValue(13)]
        public int RprCardWeight { get; set; }
        [Setting]
        [DefaultValue(14)]
        public int VprCardWeight { get; set; }

        #endregion

        #region physicalRangeDPS
        [Setting]
        [DefaultValue(15)]
        public int BrdCardWeight { get; set; }
        [Setting]
        [DefaultValue(16)]
        public int MchCardWeight { get; set; }
        [Setting]
        [DefaultValue(17)]
        public int DncCardWeight { get; set; }


        #endregion

        #region magicalRangeDPS
        [Setting]
        [DefaultValue(18)]
        public int BlmCardWeight { get; set; }
        [Setting]
        [DefaultValue(19)]
        public int SmnCardWeight { get; set; }
        [Setting]
        [DefaultValue(20)]
        public int RdmCardWeight { get; set; }
        [Setting]
        [DefaultValue(21)]
        public int PctCardWeight { get; set; }
        [Setting]
        [DefaultValue(22)]
        public int BluCardWeight { get; set; }


        #endregion

        #endregion

        #region FightLogic
        [Setting]
        [DefaultValue(true)]
        public bool FightLogicNeutralSect { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogicEarthlyStar { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool EarthlyStarCenterParty { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogicCollectiveUnconscious { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool CollectiveUnconsciousCenterParty { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogicHoroscope { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogicAspectedHelios { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogicCelestialIntersection { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool FightLogicExaltation { get; set; }
        #endregion

        #region PVP
        [Setting]
        [DefaultValue(true)]
        public bool Pvp_FallMalefic { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_AspectedBenefic { get; set; }

        [Setting]
        [DefaultValue(50.0f)]
        public float Pvp_AspectedBeneficHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_GravityII { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int Pvp_GravityIIEnemies { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_DoubleCast { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_Macrocosmos { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_Microcosmos { get; set; }

        [Setting]
        [DefaultValue(60.0f)]
        public float Pvp_MicrocosmosHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_MinorArcana { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_Oracle { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_CelestialRiver { get; set; }

        [Setting]
        [DefaultValue(1)]
        public int Pvp_CelestialRiverNearbyAllies { get; set; }

        [Setting]
        [DefaultValue(85.0f)]
        public float Pvp_CelestialRiverHealthPercent { get; set; }

        [Setting]
        [DefaultValue(3)]
        public int Pvp_MacrocosmosEnemies { get; set; }

        [Setting]
        [DefaultValue(2)]
        public int Pvp_LordOfCrownsEnemies { get; set; }
        #endregion

    }
}
