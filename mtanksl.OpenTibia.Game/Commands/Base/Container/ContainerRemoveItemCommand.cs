﻿using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ContainerRemoveItemCommand : Command
    {
        public ContainerRemoveItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            byte index = Container.GetIndex(Item);

            //Act

            Container.RemoveContent(index);

            //Notify

            foreach (var observer in Container.GetPlayers() )
            {
                foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                {
                    if (pair.Value == Container)
                    {
                        context.Write(observer.Client.Connection, new ContainerRemoveOutgoingPacket(pair.Key, index) );
                    }
                }
            }

            base.Execute(server, context);
        }
    }
}