namespace ProyectoFinal.Persistencia{
    public interface ISql
    {
        string Insert();
        string InsertDet();
        string Select(int option);
        string SelectDet(int option);
        string Update();
        string Delete();
    }
}