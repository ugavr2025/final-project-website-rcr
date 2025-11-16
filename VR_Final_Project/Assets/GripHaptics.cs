using UnityEngine;
using UnityEngine.InputSystem;

public class GripHaptics : MonoBehaviour
{
    public bool isRight;
    public InputActionProperty gripAction;
    InputAction fallback;
    bool prevPressed;
    void OnEnable()
    {
        if (gripAction.reference == null)
        {
            fallback = new InputAction(type: InputActionType.Button, binding: isRight ? "<XRController>{RightHand}/gripButton" : "<XRController>{LeftHand}/gripButton");
            fallback.Enable();
        }
        else gripAction.action.Enable();
    }
    void OnDisable()
    {
        if (fallback != null) fallback.Disable();
        else if (gripAction.reference != null) gripAction.action.Disable();
    }
    void Update()
    {
        bool pressed = gripAction.reference != null ? gripAction.action.ReadValue<float>() > 0.5f : fallback.ReadValue<float>() > 0.5f;
        if (pressed && !prevPressed)
        {
            if (Haptics.I != null)
            {
                if (isRight) Haptics.I.PulseRight(0.5f, 0.06f);
                else Haptics.I.PulseLeft(0.5f, 0.06f);
            }
        }
        prevPressed = pressed;
    }
}
