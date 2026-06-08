// ============================================================
//  P1 - CONCURRENCIA | Concurrencia.cs
//  Demo de 4 formas de hacer trabajo: secuencial, Thread, Task y Coroutine.
//  PATRON CLAVE: cola de acciones (mainThreadActions) para tocar la API de
//  Unity (mover Transform) DESDE el hilo principal de forma segura.
//  Activa/desactiva cada metodo con los checkboxes del Inspector.
// ============================================================
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Concurrencia : MonoBehaviour
{
    [Header("Activa los metodos")]
    public bool useSyncrone;
    public bool useThread;
    public bool useTask;
    public bool useCoroutine;

    [Header("Esferas a mover")]
    public Transform syncroneSphere;
    public Transform threadSphere;
    public Transform taskSphere;
    public Transform coroutineSphere;

    public Transform mainCube;

    // Acciones del hilo secundario que se ejecutan en el hilo principal
    private Queue<Action> mainThreadActions = new Queue<Action>();

    void Start()
    {
        if (useSyncrone) SyncroneMove();
        if (useThread) MoveWithThread();
        if (useTask) MoveWithTask();
        if (useCoroutine) StartCoroutine(MoveWithCoroutine());
    }

    void Update()
    {
        if (mainCube != null) mainCube.Rotate(Vector3.up, 50 * Time.deltaTime);

        // Vaciar la cola de acciones en el hilo principal (con lock)
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
                mainThreadActions.Dequeue().Invoke();
        }
    }

    void SyncroneMove()
    {
        for (int i = 0; i < 100; i++)
            syncroneSphere.position += Vector3.right * 0.05f;
        Thread.Sleep(50);
    }

    void MoveWithThread()
    {
        new Thread(() =>
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);
                lock (mainThreadActions)
                    mainThreadActions.Enqueue(() => threadSphere.position += Vector3.right * 0.05f);
            }
        }).Start();
    }

    async void MoveWithTask()
    {
        await Task.Run(() =>
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);
                lock (mainThreadActions)
                    mainThreadActions.Enqueue(() => taskSphere.position += Vector3.right * 0.05f);
            }
        });
    }

    IEnumerator MoveWithCoroutine()
    {
        for (int i = 0; i < 100; i++)
        {
            coroutineSphere.position += Vector3.right * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
