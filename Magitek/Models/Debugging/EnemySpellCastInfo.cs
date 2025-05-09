﻿using Magitek.Commands;
using Magitek.Extensions;
using Magitek.Models.WebResources;
using Magitek.ViewModels;
using PropertyChanged;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Magitek.Models.Debugging
{
    [AddINotifyPropertyChangedInterface]
    public class EnemySpellCastInfo
    {
        public EnemySpellCastInfo(string name, uint id, string castedBy, uint castedById, ushort zoneId, string zoneName)
        {
            Name = name;
            Id = id;
            CastedBy = castedBy;
            CastedById = castedById;
            ZoneId = zoneId;
            ZoneName = zoneName;
            Icon = this.GetIcon();
            InFightLogicBuilderAOE = "[+] FightLogic AOE";
            InFightLogicBuilderTB = "[+] FightLogic TB";
            InFightLogicBuilderKnockback = "[+] FightLogic KB";
        }

        public string Name { get; set; }
        public uint Id { get; set; }
        public string CastedBy { get; set; }
        public uint CastedById { get; set; }
        public ushort ZoneId { get; set; }
        public string ZoneName { get; set; }
        public double Icon { get; set; }

        public string IconUrl
        {
            get
            {
                var folder = (Math.Floor(Icon / 1000) * 1000).ToString(CultureInfo.InvariantCulture).Trim().PadLeft(6, '0');
                var image = Icon.ToString(CultureInfo.InvariantCulture).Trim().PadLeft(6, '0');
                return $@"https://secure.xivdb.com/img/game/{folder}/{image}.png";
            }
        }

        public string InFightLogicBuilderAOE { get; set; }
        public string InFightLogicBuilderTB { get; set; }
        public string InFightLogicBuilderKnockback { get; set; }

        public ICommand AddToFightLogicBuilderAOE => new DelegateCommand<EnemySpellCastInfo>(info =>
        {
            if (info == null)
                return;

            if (Debug.Instance.FightLogicBuilderAOE.Any(r => r.Id == info.Id))
            {
                Debug.Instance.FightLogicBuilderAOE.Remove(info);
                InFightLogicBuilderAOE = "[+] FightLogic AOE";
            }
            else
            {
                Debug.Instance.FightLogicBuilderAOE.Add(info);
                InFightLogicBuilderAOE = "[-] FightLogic AOE";
            }
        });

        public ICommand AddToFightLogicBuilderTB => new DelegateCommand<EnemySpellCastInfo>(info =>
        {
            if (info == null)
                return;

            if (Debug.Instance.FightLogicBuilderTB.Any(r => r.Id == info.Id))
            {
                Debug.Instance.FightLogicBuilderTB.Remove(info);
                InFightLogicBuilderTB = "[+] FightLogic TB";
            }
            else
            {
                Debug.Instance.FightLogicBuilderTB.Add(info);
                InFightLogicBuilderTB = "[-] FightLogic TB";
            }
        });

        public ICommand AddToFightLogicBuilderKnockback => new DelegateCommand<EnemySpellCastInfo>(info =>
        {
            if (info == null)
                return;

            if (Debug.Instance.FightLogicBuilderKnockbacks.Any(r => r.Id == info.Id))
            {
                Debug.Instance.FightLogicBuilderKnockbacks.Remove(info);
                InFightLogicBuilderKnockback = "[+] FightLogic KB";
            }
            else
            {
                Debug.Instance.FightLogicBuilderKnockbacks.Add(info);
                InFightLogicBuilderKnockback = "[-] FightLogic KB";
            }
        });

        public ICommand AddToInterruptsAndStuns => new DelegateCommand<EnemySpellCastInfo>(info =>
        {
            if (info == null)
                return;

            if (InterruptsAndStuns.Instance.ActionList.Any(r => r.Id == info.Id))
                return;

            var newXivItem = new XivDbItem()
            {
                Id = info.Id,
                Name = info.Name,
                Icon = info.Icon,
                Stun = true,
                Interrupt = true,
                Scholar = true,
                BlueMage = true,
                Bard = true,
                Paladin = true,
                Warrior = true,
                DarkKnight = true,
                Machinist = true,
                Dragoon = true,
                Monk = true,
                Ninja = true
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                InterruptsAndStuns.Instance.ActionList.Add(newXivItem);
            });
        });
    }
}
