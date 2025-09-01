using System;

public class Cliente : Persona
{
    public string idCliente;
    public string tramite; 
    public float tiempoAtencion;

    public Cliente(
        string nombre,
        string correo,
        string direccion,
        string idCliente,
        string tramite,
        float tiempoAtencion
    ) : base(nombre, correo, direccion)
    {
        this.idCliente = idCliente;
        this.tramite = tramite;
        this.tiempoAtencion = tiempoAtencion;
    }
}
