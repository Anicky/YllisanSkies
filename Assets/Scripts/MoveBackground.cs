using UnityEngine;
using UnityEngine.SceneManagement;
using RaverSoft.YllisanSkies.Utils;

namespace RaverSoft.YllisanSkies
{
    public class MoveBackground : MonoBehaviour
    {
        // Game
        private Game game;

        // Map
        private Tiled2Unity.TiledMap map;

        // Object components
        private Renderer backgroundRenderer;

        // Attributes
        public float scrollSpeedX;
        public float scrollSpeedY;

        // Utils
        private int frameCounter;

        // Use this for initialization
        void Start()
        {
            game = GameObject.Find("Game").GetComponent<Game>();
            SceneManager.activeSceneChanged += OnSceneChange;
            backgroundRenderer = GetComponent<Renderer>();
            initialize(SceneManager.GetActiveScene().GetRootGameObjects());
        }

        private void initialize(GameObject[] gameObjects)
        {
            GameObject mapObject = GameObjectUtils.searchByNameInList(gameObjects, "Map");
            if (mapObject && backgroundRenderer)
            {
                map = mapObject.GetComponent<Tiled2Unity.TiledMap>();
                float ratio = map.GetMapWidthInPixelsScaled() / backgroundRenderer.material.mainTexture.width;
                backgroundRenderer.material.mainTextureScale = new Vector2(ratio, ratio);
            }
        }

        private void OnSceneChange(Scene previousScene, Scene currentScene)
        {
            initialize(currentScene.GetRootGameObjects());
        }

        // Update is called once per frame
        void Update()
        {
            if (!game.stopEvents && map)
            {
                backgroundRenderer.material.mainTextureOffset = new Vector2(frameCounter * scrollSpeedX * Time.deltaTime, frameCounter * scrollSpeedY * Time.deltaTime);
                frameCounter++;
            }
        }
    }
}