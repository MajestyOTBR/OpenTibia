﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.Scripts
{
    public class RopeScript : IItemUseWithItemScript
    {
        private static HashSet<ushort> ropes = new HashSet<ushort>() { 2120 };

        private static HashSet<ushort> ropeSpots = new HashSet<ushort> { 384 };

        public void Register(Server server)
        {
            foreach (var openTibiaId in ropes)
            {
                server.ItemUseWithItemScripts.Add(openTibiaId, this);
            }
        }

        public bool NextTo
        {
            get
            {
                return true;
            }
        }

        public bool Execute(Player player, Item fromItem, Item toItem, Server server, CommandContext context)
        {
            if (ropeSpots.Contains(toItem.Metadata.OpenTibiaId) )
            {
                Tile toTile = server.Map.GetTile( ( (Tile)toItem.Container ).Position.Offset(0, 1, -1) );

                if (toTile != null)
                {
                    server.CancelQueueForExecution(Constants.PlayerSchedulerEvent(player) );

                    new CreatureMoveCommand(player, toTile).Execute(server, context);

                    return true;
                }
            }   

            return false;
        }
    }
}