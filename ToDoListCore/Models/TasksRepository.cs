using System.Collections.Generic;

namespace ToDoListCore.Models
{
    public class TasksRepository
    {
        private static List<Zadanie> tasks = new List<Zadanie>();

        public static IEnumerable<Zadanie> Zadania => tasks;

        public static void AddTask(Zadanie task)
        {
            tasks.Add(task);
        }
    }
}
