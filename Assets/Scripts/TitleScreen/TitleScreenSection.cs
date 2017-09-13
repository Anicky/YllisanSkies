using UnityEngine;

namespace RaverSoft.YllisanSkies.TitleScreen
{
    public class TitleScreenSection : MonoBehaviour
    {
        public TitleScreenSystem titleScreen;

        protected bool isOpened = false;

        public virtual void open()
        {
            isOpened = true;
        }

        protected virtual void quitSection()
        {
            if (isOpened)
            {
                isOpened = false;
                titleScreen.quitSection();
            }
        }
    }
}