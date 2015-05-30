using System;
using System.Text;

namespace PowIoC 
{
    public class PrimitiveMapNotDeclareException : Exception //, ISerializable
    {
        const string context = "PrimitiveMapNotDeclareException";
        const string formatOne = " {0}";
        const string formatTwo = " {0}, {1}";
        string message = "";

        public PrimitiveMapNotDeclareException()
        {
            StringBuilder sb = new StringBuilder(context);
            message = sb.ToString();
        }

        public PrimitiveMapNotDeclareException(string message)
        {
            StringBuilder sb = new StringBuilder(context);
            sb.AppendFormat(formatOne, message);
            message = sb.ToString();
        }
        public PrimitiveMapNotDeclareException(string message, Exception inner)
        {
            StringBuilder sb = new StringBuilder(context);
            sb.AppendFormat(formatTwo, message, inner);
            message = sb.ToString();
        }

        public override string ToString () {
            return message;
        }
    }
}