using UnityEngine;

public class MouseUtils
{
    public const int INPUT_CLICK_LEFT = 0;
    public const int INPUT_CLICK_RIGHT = 1;
    public const int INPUT_CLICK_MIDDLE = 2;

    public static Vector2 getPositionFromMouse()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
