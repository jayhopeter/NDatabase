namespace Test.NDatabase.Odb.Test.Newbie.VO
{
    /// <summary>
    ///   It is just a simple test to help the newbies
    /// </summary>
    /// <author>mayworm at
    ///   <xmpp://mayworm@gmail.com>
    /// </author>
    public class Car
    {
        public Car(string name, int numberOfOccupant, string model)
        {
            Name = name;
            NumberOfOccupant = numberOfOccupant;
            Model = model;
        }

        public Car(string name, int numberOfOccupant, string model, Driver driver)
        {
            Name = name;
            NumberOfOccupant = numberOfOccupant;
            Model = model;
            Driver = driver;
        }

        public string Name { get; set; }
        public int NumberOfOccupant { get; set; }
        public string Model { get; set; }
        public Driver Driver { get; set; }
    }
}
