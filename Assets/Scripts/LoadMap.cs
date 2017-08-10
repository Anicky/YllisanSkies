using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadMap : MonoBehaviour {

    public string mapToLoad;
    public string startingPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            var thePlayer = FindObjectOfType<Player>();
            thePlayer.eventNameWherePlayerHasToBeTeleported = startingPoint;
            StartCoroutine(fadeOut());
        }
    }

    private IEnumerator fadeIn()
    {
        GameObject game = GameObject.Find("Game");
        Animation anim = game.GetComponent<Animation>();
        anim.Play("Overlay_FadeIn");
        do
        {
            yield return null;
        } while (anim.isPlaying);
    }

    private IEnumerator fadeOut()
    {
        GameObject game = GameObject.Find("Game");
        Animation anim = game.GetComponent<Animation>();
        anim.Play("Overlay_FadeOut");
        do
        {
            yield return null;
        } while (anim.isPlaying);
        SceneManager.LoadScene(mapToLoad);
        StartCoroutine(fadeIn());
    }

}
