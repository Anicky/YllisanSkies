using RaverSoft.YllisanSkies.Characters;
using System;

namespace RaverSoft.YllisanSkies
{
    [Serializable]
    public class SaveData
    {
        public HeroesTeam heroesTeam;
        public string scene;
        public float playerX;
        public float playerY;

        public SaveData(HeroesTeam heroesTeam, string scene, Player player)
        {
            this.heroesTeam = heroesTeam;
            this.scene = scene;
            playerX = player.transform.position.x;
            playerY = player.transform.position.y;
        }
    }
}