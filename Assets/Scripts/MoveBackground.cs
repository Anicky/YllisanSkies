﻿using UnityEngine;

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
            backgroundRenderer = GetComponent<Renderer>();
            map = GameObject.Find("Map").GetComponent<Tiled2Unity.TiledMap>();
            float ratio = map.GetMapWidthInPixelsScaled() / backgroundRenderer.material.mainTexture.width;
            backgroundRenderer.material.mainTextureScale = new Vector2(ratio, ratio);
        }

        // Update is called once per frame
        void Update()
        {
            if (!game.stopEvents)
            {
                backgroundRenderer.material.mainTextureOffset = new Vector2(frameCounter * scrollSpeedX * Time.deltaTime, frameCounter * scrollSpeedY * Time.deltaTime);
                frameCounter++;
            }
        }
    }
}