using System.Text.Json.Serialization;
using ProyectoFinal.Modelo;

namespace Proyecto_Final.Modelo
{
    public class Mtipoiva
    {
        [JsonIgnore]
        public int codiva { get; set; }
        public string numiva { get; set; }
        public string desiva { get; set; }
        public decimal coheficiente { get; set; }
        public int codusu { get; set; }
        public int codempresa { get; set; }
        public Mempresa Empresa { get; set; }
        
    }
}
