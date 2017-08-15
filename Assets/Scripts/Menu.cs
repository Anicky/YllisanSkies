using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Menu : MonoBehaviour
{

    public bool isOpened = false;
    private Canvas canvas;
    public Game game;
    public bool inTransition = false;
    private int currentSectionIndex = 1;
    private int numberOfSections = 9;

    // Use this for initialization
    private void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isOpened && !inTransition)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    currentSectionIndex++;
                    if (currentSectionIndex > numberOfSections)
                    {
                        currentSectionIndex = 1;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    currentSectionIndex--;
                    if (currentSectionIndex < 1)
                    {
                        currentSectionIndex = numberOfSections;
                    }

                }
                StartCoroutine(changeSection(Input.GetAxisRaw("Vertical")));
            }
        }
    }

    public void open()
    {
        currentSectionIndex = 1;
        showHeroesStats();
        StartCoroutine(openMenu());
    }

    public void close()
    {
        StartCoroutine(closeMenu());
    }

    private void showHeroesStats()
    {
        for (int i = 0; i < game.heroes.Length; i++)
        {
            Hero hero = game.heroes[i];
            handleHeroBlock(i + 1, hero);
        }
    }

    private void handleHeroBlock(int heroPosition, Hero hero)
    {
        bool enabled = false;
        if (hero != null)
        {
            enabled = true;
        }

        GameObject block = GameObject.Find("Menu/Main/Block_Hero" + heroPosition);
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
                else if (blockText.name == "Lv_Stats")
                {
                    blockText.text = hero.lv.ToString();
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
            }
        }
        foreach (RawImage rawImage in rawImages)
        {
            rawImage.enabled = enabled;
            if (enabled)
            {
                if (rawImage.name == "Hero_Sprite")
                {
                    rawImage.texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/UI/Sprite_Hero_" + hero.name + ".png", typeof(Texture2D));
                }
            }
        }
    }

    private IEnumerator openMenu()
    {
        inTransition = true;
        game.player.disableMovement();
        GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = true;
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
        anim.Play("Menu_Open");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        isOpened = true;
        inTransition = false;
    }

    private IEnumerator closeMenu()
    {
        inTransition = true;
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
        anim.Play("Menu_Close");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        isOpened = false;
        GameObject.Find("Menu/Main").GetComponent<Canvas>().enabled = false;
        game.player.enableMovement();
        inTransition = false;
    }

    private IEnumerator changeSection(float axe)
    {
        inTransition = true;
        int previousSectionIndex = 0;
        if (axe < 0)
        {
            previousSectionIndex = currentSectionIndex - 1;
            if (currentSectionIndex == 1)
            {
                previousSectionIndex = numberOfSections;
            }
        }
        else if (axe > 0)
        {
            previousSectionIndex = currentSectionIndex + 1;
            if (currentSectionIndex == numberOfSections)
            {
                previousSectionIndex = 1;
            }
        }
        Animation anim = GameObject.Find("Menu/Main").GetComponent<Animation>();
        anim.Play("Menu_Section_Current_0" + currentSectionIndex);
        anim["Menu_Section_Previous_0" + previousSectionIndex].layer = 1;
        anim.Play("Menu_Section_Previous_0" + previousSectionIndex);
        do
        {
            yield return null;
        } while (anim.isPlaying);
        inTransition = false;
    }

}
