using System.Text.Json.Serialization;

namespace ProyectoFinal.Modelo{
    public class Mempresa{

            
            public int codempresa { get; set; }
            public string rucempresa { get; set; }
            public string razonsocial { get; set; }
            public string? propietario { get; set; }
            public string? ruc_ci { get; set; }
            public string direccion { get; set; }
            public string nrotelefono { get; set; }
            public string actividadeconomica { get; set; }

    }
}