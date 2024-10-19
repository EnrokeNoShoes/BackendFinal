namespace ProyectoFinal.Modelo{
    public class Musuario{
        public int codusu { get; set; }
        public string nomusu { get; set; }
        public string passusu { get; set; }
        public int codgrupo { get; set; }
        public int codempresa { get; set; }
         public Mempresa Empresa { get; set; }
    }

}