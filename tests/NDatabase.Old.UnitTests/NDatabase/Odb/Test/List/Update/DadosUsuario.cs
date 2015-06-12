using System.Collections;

namespace Test.NDatabase.Odb.Test.List.Update
{
    /// <author>gusto</author>
    public class DadosUsuario
    {
        private string confirmaSenha;
        private string email;
        private string login;

        private string nome;
        private string oid;

        private IList publicados;
        private string senha;

        /// <summary>
        ///   Creates a new instance of DadosUsuario
        /// </summary>
        public DadosUsuario()
        {
        }

        public DadosUsuario(string login, string senha)
        {
            this.login = login;
            this.senha = senha;
        }

        public DadosUsuario(string login, string senha, string email, string confirmaSenha)
        {
            this.login = login;
            this.senha = senha;
            this.confirmaSenha = confirmaSenha;
            this.email = email;
        }

        public virtual string GetEmail()
        {
            return email;
        }

        public virtual void SetEmail(string email)
        {
            this.email = email;
        }

        public virtual string GetSenha()
        {
            return senha;
        }

        public virtual void SetSenha(string senha)
        {
            this.senha = senha;
        }

        public virtual string GetLogin()
        {
            return login;
        }

        public virtual void SetLogin(string login)
        {
            this.login = login;
        }

        public virtual string GetConfirmaSenha()
        {
            return confirmaSenha;
        }

        public virtual void SetConfirmaSenha(string confirmaSenha)
        {
            this.confirmaSenha = confirmaSenha;
        }

        public virtual string GetNome()
        {
            return nome;
        }

        public virtual void SetNome(string nome)
        {
            this.nome = nome;
        }

        public virtual string GetOid()
        {
            return oid;
        }

        public virtual void SetOid(string oid)
        {
            this.oid = oid;
        }

        public virtual IList GetPublicados()
        {
            return publicados;
        }

        public virtual void SetPublicados(IList publicados)
        {
            this.publicados = publicados;
        }
    }
}
