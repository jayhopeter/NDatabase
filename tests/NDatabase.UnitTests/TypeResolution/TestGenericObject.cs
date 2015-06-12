using System.Collections.Generic;

namespace NDatabase.UnitTests.TypeResolution
{
    public class TestGenericObject<T, U>
    {
        private string _name = string.Empty;
        private IDictionary<string, int> _someGenericDictionary = new Dictionary<string, int>();
        private IList<int> _someGenericList = new List<int>();

        public TestGenericObject()
        {
        }

        public TestGenericObject(int id)
        {
            ID = id;
        }

        public TestGenericObject(int id, string name)
        {
            ID = id;
            _name = name;
        }

        public int ID { get; set; }

        public T Tprop { get; set; }
        public U Uprop { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IList<int> SomeGenericList
        {
            get { return _someGenericList; }
            set { _someGenericList = value; }
        }

        public IDictionary<string, int> SomeGenericDictionary
        {
            get { return _someGenericDictionary; }
            set { _someGenericDictionary = value; }
        }
    }
}