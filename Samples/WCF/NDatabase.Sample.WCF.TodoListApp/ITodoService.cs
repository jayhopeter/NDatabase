using System.Collections.Generic;
using System.ServiceModel;
using NDatabase.Sample.WCF.TodoListApp.Domain;

namespace NDatabase.Sample.WCF.TodoListApp
{
    [ServiceContract]
    public interface ITodoService
    {
        [OperationContract]
        void Save(string user, TodoItemWrapper todo);

        [OperationContract]
        List<TodoItemWrapper> GetAll(string user);

        [OperationContract]
        void MarkAsDone(string user, string name);

        [OperationContract]
        void Delete(string user, string name);
    }
}
