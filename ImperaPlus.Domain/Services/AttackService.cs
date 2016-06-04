using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Services
{
    public static class AttackConfiguration
    {
        public const int AttackerDice = 3;
        public const int DefenderDice = 2;

        public const int DiceMin = 1;
        public const int DiceMax = 6;
    }

    public interface IAttackRandomGen
    {
        int GetAttackerDice();
        int GetDefenderDice();
    }

    public class AttackerRandomGen : IAttackRandomGen
    {
        private IRandomGen randomGen;

        public AttackerRandomGen(IRandomGen randomGen)
        {
            this.randomGen = randomGen;
        }

        public int GetAttackerDice()
        {
            return this.randomGen.GetNext(AttackConfiguration.DiceMin, AttackConfiguration.DiceMax);
        }

        public int GetDefenderDice()
        {
            return this.randomGen.GetNext(AttackConfiguration.DiceMin, AttackConfiguration.DiceMax);
        }
    }

    public interface IAttackService
    {
        bool Attack(int unitsAttacker, int unitsDefender, out int unitsAttackerLost, out int unitsDefenderLost);
    }

    public class AttackService : IAttackService
    {       
        private IAttackRandomGen randomGen;

        public AttackService(IAttackRandomGen randomGen)
        {
            this.randomGen = randomGen;
        }

        public bool Attack(int unitsAttacker, int unitsDefender, out int unitsAttackerLost, out int unitsDefenderLost)
        {
            int orgUnitsAttacker = unitsAttacker;
            int orgUnitsDefender = unitsDefender;

            while (unitsAttacker > 0 && unitsDefender > 0)
            {
                var attackerDice = Enumerable
                    .Range(0, Math.Min(unitsAttacker, AttackConfiguration.AttackerDice))
                    .Select(x => this.randomGen.GetAttackerDice())
                    .OrderByDescending(x => x)
                    .ToArray();

                var defenderDice = Enumerable
                    .Range(0, Math.Min(unitsDefender, AttackConfiguration.DefenderDice))
                    .Select(x => this.randomGen.GetDefenderDice())
                    .OrderByDescending(x => x)
                    .ToArray();

                for (int i = 0; i < Math.Min(attackerDice.Length, defenderDice.Length); ++i)    
                {
                    if (defenderDice[i] >= attackerDice[i])
                    {
                        --unitsAttacker;
                    }
                    else
                    {
                        --unitsDefender;
                    }
                }
            }

            if (unitsAttacker == 0)
            {
                // Attacker has failed
                unitsAttackerLost = orgUnitsAttacker;
                unitsDefenderLost = orgUnitsDefender - unitsDefender;

                return false;
            }
            else
            {
                // Attacker has succeeded
                unitsAttackerLost = orgUnitsAttacker - unitsAttacker;
                unitsDefenderLost = orgUnitsDefender;

                return true;
            }
        }
    }    
}
