using PackagePersona;
using System.Collections;
using TMPro;                 // ✅ ya lo tienes
using UnityEngine;
using UnityEngine.UI;

public class Cajero : MonoBehaviour
{
    public int id;
    public bool estaLibre = true;
    public int clientesAtendidos = 0;
    public float tiempoTotalAtencion = 0f;

    public Image panelVisual;
    public TextMeshProUGUI estadoTexto;  // ✅ ahora sí es TMP

    private Cliente clienteActual;

    public void ActualizarEstado(bool libre, Cliente cliente = null)
    {
        estaLibre = libre;
        if (estadoTexto != null)
        {
            if (libre)
            {
                estadoTexto.text = "Libre";
                estadoTexto.color = Color.green;
            }
            else
            {
                estadoTexto.text = $"Ocupado: {cliente.nombre}";
                estadoTexto.color = Color.red;
            }
        }
    }

    public IEnumerator AtenderCliente(Cliente cliente, System.Action onTerminar = null)
    {
        clienteActual = cliente;
        ActualizarEstado(false, cliente);

        clientesAtendidos++;
        float tiempo = cliente.tiempoAtencion;
        tiempoTotalAtencion += tiempo;

        // Simular tiempo de atención
        yield return new WaitForSeconds(tiempo);

        clienteActual = null;
        ActualizarEstado(true);

        onTerminar?.Invoke();
    }
}

