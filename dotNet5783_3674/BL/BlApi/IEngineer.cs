namespace BlApi;


public interface IEngineer
{
    public int Create(BO.Engineer item);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer?> ReadAll();
    public void Update(BO.Engineer item);
    public void Delete(int id);

    //public void Reset();


    public IEnumerable<BO.Engineer?> GetEngineersAppropriateLevel(BO.EngineerExperience level);
    public IEnumerable<BO.Engineer?> GetEngineersAppropriateCost(int cost);
    public IEnumerable<BO.EngineerInTask?> GetAvailableEngineer();
}
