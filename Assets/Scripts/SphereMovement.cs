using UnityEngine;
using UnityEngine.InputSystem;

public class SphereMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody _rb;
    private InputAction _moveAction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Using InputSystem.actions to find the "Move" action globally.
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void OnEnable()
    {
        _moveAction?.Enable();
    }

    private void OnDisable()
    {
        _moveAction?.Disable();
    }

    private void FixedUpdate()
    {
        if (_moveAction == null) return;

        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        _rb.AddForce(movement * speed);
    }
}
