namespace ProyectoFinal.Persistencia
{
    public class Sucursal : ISql
    {
        public string Delete()
        {
            throw new NotImplementedException();
        }

        public string Insert()
        {
            return @"insert into sucursal(codsucursal, numsuc, dessucu, telfsuc, codsucpertenece, tipo)
                values(@codsucursal, @numsuc, @dessuc, @telsuc, @codsucpertenece, @tipo)";
        }

        public string InsertDet()
        {
            throw new NotImplementedException();
        }

        public string Select(int option)
        {
            throw new NotImplementedException();
        }

        public string SelectDet(int option)
        {
            throw new NotImplementedException();
        }

        public string Update()
        {
            throw new NotImplementedException();
        }
    }
}