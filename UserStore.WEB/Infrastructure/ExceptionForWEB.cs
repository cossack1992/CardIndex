using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.WEB.Infrastructure
{
    [Serializable]
    public class ConvertWEBException : Exception
    {
        public ConvertWEBException() : base() { }
        public ConvertWEBException(string message) : base(message) { }
        public ConvertWEBException(string message, Exception inner) : base(message, inner) { }
        protected ConvertWEBException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
    [Serializable]
    public class SaveContentException : Exception
    {
        public SaveContentException() : base() { }
        public SaveContentException(string message) : base(message) { }
        public SaveContentException(string message, Exception inner) : base(message, inner) { }
        protected SaveContentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
