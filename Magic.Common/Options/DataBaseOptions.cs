namespace Magic.Common.Options;

public class DataBaseOptions
{
    public DataBaseOptions(string dBConnectionString)
    {
        DBConnectionString = dBConnectionString;
    }

    public string DBConnectionString { get; }
}