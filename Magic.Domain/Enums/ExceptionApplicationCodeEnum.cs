namespace Magic.Domain.Enums;

public enum ExceptionApplicationCodeEnum
{
    /// <summary>
    /// Пользователь существует
    /// </summary>
    UserExist = 1,
    /// <summary>
    /// Пользователь не существует
    /// </summary>
    UserNotExist = 2,
    /// <summary>
    /// Пользователь плохой
    /// </summary>
    UserBanned = 3,
    /// <summary>
    /// Пароль плохой
    /// </summary>
    InvalidPassword = 4,
    /// <summary>
    /// Игровая сессия не найдена
    /// </summary>
    GameSessionNotFound = 5,
    /// <summary>
    /// Нельзя войти в комнату если вы ее создатель
    /// </summary>
    CreatorIdGameSessionEqualsUserIdToEnter = 6,
    /// <summary>
    /// Пользователь уже находится в этой игровой сессии
    /// </summary>
    UserInGameSession = 7,
    /// <summary>
    /// Ошибка доступа
    /// </summary>
    AccessError = 8,
    /// <summary>
    /// Игра запущена
    /// </summary>
    GameStarted = 9,
}