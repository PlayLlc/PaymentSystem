using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Persistence.Grremlin
{

    public static class ValueTypes
    {
        private const string Byte = "g:Int8";
        private const string Int16 = "g:Int16";
        private const string Int32 = "g:Int32";
        private const string Int64 = "g:Int64";
        private const string Double = "g:Double";
        private const string Int32 = "g:Int32";
        private const string Int32 = "g:Int32";
        private const string Int32 = "g:Int32";
        private const string Int32 = "g:Int32";
        private const string Int32 = "g:Int32";
        private const string Int32 = "g:Int32";
        private const string Int32 = "g:Int32";

        /*
         *  case "system.string":
            return "'" + Sanitize(item as string) + "'";
        case "system.boolean":
            return (bool)item ? "true" : "false";
        case "system.single":
            return ((float)item).ToString();
        case "system.double":
            return ((double)item).ToString();
        case "system.decimal":
            return ((decimal)item).ToString();
        case "system.int32":
            return ((int)item).ToString();
        case "system.int64":
            return ((long)item).ToString();
        case "system.datetime":
            return "'" + Sanitize((item as DateTime?).Value.ToString("s")) + "'";
        default:
            return GetObjectString(item.ToString());
         */
    }


    public record Node
    {
        private readonly IEntity _Entity;


        // GraphSON https://tinkerpop.apache.org/docs/3.4.1/dev/io/#graphml
        public string Serialize()
        {
            var dto = _Entity.AsDto();
            throw new NotImplementedException();
        }
    }
    public record Edge
    {
        // Omnidirection
        private readonly IEntity _From;
        private readonly IEntity _To;

        // Label
        public string DomainEventType;

        // Relational Properties
        public readonly DateTimeUtc DateTimeUtc; 
        private readonly Dictionary < string, object > _Properties;

        public Edge(IEntity from, IEntity to)
        {
            _From = from;
            _To = to;
        }

        private void GetFromNodeProperties(){ }
        private void GetToNodeProperties(){ }

        // GraphSON https://tinkerpop.apache.org/docs/3.4.1/dev/io/#graphml
        public string Serialize() {
            throw new NotImplementedException();
        }

    }
}
