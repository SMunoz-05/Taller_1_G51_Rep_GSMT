using PackagePersona;
using UnityEngine;
using System.Collections;

public class Cajero : MonoBehaviour
{
    public int idCajero;
    public bool ocupado = false;
    public int clientesAtendidos = 0;
    public float tiempoTotal = 0f;

    // ✅ En lugar de constructor, usamos un método de inicialización
    public void Inicializar(int id)
    {
        idCajero = id;
    }

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

    // Método opcional para obtener estadísticas
    public string GetEstadisticas()
    {
        return $"📊 Cajero {idCajero}: {clientesAtendidos} clientes, {tiempoTotal} seg en total.";
    }
}
