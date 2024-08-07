using System.Collections.Concurrent;

namespace Magic.Api.Controllers.Websockets;

public class LockedCharacters
{
    // gameSessionId -> dic<userId, lockedCharacterTemplateId>
    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, Guid>> _connections = new();

    public void Lock(Guid gameSessionId, Guid userId, Guid lockedCharacterTemplateId)
    {
        _connections.AddOrUpdate(
            gameSessionId,
            key =>
            {
                var dic = new ConcurrentDictionary<Guid, Guid>();
                dic.TryAdd(userId, lockedCharacterTemplateId);
                return dic;
            },
            (key, oldValue) =>
            {
                oldValue.AddOrUpdate(userId,
                    _ => lockedCharacterTemplateId,
                    (_, __) => lockedCharacterTemplateId
                );
                return oldValue;
            });
    }

    public void Unlock(Guid gameSessionId, Guid userId)
    {
        if (_connections.TryGetValue(gameSessionId, out var lockedCharacterTemplateIds))
            lockedCharacterTemplateIds.TryRemove(userId, out _);
    }

    public Dictionary<Guid, Guid>? GetGameSessionLocks(Guid gameSessionId)
    {
        return _connections.TryGetValue(gameSessionId, out var connectedUsers)
            ? connectedUsers.ToDictionary()
            : null;
    }

    public bool IsCharacterLocked(Guid gameSessionId, Guid characterId)
    {
        return _connections.TryGetValue(gameSessionId, out var connectedUsers)
               && connectedUsers.Values.Contains(characterId);
    }
}