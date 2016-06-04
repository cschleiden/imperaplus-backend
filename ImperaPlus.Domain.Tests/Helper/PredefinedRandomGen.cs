using ImperaPlus.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Tests.Helper
{
    public class PredefinedRandomGen : IAttackRandomGen
    {
        private int attackerDice;
        private int defenderDice;

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
