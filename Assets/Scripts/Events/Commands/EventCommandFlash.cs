using UnityEngine;
using UnityEngine.UI;

namespace RaverSoft.YllisanSkies.Events.Commands
{
    public class EventCommandFlash : MonoBehaviour
    {
        private Game game;
        private Color color;
        private float seconds;
        private float intensity;
        private bool stopEvents;
        private bool isStarted = false;
        private bool isFinished = false;
        private Canvas canvas;
        private RawImage image;

        public static EventCommandFlash createComponent(GameObject eventObject, Game game, Color color, float seconds, bool stopEvents, float intensity = 1)
        {
            EventCommandFlash component = eventObject.AddComponent<EventCommandFlash>();
            component.seconds = seconds;
            component.color = new Color(color.r, color.g, color.b, intensity);
            component.stopEvents = stopEvents;
            component.game = game;
            component.intensity = intensity;
            component.canvas = GameObject.Find("Game/Canvas").GetComponent<Canvas>();
            component.image = GameObject.Find("Game/Canvas/FadeOverlay").GetComponent<RawImage>();
            return component;
        }

        private void Update()
        {
            if (isStarted && !isFinished)
            {
                image.color = new Color(color.r, color.g, color.b, image.color.a - (Time.deltaTime / seconds) * (intensity));
                if (image.color.a <= 0)
                {
                    end();
                }
            }
        }

        public void init()
        {
            if (stopEvents)
            {
                game.stopAllEvents();
            }
            canvas.enabled = true;
            image.texture = null;
            image.color = color;
            isStarted = true;
        }

        private void end()
        {
            canvas.enabled = false;
            if (stopEvents)
            {
                game.resumeAllEvents();
            }
            isFinished = true;
        }
    }
}