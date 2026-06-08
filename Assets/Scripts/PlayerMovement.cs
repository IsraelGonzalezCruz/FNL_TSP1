using UnityEngine;
using UnityEngine.InputSystem; // Esta es necesaria para las acciones de movimiento

public class PlayerMovement : MonoBehaviour // Asegúrate de que el nombre coincida con el archivo
{
    [SerializeField] private float speed = 5f;

    private InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        if (moveAction == null) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}