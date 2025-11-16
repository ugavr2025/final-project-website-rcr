using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Haptics : MonoBehaviour
{
    public static Haptics I;
    InputDevice left, right;
    bool haveLeft, haveRight;
    void Awake(){ I = this; Refresh(); }
    void OnEnable(){ Refresh(); }
    void Refresh()
    {
        var lefts = new List<InputDevice>();
        var rights = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, lefts);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rights);
        haveLeft = lefts.Count > 0; if (haveLeft) left = lefts[0];
        haveRight = rights.Count > 0; if (haveRight) right = rights[0];
    }
    public void PulseLeft(float amplitude, float duration){ if(!haveLeft) Refresh(); if(haveLeft) left.SendHapticImpulse(0, Mathf.Clamp01(amplitude), Mathf.Max(0, duration)); }
    public void PulseRight(float amplitude, float duration){ if(!haveRight) Refresh(); if(haveRight) right.SendHapticImpulse(0, Mathf.Clamp01(amplitude), Mathf.Max(0, duration)); }
    public void PulseBoth(float amplitude, float duration){ PulseLeft(amplitude, duration); PulseRight(amplitude, duration); }
}
