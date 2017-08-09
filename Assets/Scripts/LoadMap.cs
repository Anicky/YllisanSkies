using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(mapToLoad);
        }
    }
}
