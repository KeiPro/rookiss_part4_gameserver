using System.Xml;

namespace PacketGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            using (XmlReader r = XmlReader.Create("PDL.xml", settings))
            {
                // XML파일에서 핵심 내용 부분으로 커서가 옮겨짐(헤더를 다 건너뛴다.)
                r.MoveToContent();

                while (r.Read())
                {
                    // XmlNodeType.Element : 꺽쇠가 시작되는 요소.
                    // 즉, '</' 로 끝나는 요소는 처리하지 않도록 한다. 끝나는 요소는 EndElement라고 따로 있음.
                    if (r.Depth == 1 && r.NodeType == XmlNodeType.Element)
                        ParsePacket(r);

                    //Console.WriteLine(r.Name + " " + r["name"]);
                }
            }
        }

        public static void ParsePacket(XmlReader r)
        {
            if (r.NodeType == XmlNodeType.EndElement)
                return;

            if (r.Name.ToLower() != "packet")
            {
                Console.WriteLine("Invalid packet node");
                return;
            }

            string packetName = r["name"];
            if (string.IsNullOrEmpty(packetName))
            {
                Console.WriteLine("Packet without name");
                return;
            }

            ParseMembers(r);
        }

        public static void ParseMembers(XmlReader r)
        {
            string packetName = r["name"];

            int depth = r.Depth + 1;
            while (r.Read())
            {
                if (r.Depth != depth)
                    break;

                string memberName = r["name"];
                if (string.IsNullOrEmpty(memberName))
                {
                    Console.WriteLine("Member without name");
                    return;
                }

                string memberType = r.Name.ToLower();
                switch (memberType)
                {
                    case "bool":
                    case "byte":
                    case "short":
                    case "ushort":
                    case "int":
                    case "long":
                    case "float":
                    case "double":
                    case "string":
                    case "list":
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
