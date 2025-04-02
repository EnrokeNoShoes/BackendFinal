namespace ProyectoFinal.Modelo{
    public class Mproducto{
        public int codproducto { get; set; }
        public string codigobarra { get; set; }
        public string desproducto { get; set; }
        public int estado { get; set; }
        public string fechaingreso { get; set; }
        public string fechabaja { get; set; }
        public int codmarca { get; set; }
        public string nummarca {get; set;}
        public string desmarca {get; set;}
        public int codrubro { get; set; }
        public string numrubro {get; set;}
        public string desrubro {get; set;}
        public int codfamilia { get; set; }
        public string numfamilia {get; set;}
        public string desfamilia {get; set;}
        public int codproveedor { get; set; }
        public string numproveedor {get; set;}
        public string numdoc {get; set;}
        public string razonsocial {get; set;}
        public decimal costoultimo { get; set; }
        public decimal costopromedio { get; set; }
        public int codiva { get; set; }
        public string numiva {get; set;}
        public string desiva {get; set;}
        public decimal coheficiente {get; set;}
        public int afectastock { get; set; }
        public int codunidadmedida { get; set; }
        public string numunidadmedida {get;set;}
        public string desunidadmedida {get;set;}
        public decimal cantidad { get; set; }
        public int codusu { get; set; }

    }


    public class Mproductosucursal{

        public int codproducto { get; set; }
        public string codigobarra {get; set;}
        public string desproducto {get; set;}
        public decimal costoultimo {get; set;}
        public int codsucursal { get; set; }
        public string numsuc {get; set;}
        public string dessucu {get; set;}
        public decimal cantidadinicial { get; set; }
        public decimal cantidadactual { get; set; }
        public string alerta { get; set; }
        public decimal cantidadalerta { get; set; }
        public string fechaactualizacion { get; set; }

    }

}