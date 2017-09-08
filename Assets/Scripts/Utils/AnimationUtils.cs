using UnityEngine;

namespace RaverSoft.YllisanSkies.Utils
{
    public class AnimationUtils
    {
        public static string getCurrentAnimationNameFromAnimator(Animator animator)
        {
            var currentAnimationName = "";
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
                {
                    currentAnimationName = clip.name.ToString();
                }
            }
            return currentAnimationName;
        }
    }
}