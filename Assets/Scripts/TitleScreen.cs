using RaverSoft.YllisanSkies.Sound;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RaverSoft.YllisanSkies
{
    public class TitleScreen : MonoBehaviour
    {
        private SaveSystem saveSystem;
        private SoundManager soundManager;
        private Animation anim;

        private bool hasGameSaves;
        private int currentSectionIndex = 0;
        private int previousSectionIndex = 0;
        private const int PIXELS_TO_MOVE_FOR_SELECTED_SECTION = 64;
        private const int PIXELS_TO_MOVE_IN_ONE_UPDATE = 4;
        private float pixelsToMove = 0;
        private bool inTransition = true;

        private enum Sections
        {
            NewGame,
            Continue,
            Options,
            Bonus,
            Quit
        }

        // Use this for initialization
        void Start()
        {
            saveSystem = GetComponent<SaveSystem>();
            soundManager = GetComponent<SoundManager>();
            anim = GetComponent<Animation>();
            hasGameSaves = saveSystem.hasGameSaves();
            displaySectionAvailability("Continue", hasGameSaves);
            soundManager.fadeOut(1.5f);
            StartCoroutine(playAnimation("FadeOut"));
        }

        // Update is called once per frame
        void Update()
        {
            if (!inTransition)
            {
                if (pixelsToMove > 0)
                {
                    displaySectionMoving();
                }
                else
                {
                    if (Input.GetAxisRaw("Vertical") != 0)
                    {
                        soundManager.playSound(Sounds.Cursor);
                        moveSection();
                    }
                    else if (Input.GetButtonDown("Submit"))
                    {
                        submitSection();
                    }
                }
            }
        }

        private IEnumerator playAnimation(string animationName)
        {
            inTransition = true;
            anim.Play(animationName);
            do
            {
                yield return null;
            } while (anim.isPlaying);
            inTransition = false;
        }

        private void submitSection()
        {
            switch (currentSectionIndex)
            {
                case (int)Sections.NewGame:
                    // @TODO
                    break;
                case (int)Sections.Continue:
                    if (hasGameSaves)
                    {
                        soundManager.playSound(Sounds.Submit);
                        // @TODO
                    }
                    else
                    {
                        soundManager.playSound(Sounds.Error);
                    }
                    break;
                case (int)Sections.Options:
                    // @TODO
                    break;
                case (int)Sections.Bonus:
                    // @TODO
                    break;
                case (int)Sections.Quit:
                    StartCoroutine(quit());
                    break;
            }
        }

        private int getNumberOfSections()
        {
            return Enum.GetNames(typeof(Sections)).Length;
        }

        private void displaySectionAvailability(string sectionName, bool isAvailable)
        {
            Color saveColor = Color.white;
            if (!isAvailable)
            {
                saveColor = Color.gray;
            }
            GameObject.Find("Canvas/Sections/" + sectionName + "/Section_Title").GetComponent<Text>().color = saveColor;
        }

        private void moveSection()
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                currentSectionIndex++;
                if (currentSectionIndex >= getNumberOfSections())
                {
                    currentSectionIndex = 0;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                currentSectionIndex--;
                if (currentSectionIndex < 0)
                {
                    currentSectionIndex = getNumberOfSections() - 1;
                }
            }
            pixelsToMove = PIXELS_TO_MOVE_FOR_SELECTED_SECTION;
            previousSectionIndex = getPreviousSectionIndex();
        }

        private int getPreviousSectionIndex()
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                previousSectionIndex = currentSectionIndex - 1;
                if (currentSectionIndex == 0)
                {
                    previousSectionIndex = getNumberOfSections() - 1;
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                previousSectionIndex = currentSectionIndex + 1;
                if (currentSectionIndex == getNumberOfSections() - 1)
                {
                    previousSectionIndex = 0;
                }
            }
            return previousSectionIndex;
        }

        private void displaySectionMoving()
        {
            GameObject currentSection = GameObject.Find("Canvas/Sections/" + (Sections)currentSectionIndex);
            GameObject previousSection = GameObject.Find("Canvas/Sections/" + (Sections)previousSectionIndex);
            currentSection.transform.localPosition = new Vector2(currentSection.transform.localPosition.x - PIXELS_TO_MOVE_IN_ONE_UPDATE, currentSection.transform.localPosition.y);
            previousSection.transform.localPosition = new Vector2(previousSection.transform.localPosition.x + PIXELS_TO_MOVE_IN_ONE_UPDATE, previousSection.transform.localPosition.y);
            pixelsToMove -= PIXELS_TO_MOVE_IN_ONE_UPDATE;
        }

        private IEnumerator quit()
        {
            soundManager.fadeIn(1.5f);
            soundManager.playSound(Sounds.Submit);
            yield return playAnimation("FadeIn");
            Application.Quit();
        }
    }
}