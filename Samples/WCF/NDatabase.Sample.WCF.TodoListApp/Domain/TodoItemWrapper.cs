using System;
using System.Runtime.Serialization;

namespace NDatabase.Sample.WCF.TodoListApp.Domain
{
    [DataContract]
    public class TodoItemWrapper
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsDone { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }
    }
}