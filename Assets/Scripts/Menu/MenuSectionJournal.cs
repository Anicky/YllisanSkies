﻿using UnityEngine;

namespace RaverSoft.YllisanSkies.Menu
{
    public class MenuSectionJournal : MenuSection
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