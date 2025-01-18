namespace ProyectoFinal.Modelo{

    public class Msucursal{

        public int codsucursal { get; set; }
        public string numsuc { get; set; }
        public string dessucu { get; set; }
        public string dirsuc { get; set; }
        public string telfsuc { get; set; }
        public int tipo { get; set; }
        public List<Mdeposito> Depositos { get; set; }
    }

    public class Mdeposito
    {
        public int codsucursal { get; set; }
        public int CodSucursalPertenece { get; set; }
        public string NumSuc { get; set; }
        public string DesSucu { get; set; }
        public Msucursal Sucursal { get; set; }
    }
}