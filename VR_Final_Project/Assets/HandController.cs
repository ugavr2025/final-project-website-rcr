using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    XRGrabbable grabbedObject = null;
    List<XRGrabbable> grabbablesInTrigger = new List<XRGrabbable>();
    [SerializeField] InputAction grabAction;
    [SerializeField] float grabThreshold = .2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float grabber = grabAction.ReadValue<float>();
        if (grabber > grabThreshold && grabbedObject == null && grabbablesInTrigger.Count > 0)
        {
            grabbedObject = grabbablesInTrigger[0];
            grabbedObject.Grab(this);
        }
        if (grabber <= grabThreshold && grabbedObject != null)
        {
            grabbedObject.Release(this);
            grabbedObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.attachedRigidbody?.GetComponent<XRGrabbable>();
        if (grabbable != null && !grabbablesInTrigger.Contains(grabbable))
        {
            grabbablesInTrigger.Add(grabbable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var grabbable = other.attachedRigidbody?.GetComponent<XRGrabbable>();
        if (grabbable != null && grabbablesInTrigger.Contains(grabbable))
        {
            grabbablesInTrigger.Remove(grabbable);
        }
    }
}
