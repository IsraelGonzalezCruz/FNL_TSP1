// ============================================================
//  P1 - CONCURRENCIA | Flight.cs  (VERSION SECUENCIAL)
//  El calculo pesado (SimulateTurbulence) corre en el HILO PRINCIPAL.
//  -> Bloquea el frame -> caen los FPS. Sirve para DEMOSTRAR el problema.
//  Compara con FlightThread.cs (concurrente).
//  Requiere: componente PlayerInput con una accion "Movement" (Input System).
// ============================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flight : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 50f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    public int turbulenceIterations = 100000;
    private List<Vector3> turbulenceForces = new List<Vector3>();

    // Lo llama PlayerInput (Behavior = Send Messages), accion "Movement"
    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void Update()
    {
        // PROCESO PESADO EN EL HILO PRINCIPAL (esto tira los FPS)
        SimulateTurbulence();

        if (cameraTransform == null) { Debug.LogError("No hay camara asignada..."); return; }

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        transform.position += moveDirection;

        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence()
    {
        turbulenceForces.Clear();
        for (int i = 0; i < turbulenceIterations; i++)
        {
            Vector3 force = new Vector3(
                Mathf.PerlinNoise(i * 0.001f, Time.time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.002f, Time.time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.003f, Time.time) * 2 - 1
            );
            turbulenceForces.Add(force);
        }
    }
}
