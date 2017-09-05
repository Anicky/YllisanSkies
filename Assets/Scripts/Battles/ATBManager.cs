using System.Collections.Generic;
using RaverSoft.YllisanSkies.Characters;
using System;
using UnityEngine;

namespace RaverSoft.YllisanSkies.Battles
{
    public class ATBManager
    {
        public BattleSystem battle;
        private int numberOfCharacters = 0;
        private float agilityAverage = 0;
        private Dictionary<int, SpeedSlice> speedSlices;
        private List<Character> charactersByAgility;
        private System.Random ra = new System.Random();

        // Adjusting parameters
        private const int NUMBER_OF_SPEED_SLICES = 21;
        private const int AGILITY_MIN = 0;
        private const int AGILITY_MAX = 999999;
        public Dictionary<BattleSystem.BattleStates, int> POSITIONS_ELEMENTS = new Dictionary<BattleSystem.BattleStates, int>()
        {
            { BattleSystem.BattleStates.Wait, 54 },
            { BattleSystem.BattleStates.Command, 986 },
            { BattleSystem.BattleStates.Action, 1352}
        };

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
            charactersByAgility = sortCharactersByAgility();
            initHeroes();
            initEnemies();
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
                agilityAverage += hero.agility;
            }
            foreach (Enemy enemy in battle.game.enemiesTeam.getEnemies())
            {
                agilityAverage += enemy.agility;
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
                if ((character.agility >= speedSlices[i].min) && (character.agility < speedSlices[i].max))
                {
                    battleSpeed = i;
                    break;
                }
            }
            return battleSpeed;
        }

        private List<Character> sortCharactersByAgility()
        {
            List<Character> charactersByAgility = new List<Character>();
            foreach (Hero hero in battle.game.heroesTeam.getHeroes())
            {
                charactersByAgility.Add(hero);
            }
            foreach (Enemy enemy in battle.game.enemiesTeam.getEnemies())
            {
                charactersByAgility.Add(enemy);
            }
            charactersByAgility.Sort(delegate (Character a, Character b)
            {
                return b.agility - a.agility;
            });
            return charactersByAgility;
        }

        private int getStartPositionForCharacter(Character character, int battleSpeed)
        {
            int startPosition = 0;
            for (int i = 0; i < charactersByAgility.Count; i++)
            {
                if (charactersByAgility[i] == character)
                {
                    int r = ra.Next(-10, 10);
                    startPosition = POSITIONS_ELEMENTS[BattleSystem.BattleStates.Wait] + (battleSpeed * 16) + r;
                    break;
                }
            }
            return startPosition;
        }

        private void initHeroes()
        {
            foreach (Hero hero in battle.game.heroesTeam.getHeroes())
            {
                int battleSpeed = getSpeedForCharacter(hero);
                int startPosition = getStartPositionForCharacter(hero, battleSpeed);
                hero.initBattle(battleSpeed, startPosition);
            }
        }

        private void initEnemies()
        {
            foreach (Enemy enemy in battle.game.enemiesTeam.getEnemies())
            {
                int battleSpeed = getSpeedForCharacter(enemy);
                int startPosition = getStartPositionForCharacter(enemy, battleSpeed);
                enemy.initBattle(battleSpeed, startPosition);
            }
        }
    }
}