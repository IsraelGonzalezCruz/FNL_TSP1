// ============================================================
//  P1 - CONCURRENCIA (Actividad 3) | FlightThreadSync.cs
//  SINCRONIZACION de acceso a un archivo entre dos hilos:
//   - Mecanismo 1: bandera 'write' (causalidad: leer solo despues de escribir)
//   - Mecanismo 2: lock(filelock) (exclusion mutua: un hilo a la vez)
//  Juntos eliminan el IOException por acceso concurrente al archivo.
// ============================================================
using System.Collections.Generic;
using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightThreadSync : MonoBehaviour
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

    // Banderas de control de lectura/escritura
    public bool read = false;
    public bool write = false;
    private object filelock = new object();

    public string filepath;

    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void Start()
    {
        filepath = Application.dataPath + "/TurbulenceData.txt";
        Debug.Log("RUTA DEL ARCHIVO: " + filepath);
    }

    void Update()
    {
        if (cameraTransform == null) { Debug.LogError("No hay camara asignada..."); return; }

        capturedTime = Time.time;

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

        // ACTIVIDAD 3: leer solo cuando ya se escribio (causalidad)
        if (write && !read)
        {
            TryReadFile();
            read = true;
        }
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

        Debug.Log("Iniciando simulacion de turbulencia...");
        Debug.Log("Escribiendo el archivo...");

        // EXCLUSION MUTUA al escribir
        lock (filelock)
        {
            using (StreamWriter writer = new StreamWriter(filepath, false))
            {
                foreach (var force in turbulenceForces)
                    writer.WriteLine(force.ToString());
                writer.Flush();
            }
        }

        Debug.Log("Archivo escrito...");
        isTurbulenceRunning = false;
        write = true; // recien ahora se permite leer
    }

    void TryReadFile()
    {
        try
        {
            // EXCLUSION MUTUA al leer
            lock (filelock)
            {
                if (File.Exists(filepath))
                {
                    string content = File.ReadAllText(filepath);
                    Debug.Log("Archivo leido... " + content);
                }
                else
                {
                    Debug.LogError("Ocurrio un problema...");
                }
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("Error de acceso al archivo... " + ex.Message);
        }
    }

    private void OnDestroy()
    {
        stopTurbulenceThread = true;
        if (turbulenceThread != null && turbulenceThread.IsAlive)
            turbulenceThread.Join();
    }
}
