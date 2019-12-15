using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace RainbowMage.OverlayPlugin
{
    class NetworkParser
    {
        public static event EventHandler<OnlineStatusChangedArgs> OnOnlineStatusChanged;

        private static int ActorControl142_Size = 0;
        private static int MessageType_Offset = 0;
        private static int ActorID_Offset = 0;
        private static int Category_Offset = 0;
        private static int Param1_Offset = 0;

        /**
         * We use reflection to calculate the field offsets since there's no public Machina DLL we could link
         * against. I want to avoid copying the relevant structures so reflection is the last option left.
         * 
         * IMPORTANT: Remove the parsing code, once the parser parses the network packet and provides
         * an event or log line for it.
        */

        public static void Init()
        {
            try
            {
                var mach = Assembly.Load("Machina.FFXIV");
                var ActorControl142 = mach.GetType("Machina.FFXIV.Headers.Server_ActorControl142");
                ActorControl142_Size = Marshal.SizeOf(ActorControl142);

                var headerOffset = GetOffset(ActorControl142, "MessageHeader");
                var msgHeader = ActorControl142.GetField("MessageHeader").FieldType;

                MessageType_Offset = headerOffset + GetOffset(msgHeader, "MessageType");
                ActorID_Offset = headerOffset + GetOffset(msgHeader, "ActorID");

                Category_Offset = GetOffset(ActorControl142, "category");
                Param1_Offset = GetOffset(ActorControl142, "param1");

                FFXIVRepository.RegisterNetworkParser(Parse);
            } catch (Exception e)
            {
                Registry.Resolve<ILogger>().Log(LogLevel.Error, Resources.NetworkParserInitException, e);
            }
        }

        private static int GetOffset(Type type, string property)
        {
            int offset = 0;

            foreach (var prop in type.GetFields())
            {
                var customOffset = prop.GetCustomAttribute<FieldOffsetAttribute>();
                if (customOffset != null)
                {
                    offset = customOffset.Value;
                }

                if (prop.Name == property)
                {
                    break;
                }

                if (prop.FieldType.IsEnum)
                {
                    offset += Marshal.SizeOf(Enum.GetUnderlyingType(prop.FieldType));
                } else
                {
                    offset += Marshal.SizeOf(prop.FieldType);
                }
            }

            return offset;
        }

        public unsafe static void Parse(string id, long epoch, byte[] message)
        {
            if (message.Length >= ActorControl142_Size)
            {
                fixed (byte* buffer = message)
                {
                    /*
                    Server_ActorControl142* packet = (Server_ActorControl142*)buffer;

                    if (packet->MessageHeader.MessageType != Server_MessageType.ActorControl142) return;
                    if (packet->category != Server_ActorControlCategory.StatusUpdate) return;

                    OnOnlineStatusChanged?.Invoke(null, new OnlineStatusChangedArgs(packet->MessageHeader.ActorID, packet->param1));
                    */

                    if (*((ushort*)&buffer[MessageType_Offset]) != 0x12f) return;
                    if (*((UInt16*)&buffer[Category_Offset]) != 0x1f8) return;

                    OnOnlineStatusChanged?.Invoke(null, new OnlineStatusChangedArgs(*(uint*)&buffer[ActorID_Offset], *(uint*)&buffer[Param1_Offset]));
                }
            }
        }
    }

    public class OnlineStatusChangedArgs : EventArgs
    {
        public uint Target { get; private set; }
        public UInt32 Status { get; private set; }

        public OnlineStatusChangedArgs(uint target, UInt32 status)
        {
            this.Target = target;
            this.Status = status;
        }
    }
}
