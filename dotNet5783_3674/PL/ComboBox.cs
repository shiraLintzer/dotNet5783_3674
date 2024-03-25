using System.Collections.Generic;
using System;
using System.Collections;

namespace PL;

internal class TaskCollection : IEnumerable
{
        static readonly IEnumerable<BO.TaskInEngineer> s_task =
    (BlApi.Factory.Get().Task.GetAvailableTask() as IEnumerable<BO.TaskInEngineer>)!;

        public IEnumerator GetEnumerator() => s_task.GetEnumerator();

    
}