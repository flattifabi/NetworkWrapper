using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Core.Packet
{
    public class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteString(string message)
        {
            var messageLength = message.Length;
            byte[] buffer = BitConverter.GetBytes(messageLength);
            _ms.Write(BitConverter.GetBytes(messageLength), 0, buffer.Length);
            _ms.Write(Encoding.ASCII.GetBytes(message), 0, messageLength);
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
