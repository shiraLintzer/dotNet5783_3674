using System.Collections.Generic;
using System;
using System.Collections;

namespace PL;

internal class TaskAvailableForEngineerCollection : IEnumerable
{
        static readonly IEnumerable<BO.TaskInEngineer> s_task =
    (BlApi.Factory.Get().Task.GetAvailableTask() as IEnumerable<BO.TaskInEngineer>)!;

        public IEnumerator GetEnumerator() => s_task.GetEnumerator();

    
}


internal class EngineerAvailableForTaskCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerInTask> s_task =
(BlApi.Factory.Get().Engineer.GetAvailableEngineer() as IEnumerable<BO.EngineerInTask>)!;

    public IEnumerator GetEnumerator() => s_task.GetEnumerator();


}



internal class TasksDependencies : IEnumerable
{
    static readonly IEnumerable<BO.TaskInList> s_task =
(BlApi.Factory.Get().Task.GetAllDependenciesOptions() as IEnumerable<BO.TaskInList>)!;

    public IEnumerator GetEnumerator() => s_task.GetEnumerator();


}



//internal class TaskToChoose : IEnumerable
//{
//    static readonly IEnumerable<BO.TaskInEngineer> s_taskInEng = 
//        BlApi.Factory.Get().Task.AllTaskInEngineer(BO.EngineerExperience.Competent);
//    public IEnumerator GetEnumerator() => s_taskInEng.GetEnumerator();
//}