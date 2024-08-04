namespace Magic.Domain.Enums
{
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
    }
}
