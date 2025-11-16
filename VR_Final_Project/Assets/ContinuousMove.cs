using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMove : MonoBehaviour
{
    public CharacterController controller;
    public Transform xrCamera;

    
    public InputActionProperty moveAction;
    InputAction _fallback;   

    public float moveSpeed = 1.75f;

    void OnEnable()
    {
        if (moveAction.reference == null)
        {
            _fallback = new InputAction(type: InputActionType.Value,
                                        binding: "<XRController>{LeftHand}/thumbstick");
            _fallback.Enable();
        }
        else moveAction.action.Enable();
    }

    void OnDisable()
    {
        if (_fallback != null) _fallback.Disable();
        else if (moveAction.reference != null) moveAction.action.Disable();
    }

    void Update()
    {
        if (!controller || !xrCamera) return;

        Vector2 input = moveAction.reference != null
                        ? moveAction.action.ReadValue<Vector2>()
                        : _fallback.ReadValue<Vector2>();

        float yaw = xrCamera.eulerAngles.y;
        Quaternion headYaw = Quaternion.Euler(0f, yaw, 0f);
        Vector3 moveDir = headYaw * new Vector3(input.x, 0f, input.y);

        controller.Move(moveDir * moveSpeed * Time.deltaTime);
        controller.Move(Physics.gravity * Time.deltaTime);
    }
}
