using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomotrizApp.dominio
{
    public class Vehiculo
    {
        public int VehiculoNro { get; set; }
        public int Precio { get; set; }
        public DateTime Año { get; set; }
        public string Color { get; set; }
        public string Modelo { get; set; }
        public bool Activo { get; set; }

        public Vehiculo(int nro, int pre,string color,string modelo,DateTime año)
        {
            VehiculoNro = nro;
            Precio = pre;
            Color = color;
            Modelo = modelo;
            Activo = true;
            Año = año;
        }

        public override string ToString()
        {
            return Modelo;
        }

    }
}
