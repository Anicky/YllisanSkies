using UnityEngine;

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
        public AudioClip cursor;
        public AudioClip submit;
        public AudioClip error;
        public AudioClip cancel;

        public void playSound(Sounds sound)
        {
            GetComponent<AudioSource>().PlayOneShot(getAudioClipBySoundName(sound));
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
    }
}