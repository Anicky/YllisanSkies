
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RaverSoft.YllisanSkies.Characters;

namespace RaverSoft.YllisanSkies
{
    public class Battle : MonoBehaviour
    {

        public enum Commands
        {
            Attack,
            Defense,
            Abilities,
            Items,
            RunAway
        }

        private Game game;
        private Dictionary<Commands, string> commandsTitles;

        // Use this for initialization
        void Start()
        {
            game = GameObject.Find("Game").GetComponent<Game>();
            game.heroesTeam.initBattle();
            commandsTitles = new Dictionary<Commands, string>();
            commandsTitles.Add(Commands.Attack, game.getTranslation("Battles", "Attack"));
            commandsTitles.Add(Commands.Defense, game.getTranslation("Battles", "Defense"));
            commandsTitles.Add(Commands.Abilities, game.getTranslation("Battles", "Abilities"));
            commandsTitles.Add(Commands.Items, game.getTranslation("Battles", "Items"));
            commandsTitles.Add(Commands.RunAway, game.getTranslation("Battles", "Run away"));
            GameObject.Find("Battle/Block_ATBBar/Text_Wait").GetComponent<Text>().text = game.getTranslation("Battles", "WAIT");
            GameObject.Find("Battle/Block_ATBBar/Text_Command").GetComponent<Text>().text = game.getTranslation("Battles", "COM");
            GameObject.Find("Battle/Block_ATBBar/Text_Action").GetComponent<Text>().text = game.getTranslation("Battles", "ACT");
            displayInterface();
            displayHeroes();
            displayEnemies();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void displayInterface()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                handleHeroBlock(i + 1, game.heroesTeam.getHeroByIndex(i));
            }
        }

        private void handleHeroBlock(int heroPosition, Hero hero)
        {
            bool enabled = false;
            if (hero != null)
            {
                enabled = true;
            }

            GameObject block = GameObject.Find("Battle/Block_Hero" + heroPosition);
            Image blockRootImage = block.GetComponent<Image>();
            blockRootImage.enabled = enabled;
            Text[] blockTexts = block.GetComponentsInChildren<Text>();
            Image[] blockImages = block.GetComponentsInChildren<Image>();
            RawImage[] rawImages = block.GetComponentsInChildren<RawImage>();
            foreach (Text blockText in blockTexts)
            {
                blockText.enabled = enabled;
                if (enabled)
                {
                    if (blockText.name == "Hero_Name")
                    {
                        blockText.text = hero.name;
                    }
                    else if (blockText.name == "Hp_Stats")
                    {
                        blockText.text = hero.getHp().ToString() + "/" + hero.getHpMax().ToString();
                    }
                    else if (blockText.name == "Ap_Stats")
                    {
                        blockText.text = hero.getAp().ToString() + "/" + hero.getApMax().ToString();
                    }
                }
            }
            foreach (Image blockImage in blockImages)
            {
                blockImage.enabled = enabled;
                if (enabled)
                {
                    if (blockImage.name == "Hp_Gauge")
                    {
                        blockImage.fillAmount = (float)hero.getHp() / hero.getHpMax();
                    }
                    else if (blockImage.name == "Ap_Gauge")
                    {
                        blockImage.fillAmount = (float)hero.getAp() / hero.getApMax();
                    }
                    else if (blockImage.name == "AttackBar")
                    {
                        blockImage.fillAmount = (float)hero.getAttackPoints() / Hero.BATTLE_MAX_ATTACK_POINTS;
                    }
                }
            }
            foreach (RawImage rawImage in rawImages)
            {
                rawImage.enabled = enabled;
                if (enabled)
                {
                    if (rawImage.name == "Hero_Sprite")
                    {
                        rawImage.texture = Resources.Load<Texture>("UI/Battles/Battles_Interface_Hero_" + hero.name);
                    }
                }
            }
        }

        private void displayHeroes()
        {
            // @TODO
        }

        private void displayEnemies()
        {
            // @TODO
        }
    }
}