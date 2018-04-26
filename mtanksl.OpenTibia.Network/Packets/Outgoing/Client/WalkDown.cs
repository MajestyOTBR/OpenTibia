﻿using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class WalkDown : MapPacket
    {
        public WalkDown(TibiaGameClient client, Position fromPosition) : base(client)
        {
            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xBF );

            if (FromPosition.Z == 7)
            {
                MapDescription(writer, FromPosition.X - 8, FromPosition.Y - 6, FromPosition.Z, 18, 14, 8, 2);
            }
            else if (FromPosition.Z > 7)
            {
                MapDescription(writer, FromPosition.X - 8, FromPosition.Y - 6, FromPosition.Z, 18, 14, FromPosition.Z + 3, 0);
            }

            Position toPosition = FromPosition.Offset(0, 0, 1);

            if (toPosition.Z <= 7)
            {
                writer.Write( (byte)0x66 ); //East

                MapDescription(writer, toPosition.X + 9, toPosition.Y - 7, toPosition.Z, 1, 14, 7, -7);

                writer.Write( (byte)0x67 ); //South

                MapDescription(writer, toPosition.X - 8, toPosition.Y + 7, toPosition.Z, 18, 1, 7, -7);
            }
            else
            {
                writer.Write( (byte)0x66 ); //East

                MapDescription(writer, toPosition.X + 9, toPosition.Y - 7, toPosition.Z, 1, 14, toPosition.Z - 2, 4);

                writer.Write( (byte)0x67 ); //South

                MapDescription(writer, toPosition.X - 8, toPosition.Y + 7, toPosition.Z, 18, 1, toPosition.Z - 2, 4);
            }
        }
    }
}