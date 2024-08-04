namespace Magic.Service.Provider;

public interface ILogProvider
{
    Task Write(string text);
    Task WriteError(string text);
    Task WriteWarning(string text);
    Task WriteInformation(string text);
}