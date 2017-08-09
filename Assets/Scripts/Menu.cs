using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public bool isOpened = false;
    private Canvas canvas;
    public Game game;

    // Use this for initialization
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void open()
    {
        showHeroesStats();
        StartCoroutine(openMenu());
    }

    public void close()
    {
        StartCoroutine(closeMenu());
    }

    private void showHeroesStats()
    {
        for(int i = 0; i < game.heroes.Length; i++)
        {
            Hero hero = game.heroes[i];
            handleHeroBlock(i+1, hero);
        }
    }

    private void handleHeroBlock(int heroPosition, Hero hero)
    {
        bool enabled = false;
        if (hero != null)
        {
            enabled = true;
        }

        GameObject block = GameObject.Find("Menu/Block_Hero" + heroPosition);
        Image blockImage = block.GetComponent<Image>();
        blockImage.enabled = enabled;
        Text[] blockStats = block.GetComponentsInChildren<Text>();
        Image[] gauges = block.GetComponentsInChildren<Image>();
        foreach (Text blockStat in blockStats)
        {
            blockStat.enabled = enabled;
            if (enabled)
            {
                if (blockStat.name == "Lv_Stats")
                {
                    blockStat.text = hero.lv.ToString();
                } else if (blockStat.name == "Hp_Stats")
                {
                    blockStat.text = hero.hp.ToString() + "/" + hero.hpMax.ToString();
                } else if (blockStat.name == "Ap_Stats")
                {
                    blockStat.text = hero.ap.ToString() + "/" + hero.apMax.ToString();
                }
            }
        }
        foreach (Image image in gauges)
        {
            image.enabled = enabled;
            if (enabled)
            {
                if (image.name == "Hp_Gauge")
                {
                    image.fillAmount = (float) hero.hp / hero.hpMax;
                } else if (image.name == "Ap_Gauge")
                {
                    image.fillAmount = (float) hero.ap / hero.apMax;
                }
            }
        }
    }

    IEnumerator openMenu()
    {
        game.player.disableMovement();
        canvas.enabled = true;
        Animation anim = GetComponent<Animation>();
        anim.Play("Menu_Open");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        isOpened = true;
    }

    IEnumerator closeMenu()
    {
        Animation anim = GetComponent<Animation>();
        anim.Play("Menu_Close");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        isOpened = false;
        canvas.enabled = false;
        game.player.enableMovement();
    }

}
