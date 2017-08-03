using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMap : MonoBehaviour {

    public Object mapToLoad;
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
            var thePlayer = FindObjectOfType<PlayerController>();
            thePlayer.eventNameWherePlayerHasToBeTeleported = startingPoint;
            SceneManager.LoadScene(mapToLoad.name);
        }
    }
}
