namespace DalApi
{
    public interface IDal
    {
        IDependency Dependency { get; }
        IEngineer Engineer { get; }
        ITask Task { get; }

        //reset all data
        void Reset();
        //function to update project end date
        public void updateEndProject(DateTime? value);
        //function to update project start date
        public void updateStartProject(DateTime? value);
        //function to return project end date
        public DateTime? ReturnEndProject();
        //function to return project start date
        public DateTime? ReturnStartProject();

    }
}
