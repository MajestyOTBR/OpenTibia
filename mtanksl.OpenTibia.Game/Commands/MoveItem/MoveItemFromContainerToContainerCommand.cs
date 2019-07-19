﻿using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromContainerToContainerCommand : MoveItemCommand
    {
        public MoveItemFromContainerToContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, byte toContainerId, byte toContainerIndex, byte count)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

            if (fromItem != null)
            {
                Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                if (toContainer != null)
                {
                    //Act

                    RemoveItem(fromContainer, fromItem, server, context);

                    AddItem(toContainer, fromItem, server, context);
                }
            }
        }
    }
}