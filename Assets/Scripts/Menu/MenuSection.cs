using UnityEngine;

namespace RaverSoft.YllisanSkies.Menu
{
    public class MenuSection : MonoBehaviour
    {
        public MenuSystem menu;

        protected bool isOpened = false;

        public virtual void open()
        {
            isOpened = true;
        }

        protected void quitSection()
        {
            if (isOpened)
            {
                isOpened = false;
                menu.quitSection();
            }
        }
    }
}