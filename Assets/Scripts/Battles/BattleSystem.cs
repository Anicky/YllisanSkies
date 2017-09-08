using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RaverSoft.YllisanSkies.Characters;
using RaverSoft.YllisanSkies.Sound;
using RaverSoft.YllisanSkies.Utils;

namespace RaverSoft.YllisanSkies.Battles
{
    public class BattleSystem : MonoBehaviour
    {
        public enum Commands
        {
            Attack,
            Defense,
            Abilities,
            Items,
            RunAway
        }

        public enum BattleStates
        {
            Wait,
            Command,
            Action
        }

        public Game game;
        private ATBManager aTBManager;
        private Dictionary<Commands, string> commandsTitles;
        private bool battleInitialized = false;
        private bool stopATB = false;
        private Character currentCharacterAtCommand = null;
        private int currentCommandPosition = 0;
        private int numberOfCommands;
        private bool isAxisInUse = false;
        private bool inPause = false;

        // Use this for initialization
        void Start()
        {
            game = GameObject.Find("Game").GetComponent<Game>();
            game.heroesTeam.initBattle();
            aTBManager = new ATBManager(this);
            initTexts();
            aTBManager.init();
            displayInterface();
            displayHeroes();
            displayEnemies();
            numberOfCommands = Enum.GetValues(typeof(Commands)).Length;
            initSpriteAnimations();
            battleInitialized = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (battleInitialized)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    // @TODO : handle pause
                }
                if ((currentCharacterAtCommand != null) && (!inPause))
                {
                    if (Input.GetAxisRaw("Horizontal") != 0)
                    {
                        if (!isAxisInUse)
                        {
                            moveCommandPosition();
                            isAxisInUse = true;
                        }
                    }
                    else if (Input.GetButtonDown("Submit"))
                    {
                        handleCommand();
                    }
                    if (Input.GetAxisRaw("Horizontal") == 0)
                    {
                        isAxisInUse = false;
                    }
                }
                if (!stopATB)
                {
                    // @TODO : resolve equalities by comparing agility
                    foreach (Character character in aTBManager.charactersSortedByPosition)
                    {
                        if ((character.currentBattleState == BattleStates.Wait) && (character.currentBattlePosition >= aTBManager.POSITIONS_ELEMENTS[BattleStates.Command]))
                        {
                            characterAtCommandPoint(character);
                            break;
                        }
                        else if ((character.currentBattleState == BattleStates.Command) && (character.currentBattlePosition >= aTBManager.POSITIONS_ELEMENTS[BattleStates.Action]))
                        {
                            characterAtActionPoint(character);
                            break;
                        }
                    }
                    if (!stopATB)
                    {
                        aTBManager.changeCharactersPositions();
                    }
                    displayATBBar();
                    if (!stopATB)
                    {
                        changeATBCharactersIndex();
                    }
                }
            }
        }

        private void initSpriteAnimations()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                Hero hero = (Hero)game.heroesTeam.getCharacterByIndex(i);
                if (hero != null)
                {
                    Animator animator = GameObject.Find("Sprites/Hero" + (i + 1)).GetComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load("Animations/Characters/Heroes/" + hero.id + "/Hero_" + hero.id + "_BattleController") as RuntimeAnimatorController;
                    animator.Play(AnimationUtils.getCurrentAnimationNameFromAnimator(animator), 0, (i * 0.1f));
                }
            }
            for (int i = 0; i < EnemiesTeam.MAXIMUM_NUMBER_OF_ENEMIES; i++)
            {
                Enemy enemy = (Enemy)game.enemiesTeam.getCharacterByIndex(i);
                if (enemy != null)
                {
                    Animator animator = GameObject.Find("Sprites/Enemy" + (i + 1)).GetComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load("Animations/Characters/Enemies/" + enemy.id + "/Enemy_" + enemy.id + "_BattleController") as RuntimeAnimatorController;
                    animator.Play(AnimationUtils.getCurrentAnimationNameFromAnimator(animator), 0, (i * 0.1f));
                }
            }
        }

        private void handleCommand()
        {
            switch (currentCommandPosition)
            {
                case (int)Commands.Attack:
                    // @TODO
                    break;
                case (int)Commands.Defense:
                    game.playSound(Sounds.Submit);
                    currentCharacterAtCommand.currentBattleCommand = new BattleDefenseCommand();
                    characterCommandFinished();
                    break;
                case (int)Commands.Abilities:
                    // @TODO
                    break;
                case (int)Commands.Items:
                    // @TODO
                    break;
                case (int)Commands.RunAway:
                    // @TODO
                    break;
            }
        }

        private void characterCommandFinished()
        {
            if (currentCharacterAtCommand is Hero)
            {
                hideCommands();
            }
            currentCharacterAtCommand = null;
            stopATB = false;
        }

        private void characterAtCommandPoint(Character character)
        {
            character.currentBattleState = BattleStates.Command;
            currentCharacterAtCommand = character;
            stopATB = true;
            if (character is Hero)
            {
                Hero hero = (Hero)character;
                hero.initBattleCommand();
                displayHeroesBlocks();
                displayCommands();
            }
            else if (character is Enemy)
            {
                // @TODO : create AI for enemy
                characterCommandFinished();
            }
        }

        private void characterAtActionPoint(Character character)
        {
            character.currentBattleState = BattleStates.Action;
            stopATB = true;
            // @TODO : Do action
            characterActionFinished(character);
        }

        private void characterActionFinished(Character character)
        {
            character.currentBattleState = BattleStates.Wait;
            character.currentBattlePosition = aTBManager.POSITIONS_ELEMENTS[BattleStates.Wait];
            stopATB = false;
        }

        private void displayCommands()
        {
            currentCommandPosition = 0;
            displayBlockCommands(true);
        }

        private void hideCommands()
        {
            displayBlockCommands(false);
        }

        private void moveCommandPosition()
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (currentCommandPosition > 0)
                {
                    game.playSound(Sounds.Cursor);
                    currentCommandPosition--;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (currentCommandPosition < numberOfCommands - 1)
                {
                    game.playSound(Sounds.Cursor);
                    currentCommandPosition++;
                }
            }
            displayCommandInfo();
        }

        private void changeATBCharactersIndex()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                Hero hero = (Hero)game.heroesTeam.getCharacterByIndex(i);
                if (hero != null)
                {
                    int index = aTBManager.charactersSortedByPosition.IndexOf(hero);
                    GameObject.Find("Canvas/Block_ATBBar/ATB_Characters/Hero" + (i + 1)).transform.SetSiblingIndex(index);
                }
            }
            for (int i = 0; i < EnemiesTeam.MAXIMUM_NUMBER_OF_ENEMIES; i++)
            {
                Enemy enemy = (Enemy)game.enemiesTeam.getCharacterByIndex(i);
                if (enemy != null)
                {
                    int index = aTBManager.charactersSortedByPosition.IndexOf(enemy);
                    GameObject.Find("Canvas/Block_ATBBar/ATB_Characters/Enemy" + (i + 1)).transform.SetSiblingIndex(index);
                }
            }
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
            blockRootImage.enabled = enabled;
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
            if (enabled)
            {
                displayCommandInfo();
            }
        }

        private void displayCommandInfo()
        {
            Commands[] commands = (Commands[])Enum.GetValues(typeof(Commands));
            foreach (Commands command in commands)
            {
                GameObject.Find("Canvas/Block_Commands/Icon_" + command).GetComponent<RawImage>().texture = getCommandImage(command);
            }
            GameObject.Find("Canvas/Block_Commands/Command_Title").GetComponent<Text>().text = commandsTitles[((Commands)currentCommandPosition)];
            bool cursorLeftEnabled = false;
            bool cursorRightEnabled = false;
            if (currentCommandPosition > 0)
            {
                cursorLeftEnabled = true;
            }
            if (currentCommandPosition < numberOfCommands - 1)
            {
                cursorRightEnabled = true;
            }
            GameObject.Find("Canvas/Block_Commands/Cursor_Left").GetComponent<RawImage>().enabled = cursorLeftEnabled;
            GameObject.Find("Canvas/Block_Commands/Cursor_Right").GetComponent<RawImage>().enabled = cursorRightEnabled;
        }

        private Texture getCommandImage(Commands command)
        {
            string textureName = "UI/Battles/Battles_Interface_Commands_" + command;
            if (currentCommandPosition == (int)command)
            {
                textureName += "_Selected";
            }
            return Resources.Load(textureName) as Texture;
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
                handleHeroBlock(i + 1, (Hero)game.heroesTeam.getCharacterByIndex(i));
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
                        blockText.text = hero.hp.ToString() + "/" + hero.hpMax.ToString();
                    }
                    else if (blockText.name == "Ap_Stats")
                    {
                        blockText.text = hero.ap.ToString() + "/" + hero.apMax.ToString();
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
                        blockImage.fillAmount = (float)hero.hp / hero.hpMax;
                    }
                    else if (blockImage.name == "Ap_Gauge")
                    {
                        blockImage.fillAmount = (float)hero.ap / hero.apMax;
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
                handleATBHero(i + 1, (Hero)game.heroesTeam.getCharacterByIndex(i));
            }
            for (int i = 0; i < EnemiesTeam.MAXIMUM_NUMBER_OF_ENEMIES; i++)
            {
                handleATBEnemy(i + 1, (Enemy)game.enemiesTeam.getCharacterByIndex(i));
            }
        }

        private void handleATBHero(int heroPosition, Hero hero)
        {
            GameObject block = GameObject.Find("Canvas/Block_ATBBar/ATB_Characters/Hero" + heroPosition);
            bool enabled = false;
            if (hero != null)
            {
                RectTransform blockRectTransform = block.GetComponent<RectTransform>();
                blockRectTransform.anchoredPosition = new Vector2(hero.currentBattlePosition, blockRectTransform.anchoredPosition.y);
                enabled = true;
            }
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
            GameObject block = GameObject.Find("Canvas/Block_ATBBar/ATB_Characters/Enemy" + enemyPosition);
            bool enabled = false;
            if (enemy != null)
            {
                RectTransform blockRectTransform = block.GetComponent<RectTransform>();
                blockRectTransform.anchoredPosition = new Vector2(enemy.currentBattlePosition, blockRectTransform.anchoredPosition.y);
                enabled = true;
            }
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
                        rawImage.texture = Resources.Load<Texture>("UI/Battles/Battles_Interface_ATB_Sprite_Enemy_" + enemy.id);
                    }
                }
            }
        }

        private void displayHeroes()
        {
            for (int i = 0; i < HeroesTeam.MAXIMUM_NUMBER_OF_HEROES; i++)
            {
                displayHero(i + 1, (Hero)game.heroesTeam.getCharacterByIndex(i));
            }
        }

        private void displayHero(int heroPosition, Hero hero)
        {
            bool enabled = false;
            if (hero != null)
            {
                enabled = true;
            }
            GameObject heroObject = GameObject.Find("Sprites/Hero" + heroPosition);
            heroObject.GetComponent<SpriteRenderer>().enabled = enabled;
            // @TODO : change sprite according to hero
        }

        private void displayEnemies()
        {
            for (int i = 0; i < EnemiesTeam.MAXIMUM_NUMBER_OF_ENEMIES; i++)
            {
                displayEnemy(i + 1, (Enemy)game.enemiesTeam.getCharacterByIndex(i));
            }
        }

        private void displayEnemy(int heroPosition, Enemy enemy)
        {
            bool enabled = false;
            if (enemy != null)
            {
                enabled = true;
            }
            GameObject enemyObject = GameObject.Find("Sprites/Enemy" + heroPosition);
            enemyObject.GetComponent<SpriteRenderer>().enabled = enabled;
            // @TODO : change sprite according to enemy
        }
    }
}