using System;

namespace Test.NDatabase.Odb.Test.Update.Nullobject
{
    /// <summary>
    ///   Fornecedor
    /// </summary>
    /// <author>Jeremias</author>
    public class Constructor
    {
        private DateTime creationDate;
        private bool deleted;
        private string description;
        private string name;

        private DateTime updateDate;

        private User user;

        // S ou N
        public virtual DateTime GetCreationDate()
        {
            return creationDate;
        }

        public virtual bool GetDeleted()
        {
            return deleted;
        }

        public virtual string GetDescription()
        {
            return description;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual DateTime GetUpdateDate()
        {
            return updateDate;
        }

        public virtual User GetUser()
        {
            return user;
        }

        public virtual void SetCreationDate(DateTime creationDate)
        {
            this.creationDate = creationDate;
        }

        public virtual void SetDeleted(bool deleted)
        {
            this.deleted = deleted;
        }

        public virtual void SetDescription(string description)
        {
            this.description = description;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual void SetUpdateDate(DateTime updateDate)
        {
            this.updateDate = updateDate;
        }

        public virtual void SetUser(User user)
        {
            this.user = user;
        }
    }
}
