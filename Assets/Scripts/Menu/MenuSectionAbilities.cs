using UnityEngine;
using RaverSoft.YllisanSkies.Characters;

namespace RaverSoft.YllisanSkies.Menu
{
    public class MenuSectionAbilities : MenuSection
    {
        public Hero currentHero;

        public void open(Hero currentHero)
        {
            this.currentHero = currentHero;
            // @TODO
            base.open();
        }

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                quitSection();
            }
        }
    }
}