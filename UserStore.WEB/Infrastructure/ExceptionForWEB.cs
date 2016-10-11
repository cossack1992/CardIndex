using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.WEB.Infrastructure
{
    
    public class ConvertWEBException : Exception
    {
        public ConvertWEBException() : base() { }
        public ConvertWEBException(string message) : base(message) { }
        public ConvertWEBException(string message, Exception inner) : base(message, inner) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
    public class SaveContentException : Exception
    {
        public SaveContentException() : base() { }
        public SaveContentException(string message) : base(message) { }
        public SaveContentException(string message, Exception inner) : base(message, inner) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
