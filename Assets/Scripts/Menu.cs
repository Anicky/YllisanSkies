using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool isOpened = false;
    private Canvas canvas;

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
        StartCoroutine(openMenu());
    }

    public void close()
    {
        StartCoroutine(closeMenu());
    }

    IEnumerator openMenu()
    {
        var thePlayer = FindObjectOfType<PlayerController>();
        thePlayer.disableMovement();
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
        var thePlayer = FindObjectOfType<PlayerController>();
        thePlayer.enableMovement();
    }

}
