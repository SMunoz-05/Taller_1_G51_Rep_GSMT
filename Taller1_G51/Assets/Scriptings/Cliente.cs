using UnityEngine;

namespace PackagePersona
{
    public class Cliente : Persona
    {

        public string idCliente;
        public string tramite;
        public float tiempoAtencion;

        public string IdCliente { get => idCliente; set => idCliente = value; }
        public string Tramite { get => tramite; set => tramite = value; }
        public float TiempoAtencion { get => tiempoAtencion; set => tiempoAtencion = value; }

        public Cliente()
        {

        }

        public Cliente(string idCliente, string tramite, float tiempoAtencion, string nombre, string correo, string direccion)
         : base(nombre, correo, direccion)
        {
            this.IdCliente = idCliente;
            this.Tramite = tramite;
            this.TiempoAtencion = tiempoAtencion;
        }
    }   
}
