using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMap : MonoBehaviour {

    public Object mapToLoad;
    public int startingPoint;

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
            Debug.Log(startingPoint.transform.position);
            //SceneManager.LoadScene(ma)
            Application.LoadLevel(mapToLoad.name);
            var thePlayer = FindObjectOfType<PlayerController>();
            Debug.Log(thePlayer.transform.position);
            thePlayer.transform.position = startingPoint.transform.position;
            //thePlayer.lastMove = startingDirection;
            /*var theCamera = FindObjectOfType<CameraController>();
            theCamera.transform.position = new Vector3(startingPoint.transform.position.x, startingPoint.transform.position.y, theCamera.transform.position.z);*/
        }
    }
}
