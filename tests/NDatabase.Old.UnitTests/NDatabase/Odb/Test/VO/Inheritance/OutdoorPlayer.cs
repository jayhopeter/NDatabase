namespace Test.NDatabase.Odb.Test.VO.Inheritance
{
    public class OutdoorPlayer : Player
    {
        private string groundName; //1
        public string GroundName
        {
            get { return groundName; }
            set { groundName = value; }
        }
    }
}
