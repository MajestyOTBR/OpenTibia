﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromTileToContainerCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort fromItemId, byte toContainerId, byte toContainerIndex, ushort toItemId) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            FromItemId = fromItemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToItemId = toItemId;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort FromItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            //Act

                            if ( IsUseable(fromItem, server, context) &&
                                
                                IsNextTo(fromTile, server, context) )
                            {
                                UseItemWithItem(fromItem, toItem, server, context);
                            }                            
                        }
                    }
                }
            }
        }
    }
}