using PackagePersona;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;   

public class Cajero : MonoBehaviour
{
    public int idCajero;
    public bool ocupado = false; 
    public int clientesAtendidos = 0; 
    public float tiempoTotal = 0f;

    public Cajero(int id) 
    { idCajero = id; }

    public IEnumerator AtenderCliente(Cliente cliente)
    {
        ocupado = true;
        Debug.Log($"Cajero {idCajero} atendiendo a {cliente.idCliente} ({cliente.tramite})");

        // Simula el tiempo de atención
        yield return new WaitForSeconds(cliente.tiempoAtencion);

        clientesAtendidos++;
        tiempoTotal += cliente.tiempoAtencion;
        ocupado = false;

        Debug.Log($"Cajero {idCajero} terminó con {cliente.idCliente}");
    }
}
