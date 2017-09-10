using UnityEngine;
using System.Collections;

namespace RaverSoft.YllisanSkies.Sound
{
    public enum Sounds
    {
        Cancel,
        Submit,
        Cursor,
        Error
    }

    public class SoundManager : MonoBehaviour
    {

        private enum FadesTypes
        {
            In,
            Out
        }

        private AudioSource audioSource;

        private AudioClip cursor;
        private AudioClip submit;
        private AudioClip error;
        private AudioClip cancel;

        public void Start()
        {
            audioSource = GetComponent<AudioSource>();
            cursor = Resources.Load("Sounds/Menu_Cursor") as AudioClip;
            submit = Resources.Load("Sounds/Menu_Submit") as AudioClip;
            error = Resources.Load("Sounds/Menu_Error") as AudioClip;
            cancel = Resources.Load("Sounds/Menu_Cancel") as AudioClip;
        }

        public void playSound(Sounds sound)
        {
            audioSource.PlayOneShot(getAudioClipBySoundName(sound));
        }

        private AudioClip getAudioClipBySoundName(Sounds soundName)
        {
            AudioClip audioClip = null;
            switch (soundName)
            {
                case Sounds.Cancel:
                    audioClip = cancel;
                    break;
                case Sounds.Submit:
                    audioClip = submit;
                    break;
                case Sounds.Cursor:
                    audioClip = cursor;
                    break;
                case Sounds.Error:
                    audioClip = error;
                    break;
            }
            return audioClip;
        }

        public void fadeIn(float seconds = 5, float minVolume = 0, float maxVolume = 1)
        {
            StartCoroutine(fade(seconds, FadesTypes.In, minVolume, maxVolume));
        }

        public void fadeOut(float seconds = 5, float minVolume = 0, float maxVolume = 1)
        {
            StartCoroutine(fade(seconds, FadesTypes.Out, minVolume, maxVolume));
        }

        private IEnumerator fade(float seconds, FadesTypes fadeType, float minVolume = 0, float maxVolume = 1)
        {
            float volumeToChangeInOneFrame = (maxVolume - minVolume) / (seconds / 0.1f);
            do
            {
                if (fadeType == FadesTypes.In)
                {
                    GetComponent<AudioSource>().volume -= volumeToChangeInOneFrame;
                }
                else if (fadeType == FadesTypes.Out)
                {
                    GetComponent<AudioSource>().volume += volumeToChangeInOneFrame;
                }
                seconds -= 0.1f;
                yield return new WaitForSeconds(0.1f); 
            } while (seconds > 0);
        }

    }
}