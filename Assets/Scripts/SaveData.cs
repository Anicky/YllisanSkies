using RaverSoft.YllisanSkies.Characters;
using System;

namespace RaverSoft.YllisanSkies
{
    [Serializable]
    public class SaveData
    {
        public HeroesTeam heroesTeam { get; private set; }
        public string scene { get; private set; }
        public float playerX { get; private set; }
        public float playerY { get; private set; }
        public bool menuEnabled { get; private set; }

        public SaveData(HeroesTeam heroesTeam, string scene, Player player, bool menuEnabled)
        {
            this.heroesTeam = heroesTeam;
            this.scene = scene;
            playerX = player.transform.position.x;
            playerY = player.transform.position.y;
            this.menuEnabled = menuEnabled;
        }
    }
}