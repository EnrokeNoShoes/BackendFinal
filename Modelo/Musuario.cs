using System.Text.Json.Serialization;

namespace ProyectoFinal.Modelo{
    public class Musuario{
         [JsonIgnore]
        public int codusu { get; set; }
        public string nomusu { get; set; }
        public string passusu { get; set; }
        public int codgrupo { get; set; }
        public int codempresa { get; set; }
         [JsonIgnore]
         public Mempresa Empresa { get; set; }
    }

}