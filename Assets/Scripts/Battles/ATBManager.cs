using System.Collections.Generic;
using RaverSoft.YllisanSkies.Characters;

namespace RaverSoft.YllisanSkies.Battles
{
    public class ATBManager
    {
        public BattleSystem battle;
        private int numberOfCharacters = 0;
        private float agilityAverage = 0;
        private List<SpeedSlice> speedSlices;

        // Adjusting parameters
        private const int NUMBER_OF_SPEED_SLICES = 21;
        private const int AGILITY_MIN = 0;
        private const int AGILITY_MAX = 999999;
        private const int GAP_BETWEEN_STARTING_POSITIONS = 16;
        public Dictionary<BattleSystem.BattleStates, int> POSITIONS_ELEMENTS = new Dictionary<BattleSystem.BattleStates, int>()
        {
            { BattleSystem.BattleStates.Wait, 0 },
            { BattleSystem.BattleStates.Command, 934 },
            { BattleSystem.BattleStates.Action, 1300 }
        };
        private int[] adjustedStartingPositionsForSpeedSlice = new int[] { -2, 2, -6, 6, -4, 4, -8, 8 };

        private class SpeedSlice
        {
            public float min;
            public float max;
            public int speed;
            public int startingPosition;
            public List<Character> characters;

            public SpeedSlice(float min, float max, int speed, int startingPosition)
            {
                this.min = min;
                this.max = max;
                this.speed = speed;
                this.startingPosition = startingPosition;
                characters = new List<Character>();
            }

            public void addCharacter(Character character)
            {
                characters.Add(character);
            }

            public void shuffleCharacters()
            {
                List<Character> randomList = new List<Character>();
                System.Random random = new System.Random();
                int randomIndex = 0;
                while (characters.Count > 0)
                {
                    randomIndex = random.Next(0, characters.Count);
                    randomList.Add(characters[randomIndex]);
                    characters.RemoveAt(randomIndex);
                }
                characters = randomList;
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
            initCharacters();
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

        private List<SpeedSlice> getSpeedSlices()
        {
            float percentagePerSlice = getPercentagePerSlice();
            List<SpeedSlice> speedSlices = new List<SpeedSlice>();
            for (int i = 1; i <= NUMBER_OF_SPEED_SLICES; i++)
            {
                float min = AGILITY_MIN;
                float max = AGILITY_MAX;
                if (i > 1)
                {
                    min = (((i - 1) * percentagePerSlice) / 100) * agilityAverage;
                }
                if (i < NUMBER_OF_SPEED_SLICES)
                {
                    max = ((i * percentagePerSlice) / 100) * agilityAverage;
                }
                int startingPosition = POSITIONS_ELEMENTS[BattleSystem.BattleStates.Wait] + (i * GAP_BETWEEN_STARTING_POSITIONS);
                speedSlices.Add(new SpeedSlice(min, max, i, startingPosition));
            }
            return speedSlices;
        }

        private int getSpeedForCharacter(Character character)
        {
            int battleSpeed = 1;
            foreach (SpeedSlice speedSlice in speedSlices)
            {
                if ((character.agility >= speedSlice.min) && (character.agility < speedSlice.max))
                {
                    battleSpeed = speedSlice.speed;
                    speedSlice.addCharacter(character);
                    break;
                }
            }
            return battleSpeed;
        }

        private void initCharacters()
        {
            foreach (Hero hero in battle.game.heroesTeam.getHeroes())
            {
                hero.currentBattleSpeed = getSpeedForCharacter(hero);
            }
            foreach (Enemy enemy in battle.game.enemiesTeam.getEnemies())
            {
                enemy.currentBattleSpeed = getSpeedForCharacter(enemy);
            }
            foreach (SpeedSlice speedSlice in speedSlices)
            {
                speedSlice.shuffleCharacters();
                int i = 0;
                bool multipleCharactersInThisSpeedSlice = false;
                if (speedSlice.characters.Count > 0)
                {
                    multipleCharactersInThisSpeedSlice = true;
                }
                foreach (Character character in speedSlice.characters)
                {
                    int characterPosition = speedSlice.startingPosition;
                    if (multipleCharactersInThisSpeedSlice)
                    {
                        characterPosition += adjustedStartingPositionsForSpeedSlice[i];
                        i++;
                    }
                    character.currentBattlePosition = characterPosition;
                }
            }
        }
    }
}