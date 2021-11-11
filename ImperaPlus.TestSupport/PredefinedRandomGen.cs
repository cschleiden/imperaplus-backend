using ImperaPlus.Domain.Services;

namespace ImperaPlus.TestSupport
{
    public class PredefinedRandomGen : IAttackRandomGen
    {
        private int attackerDice;
        private readonly int defenderDice;

        public PredefinedRandomGen(int attackerDice, int defenderDice)
        {
            this.attackerDice = attackerDice;
            this.defenderDice = defenderDice;
        }

        public int GetAttackerDice()
        {
            return attackerDice;
        }

        public int GetDefenderDice()
        {
            return defenderDice;
        }
    }

    public class AttackerWinsRandomGen : IAttackRandomGen
    {
        public int GetAttackerDice()
        {
            return AttackConfiguration.DiceMax;
        }

        public int GetDefenderDice()
        {
            return AttackConfiguration.DiceMin;
        }
    }

    public class DefenderWinsRandomGen : IAttackRandomGen
    {
        public int GetAttackerDice()
        {
            return AttackConfiguration.DiceMin;
        }

        public int GetDefenderDice()
        {
            return AttackConfiguration.DiceMax;
        }
    }
}
