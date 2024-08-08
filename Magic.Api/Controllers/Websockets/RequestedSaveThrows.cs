using System.Collections.Concurrent;

namespace Magic.Api.Controllers.Websockets;

public class RequestedSaveThrows
{
    // connectionId -> RequestedSaveThrow
    private readonly ConcurrentDictionary<string, RequestedSaveThrow> _requestedSaveThrows = new();

    public void RequestSaveThrow(string connectionId, RequestedSaveThrow requestedSaveThrow)
    {
        _requestedSaveThrows.AddOrUpdate(
            connectionId,
            _ => requestedSaveThrow,
            (_, __) => requestedSaveThrow
        );
    }

    public RequestedSaveThrow? PassSaveThrow(string connectionId)
    {
        return _requestedSaveThrows.TryGetValue(connectionId, out var requestedSaveThrow)
            ? requestedSaveThrow
            : null;
    }
}