using Magitek.Models;

namespace Magitek.Utilities.CombatMessages
{
    internal class OverrideCombatMessageStrategy : ICombatMessageStrategy
    {
        public int Priority { get; }
        public string Message => CombatMessageOverride.Message;
        public string ImageSource => CombatMessageOverride.ImageSource;

        public OverrideCombatMessageStrategy(int priority)
        {
            Priority = priority;
        }

        public bool ShowMessage()
        {
            return CombatMessageOverride.Active;
        }
    }
}