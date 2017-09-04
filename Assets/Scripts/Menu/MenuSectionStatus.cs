using UnityEngine;
using RaverSoft.YllisanSkies.Characters;
using UnityEngine.UI;

namespace RaverSoft.YllisanSkies.Menu
{
    public class MenuSectionStatus : MenuSection
    {
        public Hero currentHero;

        public void open(Hero currentHero)
        {
            this.currentHero = currentHero;
            displayHero();
            base.open();
        }

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                quitSection();
            }
        }

        public void displayHero()
        {
            GameObject.Find("Menu/Status/Block_Lv/Lv_Stats").GetComponent<Text>().text = currentHero.getLv().ToString();
            GameObject.Find("Menu/Status/Block_NextLv/NextLv_Stats").GetComponent<Text>().text = currentHero.getXpToNextLv().ToString();
        }
    }
}