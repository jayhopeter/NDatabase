namespace NDatabase.UnitTests.CodeSnippets.Data
{
	public sealed class Sport
	{
	    private readonly string _name;

	    public Sport(string name)
		{
			_name = name;
		}

	    public string Name
	    {
	        get { return _name; }
	    }

	    public string Value { get; set; }

	    public override string ToString()
		{
			return Name;
		}

	    private bool Equals(Sport other)
	    {
	        return string.Equals(Name, other.Name);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) 
                return false;
	        if (ReferenceEquals(this, obj)) 
                return true;
	        return obj is Sport && Equals((Sport) obj);
	    }

	    public override int GetHashCode()
	    {
	        return (Name != null ? Name.GetHashCode() : 0);
	    }
	}
}
