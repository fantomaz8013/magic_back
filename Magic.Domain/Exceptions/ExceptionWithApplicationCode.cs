using Magic.Domain.Enums;

namespace Magic.Domain.Exceptions;

public class ExceptionWithApplicationCode : Exception
{
    public ExceptionWithApplicationCode(string message, ExceptionApplicationCodeEnum errorCode) : base(message)
    {
        ErrorCode = (int)errorCode;
    }

    public int ErrorCode { get; set; }
}