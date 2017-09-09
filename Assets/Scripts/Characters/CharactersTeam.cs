using System.Collections.Generic;
using System;

namespace RaverSoft.YllisanSkies.Characters
{
    [Serializable]
    public class CharactersTeam
    {
        protected List<Character> characters;
        private int maximumNumberOfCharacters = 0;

        public CharactersTeam(int maximumNumberOfCharacters = 0)
        {
            characters = new List<Character>();
            this.maximumNumberOfCharacters = maximumNumberOfCharacters;
        }

        public List<Character> getCharacters()
        {
            return characters;
        }

        public int getNumberOfCharacters()
        {
            return characters.Count;
        }

        public Character getCharacterByIndex(int i)
        {
            Character character = null;
            if (i >= 0 && i < characters.Count)
            {
                character = characters[i];
            }
            return character;
        }

        public void addCharacter(Character character)
        {
            if ((maximumNumberOfCharacters == 0) || (maximumNumberOfCharacters > 0 && characters.Count < maximumNumberOfCharacters))
            {
                characters.Add(character);
            }
        }
    }
}