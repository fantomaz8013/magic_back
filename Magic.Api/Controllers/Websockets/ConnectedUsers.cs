using System.Collections.Concurrent;

namespace Magic.Api.Controllers.Websockets;

public class ConnectedUsers
{
    // gameSessionId -> dic<userId, connectionId>
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, string>> _connections = new();

    public void Connect(Guid gameSessionId, Guid userId, string connectionId)
    {
        _connections.AddOrUpdate(
            gameSessionId,
            key =>
            {
                var dic = new ConcurrentDictionary<Guid, string>();
                dic.TryAdd(userId, connectionId);
                return dic;
            },
            (key, oldValue) =>
            {
                oldValue.TryAdd(userId, connectionId);
                return oldValue;
            });
    }

    public void Disconnect(Guid gameSessionId, Guid userId)
    {
        if (_connections.TryGetValue(gameSessionId, out var connectedUsers))
            connectedUsers.TryRemove(userId, out _);
    }

    public bool IsConnected(Guid gameSessionId, Guid userId)
    {
        return _connections.TryGetValue(gameSessionId, out var connectedUsers)
               && connectedUsers.TryGetValue(userId, out _);
    }

    public Guid? GetGameSessionId(string connectionId)
    {
        var gameSessionIds = _connections
            .Where(c => c.Value.Values.Contains(connectionId))
            .ToList();
        return gameSessionIds.Count == 0 ? null : gameSessionIds[0].Key;
    }
}