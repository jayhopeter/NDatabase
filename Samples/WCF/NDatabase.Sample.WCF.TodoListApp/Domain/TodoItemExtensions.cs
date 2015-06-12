using NDatabase.TodoList.Domain;

namespace NDatabase.Sample.WCF.TodoListApp.Domain
{
    internal static class TodoItemExtensions
    {
        internal static TodoItemWrapper ToWrapper(this TodoItem self)
        {
            return new TodoItemWrapper { Name = self.Name, Description = self.Description, IsDone = self.IsDone, CreatedOn = self.CreatedOn };
        }

        internal static TodoItem FromWrapper(this TodoItemWrapper self)
        {
            return new TodoItem { Name = self.Name, Description = self.Description, IsDone = self.IsDone, CreatedOn = self.CreatedOn };
        }
    }
}