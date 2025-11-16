using UnityEngine;
using System.Reflection;

public class TwoHandScale : MonoBehaviour
{
    public float minScale = 0.1f;
    public float maxScale = 3f;

    Transform leftHandTransform;
    Transform rightHandTransform;

    Component grabComponent;       
    FieldInfo grabbedByField;      

    bool isHeld;
    float initialDistance;
    Vector3 initialScale;

    void Start()
    {
        // find your hands by name
        GameObject leftObj = GameObject.Find("LeftHand");
        GameObject rightObj = GameObject.Find("RightHand");

        if (leftObj != null)  leftHandTransform  = leftObj.transform;
        if (rightObj != null) rightHandTransform = rightObj.transform;

        
        grabComponent = GetComponent("XRGrabbable");

        if (grabComponent != null)
        {
            var t = grabComponent.GetType();

            
            grabbedByField =
                t.GetField("grabbedBy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ??
                t.GetField("GrabbedBy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (grabbedByField == null)
            {
                Debug.LogWarning("TwoHandScale: could not find 'grabbedBy' or 'GrabbedBy' field on XRGrabbable.", this);
            }
        }
        else
        {
            Debug.LogWarning("TwoHandScale: XRGrabbable component not found on this object.", this);
        }
    }

    bool IsHeld()
    {
        if (grabComponent == null || grabbedByField == null)
            return false;

       
        object value = grabbedByField.GetValue(grabComponent);
        var unityObj = value as Object;
        return unityObj != null;
    }

    void Update()
    {
        if (leftHandTransform == null || rightHandTransform == null)
            return;

        bool currentlyHeld = IsHeld();

        if (currentlyHeld && !isHeld)
        {
           
            isHeld = true;
            initialScale = transform.localScale;

            initialDistance = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);
            if (initialDistance < 0.001f)
                initialDistance = 0.001f;
        }
        else if (!currentlyHeld && isHeld)
        {
            
            isHeld = false;
        }

        if (isHeld && initialDistance > 0.0001f)
        {
            float currentDistance = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);
            float ratio = currentDistance / initialDistance;

            float s = Mathf.Clamp(initialScale.x * ratio, minScale, maxScale);
            transform.localScale = new Vector3(s, s, s);
        }
    }
}
