using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cajero : MonoBehaviour
{
    public int id; 
    public bool estaLibre = true;
    public int clientesAtendidos = 0;
    public float tiempoTotalAtencion = 0f;

    public Image panelVisual; 
    public Text estadoTexto; 

    private Cliente clienteActual;

    public void ActualizarEstado(bool libre)
    {
        estaLibre = libre;
        if (libre)
        {
            panelVisual.color = Color.green;
            estadoTexto.text = "Libre";
        }
        else
        {
            panelVisual.color = Color.red;
            estadoTexto.text = "Ocupado";
        }
    }

    public IEnumerator AtenderCliente(Cliente cliente, System.Action onTerminar)
    {
        clienteActual = cliente;
        ActualizarEstado(false);
        clientesAtendidos++;
        float tiempo = cliente.tiempoAtencion;
        tiempoTotalAtencion += tiempo;
        yield return new WaitForSeconds(tiempo);
        clienteActual = null;
        ActualizarEstado(true);
        onTerminar?.Invoke();
    }
}