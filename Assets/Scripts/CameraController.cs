using UnityEngine;
using UnityEngine.SceneManagement;
using RaverSoft.YllisanSkies.Utils;

namespace RaverSoft.YllisanSkies
{
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
            if (game.currentBattle != null)
            {
                target = null;
                GameObject targetObject = GameObjectUtils.searchByNameInList(gameObjects, "CameraTarget");
                if (targetObject)
                {
                    target = targetObject.transform;
                }
                GameObject mapObject = GameObjectUtils.searchByNameInList(gameObjects, "Background");
                if (mapObject)
                {
                    inMap = true;
                    mapPosition = mapObject.transform.position;
                    Vector2 mapDimensions = mapObject.GetComponent<SpriteRenderer>().bounds.size;
                    mapWidth = mapDimensions.x;
                    mapHeight = mapDimensions.y;
                }
            }
            else
            {
                target = GameObject.Find("Player").transform;
                GameObject mapObject = GameObjectUtils.searchByNameInList(gameObjects, "Map");
                if (mapObject)
                {
                    inMap = true;
                    Tiled2Unity.TiledMap map = mapObject.GetComponent<Tiled2Unity.TiledMap>();
                    mapPosition = map.transform.position;
                    mapWidth = map.GetMapWidthInPixelsScaled();
                    mapHeight = map.GetMapHeightInPixelsScaled();
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
}