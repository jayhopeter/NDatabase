namespace Test.NDatabase.Odb.Test.List.Update
{
    public class Publicacao
    {
        private string name;

        private string texto;

        public Publicacao(string name, string texto)
        {
            this.name = name;
            this.texto = texto;
        }

        public virtual string GetName()
        {
            return name;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual string GetTexto()
        {
            return texto;
        }

        public virtual void SetTexto(string texto)
        {
            this.texto = texto;
        }

        public override string ToString()
        {
            return name + ":" + texto;
        }
    }
}
