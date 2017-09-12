using RaverSoft.YllisanSkies.Characters;
using System;
using UnityEngine;

namespace RaverSoft.YllisanSkies
{
    [Serializable]
    public class SaveData
    {
        public HeroesTeam heroesTeam { get; private set; }
        public string scene { get; private set; }
        private float playerPositionX;
        private float playerPositionY;
        private float playerDirectionX;
        private float playerDirectionY;
        public bool menuEnabled { get; private set; }

        public SaveData(HeroesTeam heroesTeam, string scene, Player player, bool menuEnabled)
        {
            this.heroesTeam = heroesTeam;
            this.scene = scene;
            playerPositionX = player.transform.position.x;
            playerPositionY = player.transform.position.y;
            playerDirectionX = player.lastMove.x;
            playerDirectionX = player.lastMove.y;
            this.menuEnabled = menuEnabled;
        }

        public Vector2 getPlayerPosition()
        {
            return new Vector2(playerPositionX, playerPositionY);
        }

        public Vector2 getPlayerDirection()
        {
            return new Vector2(playerDirectionX, playerDirectionY);
        }
    }
}