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
    /// <summary>
    /// Шаблон персонажа не существует
    /// </summary>
    CharacterTemplateNotExist = 10,
    /// <summary>
    /// Игровая сессия имеет неверный статус
    /// </summary>
    GameSessionIncorrectStatus = 11,
    /// <summary>
    /// Игровой персонаж не найден
    /// </summary>
    GameSessionCharacterNotFound = 12,
    /// <summary>
    /// Карта не найдена
    /// </summary>
    MapNotExist = 13 ,
    /// <summary>
    /// Параметры тайла не найдены
    /// </summary>
    TilePropertyNotExist = 14,
    /// <summary>
    /// Игровой персонаж не размещен на карте
    /// </summary>
    CharacterNotInMap = 15,
    /// <summary>
    /// Путь пустой
    /// </summary>
    PathIsEmpty = 16,
    /// <summary>
    /// Не найдена информация о ходе игрового персонажа
    /// </summary>
    TunfInfoNotExist = 17,
}