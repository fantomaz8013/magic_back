namespace Magic.Common.Models.Websocket;

public record ChatMessage(Guid MessageId, string UserLogin, string Message);