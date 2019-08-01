﻿using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class StopFollowCommand : Command
    {
        public StopFollowCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override void Execute(Server server, Context context)
        {
            //Arrange

            if (Player.FollowTarget != null)
            {
                //Act

                Player.AttackTarget = null;

                Player.FollowTarget = null;

                //Notify

                context.Write(Player.Client.Connection, new StopAttackAndFollowOutgoingPacket(0) );
            }

            base.Execute(server, context);
        }
    }
}