using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.Infrastructure
{
    public class DataAcssessException : Exception
    {
        public DataAcssessException() : base() { }
        public DataAcssessException(string message) : base(message) { }
        public DataAcssessException(string message, Exception inner) : base(message, inner) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
    public class ConvertDTOException : Exception
    {
        public ConvertDTOException() : base() { }
        public ConvertDTOException(string message) : base(message) { }
        public ConvertDTOException(string message, Exception inner) : base(message, inner) { }
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
