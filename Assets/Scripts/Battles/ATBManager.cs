using System.Collections.Generic;
using RaverSoft.YllisanSkies.Characters;

namespace RaverSoft.YllisanSkies.Battles
{
    public class ATBManager
    {
        public BattleSystem battle;
        private int numberOfCharacters = 0;
        private float agilityAverage = 0;
        private Dictionary<int, SpeedSlice> speedSlices;

        // Adjusting parameters
        private const int NUMBER_OF_SPEED_SLICES = 21;
        private const int AGILITY_MIN = 0;
        private const int AGILITY_MAX = 999999;

        private class SpeedSlice
        {
            public float min;
            public float max;

            public SpeedSlice(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
        }

        public ATBManager(BattleSystem battle)
        {
            this.battle = battle;
        }

        public void init()
        {
            numberOfCharacters = getNumberOfCharacters();
            agilityAverage = getAgilityAverage();
            speedSlices = getSpeedSlices();
            setHeroesSpeeds();
            setEnemiesSpeeds();
        }

        private int getNumberOfCharacters()
        {
            return battle.game.heroesTeam.getNumberOfHeroes() + battle.game.enemiesTeam.getNumberOfEnemies();
        }

        private int getAgilityAverage()
        {
            int agilityAverage = 0;
            foreach (Hero hero in battle.game.heroesTeam.getHeroes())
            {
                agilityAverage += hero.getAgility();
            }
            foreach (Enemy enemy in battle.game.enemiesTeam.getEnemies())
            {
                agilityAverage += enemy.getAgility();
            }
            return agilityAverage / numberOfCharacters;
        }

        private float getPercentagePerSlice()
        {
            return 100 / ((NUMBER_OF_SPEED_SLICES - 1) / 2); ;
        }

        private Dictionary<int, SpeedSlice> getSpeedSlices()
        {
            float percentagePerSlice = getPercentagePerSlice();
            Dictionary<int, SpeedSlice> speedSlices = new Dictionary<int, SpeedSlice>();
            for (int i = 0; i < NUMBER_OF_SPEED_SLICES; i++)
            {
                float min = AGILITY_MIN;
                float max = AGILITY_MAX;
                if (i > 0)
                {
                    min = ((i * percentagePerSlice) / 100) * agilityAverage;
                }
                if (i < NUMBER_OF_SPEED_SLICES - 1)
                {
                    max = (((i + 1) * percentagePerSlice) / 100) * agilityAverage;
                }
                speedSlices.Add(i, new SpeedSlice(min, max));
            }
            return speedSlices;
        }

        private int getSpeedForCharacter(Character character)
        {
            int battleSpeed = 0;
            for (int i = 0; i < NUMBER_OF_SPEED_SLICES; i++)
            {
                if ((character.getAgility() >= speedSlices[i].min) && (character.getAgility() < speedSlices[i].max))
                {
                    battleSpeed = i;
                    break;
                }
            }
            return battleSpeed;
        }

        private void setHeroesSpeeds()
        {
            foreach (Hero hero in battle.game.heroesTeam.getHeroes())
            {
                hero.battleSpeed = getSpeedForCharacter(hero);
            }
        }

        private void setEnemiesSpeeds()
        {
            foreach (Enemy enemy in battle.game.enemiesTeam.getEnemies())
            {
                enemy.battleSpeed = getSpeedForCharacter(enemy);
            }
        }
    }
}