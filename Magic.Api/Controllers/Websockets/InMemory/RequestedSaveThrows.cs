using System.Collections.Concurrent;
using Magic.Api.Controllers.Websockets.Requests;

namespace Magic.Api.Controllers.Websockets.InMemory;

public class RequestedSaveThrows
{
    // connectionId -> RequestedSaveThrow
    // todo gameSession -> connection -> saveThrow to get ALL throws for session
    private readonly ConcurrentDictionary<string, RequestedSaveThrow> _requestedSaveThrows = new();

    public void RequestSaveThrow(string connectionId, RequestedSaveThrow requestedSaveThrow)
    {
        _requestedSaveThrows.AddOrUpdate(
            connectionId,
            _ => requestedSaveThrow,
            (_, __) => requestedSaveThrow
        );
    }

    public RequestedSaveThrow? GetSaveThrows(string connectionId)
    {
        return _requestedSaveThrows.TryGetValue(connectionId, out var requestedSaveThrow)
            ? requestedSaveThrow
            : null;
    }

    public RequestedSaveThrow? PassSaveThrow(string connectionId)
    {
        return _requestedSaveThrows.TryGetValue(connectionId, out var requestedSaveThrow)
            ? requestedSaveThrow
            : null;
    }
}