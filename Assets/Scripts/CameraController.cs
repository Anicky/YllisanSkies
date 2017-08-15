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
    private Tiled2Unity.TiledMap tiledMap;
    private static bool cameraExists;
    public bool isSceneChanging = false;

    private float getRelativeX()
    {
        return target.position.x - tiledMap.transform.position.x;
    }

    private float getRelativeY()
    {
        return target.position.y - tiledMap.transform.position.y;
    }

    // Use this for initialization
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        GameObject map = GameObject.Find("Map");
        tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject map = GameObject.Find("Map");
        tiledMap = map.GetComponentInParent<Tiled2Unity.TiledMap>();
        isSceneChanging = true;
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
            if (!alwaysCenteredToTarget)
            {
                float cameraHeight = 2f * mainCamera.orthographicSize;
                float cameraWidth = cameraHeight * mainCamera.aspect;
                float cameraLeftLimitToFollowTarget = (cameraWidth / 2);
                float cameraRightLimitToFollowTarget = tiledMap.GetMapWidthInPixelsScaled() - (cameraWidth / 2);
                float cameraUpLimitToFollowTarget = -(cameraHeight / 2);
                float cameraDownLimitToFollowTarget = -tiledMap.GetMapHeightInPixelsScaled() + (cameraHeight / 2);
                if (getRelativeX() < cameraLeftLimitToFollowTarget)
                {
                    toX = tiledMap.transform.position.x + cameraLeftLimitToFollowTarget;
                }
                else if (getRelativeX() > cameraRightLimitToFollowTarget)
                {
                    toX = tiledMap.transform.position.x + cameraRightLimitToFollowTarget;
                }
                if (getRelativeY() > cameraUpLimitToFollowTarget)
                {
                    toY = tiledMap.transform.position.y + cameraUpLimitToFollowTarget;
                }
                else if (getRelativeY() < cameraDownLimitToFollowTarget)
                {
                    toY = tiledMap.transform.position.y + cameraDownLimitToFollowTarget;
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
