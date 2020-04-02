using System;

namespace CumulusFramework.Domain.ValueObject
{
    public class MessageVO
    {
        public String Code { get; set; }
        public String Text { get; set; }
        public Boolean IsError { get; set; }
    }
}
