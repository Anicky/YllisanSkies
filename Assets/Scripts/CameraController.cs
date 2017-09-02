using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Game game;
    public Transform target;
    public float speed = 0.1f;
    private Camera mainCamera;
    public bool alwaysCenteredToTarget = false;
    public bool isSceneChanging = false;
    private bool inMap = false;
    private Vector3 mapPosition;
    private float mapWidth;
    private float mapHeight;

    private float getRelativeX()
    {
        return target.position.x - mapPosition.x;
    }

    private float getRelativeY()
    {
        return target.position.y - mapPosition.y;
    }

    // Use this for initialization
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        game = GameObject.Find("Game").GetComponent<Game>();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void initialize(GameObject[] gameObjects)
    {
        inMap = false;
        if (game.inBattle)
        {
            target = null;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.name == "CameraTarget")
                {
                    target = gameObject.transform;
                }
                else if (gameObject.name == "Background")
                {
                    inMap = true;
                    mapPosition = gameObject.transform.position;
                    Vector2 mapDimensions = gameObject.GetComponent<SpriteRenderer>().bounds.size;
                    mapWidth = mapDimensions.x;
                    mapHeight = mapDimensions.y;
                }
            }
        }
        else
        {
            target = GameObject.Find("Player").transform;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.name == "Map")
                {
                    inMap = true;
                    Tiled2Unity.TiledMap map = gameObject.GetComponent<Tiled2Unity.TiledMap>();
                    mapPosition = map.transform.position;
                    mapWidth = map.GetMapWidthInPixelsScaled();
                    mapHeight = map.GetMapHeightInPixelsScaled();
                    break;
                }
            }
        }
        isSceneChanging = true;
    }

    private void OnSceneChange(Scene previousScene, Scene currentScene)
    {
        initialize(currentScene.GetRootGameObjects());
    }

    // Update is called once per frame
    private void Update()
    {
        if (isSceneChanging)
        {
            moveCamera(1);
            isSceneChanging = false;
        }
        else
        {
            moveCamera(speed);
        }
    }

    private Vector3 getTargetPosition()
    {
        Vector3 targetPosition = new Vector3();
        if (target)
        {
            float toX = target.position.x;
            float toY = target.position.y;
            if (!alwaysCenteredToTarget && inMap)
            {
                float cameraHeight = 2f * mainCamera.orthographicSize;
                float cameraWidth = cameraHeight * mainCamera.aspect;
                float cameraLeftLimitToFollowTarget = (cameraWidth / 2);
                float cameraRightLimitToFollowTarget = mapWidth - (cameraWidth / 2);
                float cameraUpLimitToFollowTarget = -(cameraHeight / 2);
                float cameraDownLimitToFollowTarget = -mapHeight + (cameraHeight / 2);
                if (getRelativeX() < cameraLeftLimitToFollowTarget)
                {
                    toX = mapPosition.x + cameraLeftLimitToFollowTarget;
                }
                else if (getRelativeX() > cameraRightLimitToFollowTarget)
                {
                    toX = mapPosition.x + cameraRightLimitToFollowTarget;
                }
                if (getRelativeY() > cameraUpLimitToFollowTarget)
                {
                    toY = mapPosition.y + cameraUpLimitToFollowTarget;
                }
                else if (getRelativeY() < cameraDownLimitToFollowTarget)
                {
                    toY = mapPosition.y + cameraDownLimitToFollowTarget;
                }
            }
            targetPosition = new Vector3(toX, toY, transform.position.z);
        }
        return targetPosition;
    }

    public void moveCamera(float movementSpeed)
    {
        if (target)
        {
            Vector3 targetPosition = getTargetPosition();
            transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                targetPosition,
                movementSpeed
            );
        }
    }

}
