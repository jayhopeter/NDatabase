using System.Collections;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    /// <author>olivier</author>
    public class Handler
    {
        private IList parameters;

        public Handler()
        {
            parameters = new ArrayList();
        }

        public virtual IList GetListOfParameters()
        {
            return parameters;
        }

        public virtual void SetListOfParameters(IList listOfParameters)
        {
            parameters = listOfParameters;
        }

        /// <param name="parameter"> </param>
        public virtual void AddParameter(Parameter parameter)
        {
            parameters.Add(parameter);
        }
    }
}
