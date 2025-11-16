using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportLocomotion : MonoBehaviour
{
    public Transform rigRoot;
    public Transform cameraTransform;
    public Transform rayOrigin;         // RightHand
    public LineRenderer line;
    public LayerMask floorMask;
    public GameObject reticlePrefab;

    public InputActionProperty triggerAction; // optional
    InputAction _fallback;

    GameObject _reticle;
    Vector3 _hitPoint;
    bool _aiming;
    bool _hasHit;

    void OnEnable()
    {
        if (triggerAction.reference == null)
        {
            _fallback = new InputAction(type: InputActionType.Button,
                                        binding: "<XRController>{RightHand}/triggerButton");
            _fallback.Enable();
        }
        else triggerAction.action.Enable();
    }

    void OnDisable()
    {
        if (_fallback != null) _fallback.Disable();
        else if (triggerAction.reference != null) triggerAction.action.Disable();
    }

    void Start()
    {
        if (reticlePrefab)
        {
            _reticle = Instantiate(reticlePrefab);
            _reticle.SetActive(false);
        }
        if (line) { line.enabled = false; line.positionCount = 2; }
    }

    void Update()
    {
        float t = triggerAction.reference != null
                  ? triggerAction.action.ReadValue<float>()
                  : _fallback.ReadValue<float>();

        if (t > 0.2f) Aim();
        else
        {
            if (_aiming && _hasHit) Teleport();
            _aiming = false; _hasHit = false;
            if (line) line.enabled = false;
            if (_reticle) _reticle.SetActive(false);
        }
    }

    void Aim()
    {
        _aiming = true;
        _hasHit = false;

        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (Physics.Raycast(ray, out var hit, 25f, floorMask))
        {
            _hasHit = true;
            _hitPoint = hit.point;

            if (line)
            {
                line.enabled = true;
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, _hitPoint);
            }
            if (_reticle)
            {
                _reticle.SetActive(true);
                _reticle.transform.position = _hitPoint;
            }
        }
        else
        {
            // show a short debug line so you can see direction even if no hit
            if (line)
            {
                line.enabled = true;
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, ray.origin + ray.direction * 2f);
            }
            if (_reticle) _reticle.SetActive(false);
        }
    }

    void Teleport()
    {
        Vector3 headInRig = cameraTransform.position - rigRoot.position;
        headInRig.y = 0f;
        Vector3 target = _hitPoint - headInRig;

        var cc = rigRoot.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;
        rigRoot.position = target;
        if (cc) cc.enabled = true;
    }
}
