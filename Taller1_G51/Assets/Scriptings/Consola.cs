using PackagePersona;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Consola : MonoBehaviour
{
    
    public Queue<Cliente> colaClientes = new Queue<Cliente>();
    public Cajero[] cajeros = new Cajero[4];

    private bool generandoClientes = false;
    private int contadorClientes = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cajeros = new Cajero[4]; // Por ejemplo, 4 cajeros

        for (int i = 0; i < cajeros.Length; i++)
        {
            GameObject cajeroGO = new GameObject("Cajero" + (i + 1));
            Cajero c = cajeroGO.AddComponent<Cajero>();
            c.Inicializar(i + 1);  // método que creamos en Cajero.cs
            cajeros[i] = c;
        }
    }




    public void Iniciar()
    {
        generandoClientes = true;
        InvokeRepeating("GenerarClientes", 1f, 1f);
        StartCoroutine(AsignarClientes());
    }

    public void Detener()
    {
        generandoClientes = false;
        CancelInvoke("GenerarClientes");
    }

    void GenerarClientes()
    {
        if (!generandoClientes) return;

        int cantidad = Random.Range(1, 4); // 1 a 3 clientes por segundo
        for (int i = 0; i < cantidad; i++)
        {
            contadorClientes++;
            string tramite = Random.value > 0.5f ? "Retiro" : "Consignar";
            float tiempo = Random.Range(2f, 5f);

            Cliente nuevo = new Cliente("Cliente" + contadorClientes, "correo@ejemplo.com", "direccion" , 
                "C" + contadorClientes, tramite, tiempo);

            colaClientes.Enqueue(nuevo);
            Debug.Log($"[COLA] Se agregó {nuevo.idCliente} - {nuevo.tramite} (t={nuevo.tiempoAtencion})");
        }
    }

    IEnumerator AsignarClientes()
    {
        while (true)
        {
            foreach (Cajero cajero in cajeros)
            {
                if (!cajero.ocupado && colaClientes.Count > 0)
                {
                    Cliente cliente = colaClientes.Dequeue();
                    StartCoroutine(cajero.AtenderCliente(cliente));
                }
            }
            yield return null;
        }
    }
}
