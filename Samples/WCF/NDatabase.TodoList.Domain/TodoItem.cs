using System;

namespace NDatabase.TodoList.Domain
{
    public class TodoItem
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
