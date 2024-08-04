using Magic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain.Exceptions
{
    public class ExceptionWithApplicationCode : Exception
    {
        public ExceptionWithApplicationCode(string message, ExceptionApplicationCodeEnum errorCode) : base(message)
        {
            ErrorCode = (int)errorCode;
        }

        public int ErrorCode { get; set; }
    }
}
