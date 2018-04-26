﻿using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class EnvironmentLight : IOutgoingPacket
    {
        public EnvironmentLight(Light light)
        {
            this.Light = light;
        }

        public Light Light { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x82 );

            writer.Write(Light);
        }
    }
}