// ============================================================
//  P1 - CONCURRENCIA | FlightThread.cs  (VERSION CONCURRENTE)
//  El calculo pesado se DELEGA a un HILO SECUNDARIO -> el frame queda libre
//  -> FPS estables. Compara con Flight.cs (secuencial).
//  Nota: se captura Time.time ANTES de lanzar el hilo (la API de Unity
//  NO es segura para hilos -> no se llama desde el hilo secundario).
// ============================================================
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightThread : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 50f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    public int turbulenceIterations = 100000;
    private List<Vector3> turbulenceForces = new List<Vector3>();

    private Thread turbulenceThread;
    private bool isTurbulenceRunning = false;
    private bool stopTurbulenceThread = false;
    private float capturedTime = 0;

    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (cameraTransform == null) { Debug.LogError("No hay camara asignada..."); return; }

        capturedTime = Time.time; // se captura en el hilo principal

        if (!isTurbulenceRunning)
        {
            isTurbulenceRunning = true;
            stopTurbulenceThread = false;
            turbulenceThread = new Thread(() => SimulateTurbulence(capturedTime));
            turbulenceThread.Start();
        }

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        transform.position += moveDirection;

        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence(float time)
    {
        turbulenceForces.Clear();
        for (int i = 0; i < turbulenceIterations; i++)
        {
            if (stopTurbulenceThread) break;
            Vector3 force = new Vector3(
                Mathf.PerlinNoise(i * 0.001f, time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.002f, time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.003f, time) * 2 - 1
            );
            turbulenceForces.Add(force);
        }
        isTurbulenceRunning = false;
    }

    private void OnDestroy()
    {
        stopTurbulenceThread = true;
        if (turbulenceThread != null && turbulenceThread.IsAlive)
            turbulenceThread.Join();
    }
}
