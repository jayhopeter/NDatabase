using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.ServiceModel;
using NDatabase.Sample.WCF.TodoListApp.Domain;
using NDatabase.TodoList.Domain;

namespace NDatabase.Sample.WCF.TodoListApp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TodoService : ITodoService
    {
        private readonly ConcurrentDictionary<string, string> _dbNamesCache = new ConcurrentDictionary<string, string>();

        private void LogIn(string user)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(user));
            
            EnsureUniqueIndex(user);
        }

        private void EnsureUniqueIndex(string user)
        {
            var dbName = GetDbName(user);

            if (File.Exists(dbName))
                return;

            using (var odb = OdbFactory.Open(GetDbName(user)))
            {
                odb.IndexManagerFor<TodoItem>().AddUniqueIndexOn("Unique_index_todo_item", "Name");
            }
        }

        public void Save(string user, TodoItemWrapper todo)
        {
            Contract.Requires(user != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(todo.Name));

            LogIn(user);

            var todoItem = todo.FromWrapper();

            using (var odb = OdbFactory.Open(GetDbName(user)))
            {
                odb.Store(todoItem);
            }
        }

        public List<TodoItemWrapper> GetAll(string user)
        {
            Contract.Requires(user != null);

            LogIn(user);

            using (var odb = OdbFactory.Open(GetDbName(user)))
            {
                return odb.QueryAndExecute<TodoItem>().Select(x => x.ToWrapper()).ToList();
            }
        }

        public void MarkAsDone(string user, string name)
        {
            Contract.Requires(user != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            LogIn(user);

            using (var odb = OdbFactory.Open(GetDbName(user)))
            {
                var item = (from todoItem in odb.AsQueryable<TodoItem>()
                            where todoItem.Name.Equals(name)
                            select todoItem).FirstOrDefault();

                if (item == null)
                    return;

                item.IsDone = true;
                odb.Store(item);
            }
        }

        public void Delete(string user, string name)
        {
            Contract.Requires(user != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            LogIn(user);

            using (var odb = OdbFactory.Open(GetDbName(user)))
            {
                var item = (from todoItem in odb.AsQueryable<TodoItem>()
                            where todoItem.Name.Equals(name)
                            select todoItem).FirstOrDefault();

                if (item == null)
                    return;

                odb.Delete(item);
            }
        }

        private string GetDbName(string login)
        {
            return _dbNamesCache.GetOrAdd(login, ProduceDbName);
        }

        private static string ProduceDbName(string login)
        {
            var dbName = string.Format("{0}_wcf_sample.ndb", login);

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "NDatabase");

            return Path.Combine(path, dbName);
        }
    }
}