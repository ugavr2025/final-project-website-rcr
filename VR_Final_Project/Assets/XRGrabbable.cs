using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class XRGrabbable : MonoBehaviour
{
    public HandController grabbedBy;
    public Rigidbody rb;

    public AudioClip pickupClip;
    public AudioClip throwClip;
    AudioSource source;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;

        source = GetComponent<AudioSource>();
        if (!source) source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 1f;
        source.volume = 0.7f;
    }

    void Update() { }

    void FixedUpdate()
    {
        if (grabbedBy != null)
        {
            Vector3 toHand = grabbedBy.transform.position - transform.position;
            rb.linearVelocity = toHand / Time.fixedDeltaTime;

            Quaternion toHandRot = grabbedBy.transform.rotation * Quaternion.Inverse(transform.rotation);
            Vector3 axis; float angle;
            toHandRot.ToAngleAxis(out angle, out axis);
            rb.angularVelocity = angle * Mathf.Deg2Rad * axis / Time.fixedDeltaTime;
        }
    }

    public void Grab(HandController hand)
    {
        grabbedBy = hand;
        if (source && pickupClip) source.PlayOneShot(pickupClip);
    }

    public void Release(HandController hand)
    {
        grabbedBy = null;
        if (source && throwClip) source.PlayOneShot(throwClip);
    }
}
