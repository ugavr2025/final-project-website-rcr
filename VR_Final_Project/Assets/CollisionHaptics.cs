using UnityEngine;

public class CollisionHaptics : MonoBehaviour
{
    public float minSpeed = 0.4f;
    public float maxSpeed = 4f;
    public float maxAmplitude = 0.9f;
    public float duration = 0.05f;
    void OnCollisionEnter(Collision c)
    {
        if (Haptics.I == null) return;
        float v = c.relativeVelocity.magnitude;
        if (v < minSpeed) return;
        float t = Mathf.Clamp01((v - minSpeed) / Mathf.Max(0.0001f, maxSpeed - minSpeed));
        float amp = Mathf.Clamp01(t) * maxAmplitude;
        Haptics.I.PulseBoth(amp, duration);
    }
}
