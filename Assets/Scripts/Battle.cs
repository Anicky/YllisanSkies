
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
            initTexts();
            displayInterface();
            displayHeroes();
            displayEnemies();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void initTexts()
        {
            commandsTitles = new Dictionary<Commands, string>();
            commandsTitles.Add(Commands.Attack, game.getTranslation("Battles", "Attack"));
            commandsTitles.Add(Commands.Defense, game.getTranslation("Battles", "Defense"));
            commandsTitles.Add(Commands.Abilities, game.getTranslation("Battles", "Abilities"));
            commandsTitles.Add(Commands.Items, game.getTranslation("Battles", "Items"));
            commandsTitles.Add(Commands.RunAway, game.getTranslation("Battles", "Run away"));
            GameObject.Find("Canvas/Block_ATBBar/Text_Wait").GetComponent<Text>().text = game.getTranslation("Battles", "WAIT");
            GameObject.Find("Canvas/Block_ATBBar/Text_Command").GetComponent<Text>().text = game.getTranslation("Battles", "COM");
            GameObject.Find("Canvas/Block_ATBBar/Text_Action").GetComponent<Text>().text = game.getTranslation("Battles", "ACT");
        }

        private void displayBlockCommands(bool enabled)
        {
            GameObject block = GameObject.Find("Canvas/Block_Commands");
            Image blockRootImage = block.GetComponent<Image>();
            blockRootImage.enabled = false;
            Text[] blockTexts = block.GetComponentsInChildren<Text>();
            RawImage[] rawImages = block.GetComponentsInChildren<RawImage>();
            foreach (Text blockText in blockTexts)
            {
                blockText.enabled = enabled;
            }
            foreach (RawImage rawImage in rawImages)
            {
                rawImage.enabled = enabled;
            }
        }

        private void displayInterface()
        {
            displayBlockCommands(false);
            displayHeroesBlocks();
            displayATBBar();
        }

        private void displayHeroesBlocks()
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

            GameObject block = GameObject.Find("Canvas/Block_Hero" + heroPosition);
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
                        rawImage.texture = Resources.Load<Texture>("UI/Battles/Battles_Interface_Hero_" + hero.getId());
                    }
                }
            }
        }

        private void displayATBBar()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                handleATBHero(i + 1, game.heroesTeam.getHeroByIndex(i));
            }
            for (int i = 0; i < EnemiesTeam.MAXIMUM_NUMBER_OF_ENEMIES; i++)
            {
                handleATBEnemy(i + 1, game.enemiesTeam.getEnemyByIndex(i));
            }
        }

        private void handleATBHero(int heroPosition, Hero hero)
        {
            bool enabled = false;
            if (hero != null)
            {
                enabled = true;
            }

            GameObject block = GameObject.Find("Canvas/Block_ATBBar/ATB_Hero" + heroPosition);
            Image blockRootImage = block.GetComponent<Image>();
            blockRootImage.enabled = enabled;
            RawImage[] rawImages = block.GetComponentsInChildren<RawImage>();
            foreach (RawImage rawImage in rawImages)
            {
                rawImage.enabled = enabled;
                if (enabled)
                {
                    if (rawImage.name == "Hero_Sprite")
                    {
                        rawImage.texture = Resources.Load<Texture>("UI/Battles/Battles_Interface_ATB_Sprite_Hero_" + hero.getId());
                    }
                }
            }
        }

        private void handleATBEnemy(int enemyPosition, Enemy enemy)
        {
            bool enabled = false;
            if (enemy != null)
            {
                enabled = true;
            }

            GameObject block = GameObject.Find("Canvas/Block_ATBBar/ATB_Enemy" + enemyPosition);
            Image blockRootImage = block.GetComponent<Image>();
            blockRootImage.enabled = enabled;
            RawImage[] rawImages = block.GetComponentsInChildren<RawImage>();
            Text[] texts = block.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                text.enabled = enabled;
                if (enabled)
                {
                    if (text.name == "Enemy_Number")
                    {
                        text.text = enemyPosition.ToString();
                    }
                }
            }
            foreach (RawImage rawImage in rawImages)
            {
                rawImage.enabled = enabled;
                if (enabled)
                {
                    if (rawImage.name == "Enemy_Sprite")
                    {
                        rawImage.texture = Resources.Load<Texture>("UI/Battles/Battles_Interface_ATB_Sprite_Enemy_" + enemy.getId());
                    }
                }
            }
        }

        private void displayHeroes()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                displayHero(i + 1, game.heroesTeam.getHeroByIndex(i));
            }
        }

        private void displayHero(int heroPosition, Hero hero)
        {
            bool enabled = false;
            if (hero != null)
            {
                enabled = true;
            }
            GameObject heroObject = GameObject.Find("Hero" + heroPosition);
            heroObject.GetComponent<SpriteRenderer>().enabled = enabled;
            // @TODO : change sprite according to hero
        }

        private void displayEnemies()
        {
            for (int i = 0; i < EnemiesTeam.MAXIMUM_NUMBER_OF_ENEMIES; i++)
            {
                displayEnemy(i + 1, game.enemiesTeam.getEnemyByIndex(i));
            }
        }

        private void displayEnemy(int heroPosition, Enemy enemy)
        {
            bool enabled = false;
            if (enemy != null)
            {
                enabled = true;
            }
            GameObject enemyObject = GameObject.Find("Enemy" + heroPosition);
            enemyObject.GetComponent<SpriteRenderer>().enabled = enabled;
            // @TODO : change sprite according to enemy
        }
    }
}