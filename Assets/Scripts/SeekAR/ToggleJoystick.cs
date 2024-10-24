using UnityEngine;

public class ToggleJoystick : MonoBehaviour
{
    bool state;
    public GameObject joyStick;
    public void Toggle()
    {
        if (state)
        {
            state = false;
            joyStick.SetActive(false);
            return;
        }
        else
        {
            state = true;
            joyStick.SetActive(true);
            return;
        }
    }
}
