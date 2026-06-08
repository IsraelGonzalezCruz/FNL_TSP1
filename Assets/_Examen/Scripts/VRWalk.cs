// ============================================================
//  VR (Cardboard) | VRWalk.cs
//  Locomocion VR "mirando hacia abajo": si inclinas la cabeza dentro de un
//  rango de angulo, el personaje camina hacia donde mira la camara VR.
//  Requiere un CharacterController en el mismo objeto y arrastrar la camara VR.
// ============================================================
using UnityEngine;

public class VRWalk : MonoBehaviour
{
    public Transform vrCameraTransform;
    public float move_angle = 30.0f;
    public float max_move_angle = 60.0f;
    public float speed = 3.0f;
    public bool move;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (vrCameraTransform == null) return;

        float pitch = vrCameraTransform.eulerAngles.x;
        move = (pitch >= move_angle && pitch < max_move_angle);

        if (move)
        {
            Vector3 direction = vrCameraTransform.TransformDirection(Vector3.forward);
            controller.SimpleMove(direction * speed);
        }
    }
}
