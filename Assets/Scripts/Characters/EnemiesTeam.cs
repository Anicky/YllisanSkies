using System.Collections.Generic;

namespace RaverSoft.YllisanSkies.Characters
{
    public class EnemiesTeam
    {
        private List<Enemy> enemies;

        public const int MAXIMUM_NUMBER_OF_ENEMIES = 4;

        public EnemiesTeam()
        {
            enemies = new List<Enemy>();
        }

        public List<Enemy> getEnemies()
        {
            return enemies;
        }

        public int getNumberOfEnemies()
        {
            return enemies.Count;
        }

        public Enemy getEnemyByIndex(int i)
        {
            Enemy enemy = null;
            if (i >= 0 && i < enemies.Count)
            {
                enemy = enemies[i];
            }
            return enemy;
        }

        public void addEnemy(Enemy enemy)
        {
            if (enemies.Count < MAXIMUM_NUMBER_OF_ENEMIES)
            {
                enemies.Add(enemy);
            }
        }

    }
}