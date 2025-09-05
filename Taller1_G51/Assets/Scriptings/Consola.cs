using PackagePersona;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CajeroData
{
    public int clientesAtendidos;
    public float tiempoTotalAtencion;
}

[System.Serializable]
public class DatosTaller
{
    public int clientesSinAtender;
    public List<CajeroData> cajeros;
    public int totalConsignaciones;
}

public class Consola : MonoBehaviour
{
    [Header("UI")] 
    public Button botonIniciar;
    public Button botonDetener;
    public TextMeshProUGUI textoClientesEnCola;
    public TextMeshProUGUI textoConsignaciones;
    public Cajero[] cajeros;

    private Queue<Cliente> colaClientes = new Queue<Cliente>();
    private bool generando = false;
    private int totalConsignaciones = 0;

    private string[] nombres;
    private string[] tramites = { "Retirar", "Consignar" };
    private string[] direcciones;

    private void Start()
    {
        botonIniciar.onClick.AddListener(Iniciar);
        botonDetener.onClick.AddListener(Detener);

        // Inicializa los cajeros como libres
        foreach (var cajero in cajeros)
        {
            cajero.ActualizarEstado(true, null); // libre sin cliente
        }

        ActualizarIndicadores();
        ActualizarClientesEnCola();
        CargarNombresDesdeArchivo();
        CargarDireccionesDesdeArchivo();
    }

    void CargarNombresDesdeArchivo()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Nombres.txt");
        if (System.IO.File.Exists(filePath))
        {
            nombres = System.IO.File.ReadAllLines(filePath)
                .Where(l => !string.IsNullOrWhiteSpace(l)) // quitar l�neas vac�as
                .ToArray();
            Debug.Log("Nombres cargados: " + nombres.Length);
        }
        else
        {
            Debug.LogWarning("No se encontr� el archivo de nombres en: " + filePath);
            // si no existe, usar un fallback
            nombres = new string[] { "Ana", "Luis", "Maria", "Jorge", "Sofia" };
        }
    }

    void CargarDireccionesDesdeArchivo()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Direcciones.txt");
        if (System.IO.File.Exists(filePath))
        {
            direcciones = System.IO.File.ReadAllLines(filePath)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();
            Debug.Log("Direcciones cargadas: " + direcciones.Length);
        }
        else
        {
            Debug.LogWarning("No se encontr� el archivo de direcciones en: " + filePath);
            // fallback por si no existe el archivo
            direcciones = new string[] { "Calle Falsa 123" };
        }
    }




    void Iniciar()
    {
        if (!generando)
        {
            generando = true;
            StartCoroutine(GenerarClientes());
            foreach (var cajero in cajeros)
            {
                StartCoroutine(AtenderCajero(cajero));
            }
        }
    }

    void Detener()
    {
        generando = false;
        StopAllCoroutines();
        foreach (var cajero in cajeros)
    {
        cajero.ActualizarEstado(true, null); // libre sin cliente

    }
        GuardarDatosJSON();

    }
    

    

    IEnumerator GenerarClientes()
    {
        while (generando)
        {
            int cantidad = Random.Range(1, 4);
            for (int i = 0; i < cantidad; i++)
            {
                Cliente nuevo = CrearClienteAleatorio();
                colaClientes.Enqueue(nuevo);
            }

            ActualizarIndicadores();
            ActualizarClientesEnCola();
            yield return new WaitForSeconds(1f);
        }
    }
    
    
    Cliente CrearClienteAleatorio() 
    {
        string nombre = nombres[Random.Range(0, nombres.Length)];
        string correo = nombre.ToLower() + "@correo.com";
        string direccion = direcciones[Random.Range(0, direcciones.Length)];
        string id = System.Guid.NewGuid().ToString().Substring(0, 8);
        string tramite = tramites[Random.Range(0, tramites.Length)];
        float tiempo = Random.Range(2f, 5f);
        return new Cliente(nombre, correo, direccion, id, tramite, tiempo);
    }

    IEnumerator AtenderCajero(Cajero cajero)
    {
        while (generando)
        {
            if (colaClientes.Count > 0 && cajero.estaLibre)
            {
                Cliente cliente = colaClientes.Dequeue();

                if (cliente.tramite == "Consignar")
                    totalConsignaciones++;

                ActualizarIndicadores();
                ActualizarClientesEnCola();
                yield return StartCoroutine(cajero.AtenderCliente(cliente));

                yield return new WaitForSeconds(1.5f); // pausa entre clientes
            }
            yield return null;
        }
    }

    void ActualizarIndicadores()
    {
        textoConsignaciones.text = $"Consignaciones realizadas: {totalConsignaciones}";
    }

    void ActualizarClientesEnCola()
    {
        if (textoClientesEnCola == null)
        {
            Debug.LogWarning("textoClientesEnCola no est� asignado.");
            return;
        }

        if (colaClientes.Count == 0)
        {
            textoClientesEnCola.text = "No hay clientes en cola";
            return;
        }

        string texto = "Clientes en cola:\n";
        foreach (var cliente in colaClientes)
        {
            texto += $"{cliente.nombre} | {cliente.correo} | {cliente.direccion}\n";
        }
        textoClientesEnCola.text = texto;
    }

    public bool GuardarDatosJSON()
    {
        try
        {
            DatosTaller datos = new DatosTaller
            {
                clientesSinAtender = colaClientes.Count,
                totalConsignaciones = totalConsignaciones,
                cajeros = new List<CajeroData>()
            };
            foreach (var cajero in cajeros)
            {
                datos.cajeros.Add(new CajeroData        
                {
                    clientesAtendidos = cajero.clientesAtendidos,
                    tiempoTotalAtencion = cajero.tiempoTotalAtencion 
                });
            }
            string jsonString = JsonUtility.ToJson(datos, true);
            string folderPath = Application.streamingAssetsPath;
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            string filePath = System.IO.Path.Combine(folderPath, "resultadoTaller.json");
            System.IO.File.WriteAllText(filePath, jsonString);
            Debug.Log("Archivo JSON guardado correctamente en: " + filePath);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar archivo JSON: " + ex.Message);
            return false;
        }
    }
}
