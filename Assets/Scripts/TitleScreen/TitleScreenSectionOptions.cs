using UnityEngine;

namespace RaverSoft.YllisanSkies.TitleScreen
{
    public class TitleScreenSectionOptions : TitleScreenSection
    {
        public override void open()
        {
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