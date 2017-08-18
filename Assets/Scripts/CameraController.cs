using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float speed = 0.1f;
    private Camera mainCamera;
    private float mapWidth;
    private float mapHeight;
    public bool alwaysCenteredToTarget = false;
    private Tiled2Unity.TiledMap map;
    private static bool cameraExists;
    public bool isSceneChanging = false;

    private float getRelativeX()
    {
        return target.position.x - map.transform.position.x;
    }

    private float getRelativeY()
    {
        return target.position.y - map.transform.position.y;
    }

    // Use this for initialization
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void initialize()
    {
        this.map = GameObject.Find("Map").GetComponentInParent<Tiled2Unity.TiledMap>();
        isSceneChanging = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initialize();
    }

    // Update is called once per frame
    private void Update()
    {
        if(isSceneChanging)
        {
            moveCamera(1);
            isSceneChanging = false;
        } else
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
            if (!alwaysCenteredToTarget && map)
            {
                float cameraHeight = 2f * mainCamera.orthographicSize;
                float cameraWidth = cameraHeight * mainCamera.aspect;
                float cameraLeftLimitToFollowTarget = (cameraWidth / 2);
                float cameraRightLimitToFollowTarget = map.GetMapWidthInPixelsScaled() - (cameraWidth / 2);
                float cameraUpLimitToFollowTarget = -(cameraHeight / 2);
                float cameraDownLimitToFollowTarget = -map.GetMapHeightInPixelsScaled() + (cameraHeight / 2);
                if (getRelativeX() < cameraLeftLimitToFollowTarget)
                {
                    toX = map.transform.position.x + cameraLeftLimitToFollowTarget;
                }
                else if (getRelativeX() > cameraRightLimitToFollowTarget)
                {
                    toX = map.transform.position.x + cameraRightLimitToFollowTarget;
                }
                if (getRelativeY() > cameraUpLimitToFollowTarget)
                {
                    toY = map.transform.position.y + cameraUpLimitToFollowTarget;
                }
                else if (getRelativeY() < cameraDownLimitToFollowTarget)
                {
                    toY = map.transform.position.y + cameraDownLimitToFollowTarget;
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
