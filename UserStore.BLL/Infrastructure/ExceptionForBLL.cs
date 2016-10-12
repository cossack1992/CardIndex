using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.Infrastructure
{
    [Serializable]
    public class DataAccessException : Exception
    {
        public DataAccessException() : base() { }
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception inner) : base(message, inner) { }
        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
    [Serializable]
    public class ConvertDTOException : Exception
    {
        public ConvertDTOException() : base() { }
        public ConvertDTOException(string message) : base(message) { }
        public ConvertDTOException(string message, Exception inner) : base(message, inner) { }
        protected ConvertDTOException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
