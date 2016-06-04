using ImperaPlus.Domain.Services;
namespace ImperaPlus.TestSupport
{
    public class PredefinedRandomGen : IAttackRandomGen
    {
        int attackerDice;
        readonly int defenderDice;

        public PredefinedRandomGen(int attackerDice, int defenderDice)
        {
            this.attackerDice = attackerDice;
            this.defenderDice = defenderDice;
        }

        public int GetAttackerDice()
        {
            return this.attackerDice;
        }

        public int GetDefenderDice()
        {
            return this.defenderDice;
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
