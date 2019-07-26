﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TeleportCommand : Command
    {
        public TeleportCommand(Creature creature, Position toPosition)
        {
            Creature = creature;

            ToPosition = toPosition;
        }

        public Creature Creature { get; set; }

        public Position ToPosition { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile toTile = server.Map.GetTile(ToPosition);

            if (toTile != null)
            {
                Position toPosition = toTile.Position;

                Tile fromTile = Creature.Tile;
                
                Position fromPosition = fromTile.Position;

                byte fromIndex = fromTile.GetIndex(Creature);

                //Act

                fromTile.RemoveContent(fromIndex);

                byte toIndex = toTile.AddContent(Creature);

                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer == Creature)
                    {
                        int deltaZ = toPosition.Z - fromPosition.Z;

                        int deltaY = toPosition.Y - fromPosition.Y;

                        int deltaX = toPosition.X - fromPosition.X;

                        if (deltaZ < -1 || deltaZ > 1 || deltaY < -2 || deltaY > 2 || deltaX < -2 || deltaX > 2)
                        {
                            context.Write(observer.Client.Connection, new SendTilesOutgoingPacket(server.Map, observer.Client, toPosition) );
                        }
                        else
                        {
                            if (fromPosition.Z == 7 && ToPosition.Z == 8)
                            {
                                context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                            }
                            else
                            {
                                context.Write(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );
                            }

                            Position position = fromPosition;

                            while (deltaZ < 0)
                            {
                                context.Write(observer.Client.Connection, new SendMapUpOutgoingPacket(server.Map, observer.Client, position) );

                                position = position.Offset(0, 0, -1);

                                context.Write(observer.Client.Connection, new SendMapWestOutgoingPacket(server.Map, observer.Client, position.Offset(0, 1, 0) ) );

                                context.Write(observer.Client.Connection, new SendMapNorthOutgoingPacket(server.Map, observer.Client, position) );

                                deltaZ++;
                            }

                            while (deltaZ > 0)
                            {
                                context.Write(observer.Client.Connection, new SendMapDownOutgoingPacket(server.Map, observer.Client, position) );

                                position = position.Offset(0, 0, 1);

                                context.Write(observer.Client.Connection, new SendMapEastOutgoingPacket(server.Map, observer.Client, position.Offset(0, -1, 0) ) );
                                
                                context.Write(observer.Client.Connection, new SendMapSouthOutgoingPacket(server.Map, observer.Client, position) );

                                deltaZ--;
                            }

                            while (deltaY < 0)
                            {
                                position = position.Offset(0, -1, 0);

                                context.Write(observer.Client.Connection, new SendMapNorthOutgoingPacket(server.Map, observer.Client, position) );

                                deltaY++;
                            }

                            while (deltaY > 0)
                            {
                                position = position.Offset(0, 1, 0);

                                context.Write(observer.Client.Connection, new SendMapSouthOutgoingPacket(server.Map, observer.Client, position) );
                                
                                deltaY--;
                            }

                            while (deltaX < 0)
                            {
                                position = position.Offset(-1, 0, 0);

                                context.Write(observer.Client.Connection, new SendMapWestOutgoingPacket(server.Map, observer.Client, position) );

                                deltaX++;
                            }

                            while (deltaX > 0)
                            {
                                position = position.Offset(1, 0, 0);

                                context.Write(observer.Client.Connection, new SendMapEastOutgoingPacket(server.Map, observer.Client, position) );

                                deltaX--;
                            }
                        }

                        foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
                        {
                            if (pair.Value.GetRootContainer() is Tile tile)
                            {
                                if ( !tile.Position.IsNextTo(toPosition) )
                                {
                                    observer.Client.ContainerCollection.CloseContainer(pair.Key);

                                    context.Write(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
                                }
                            }
                        }
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(fromPosition) && observer.Tile.Position.CanSee(toPosition) )
                        {
                            context.Write(observer.Client.Connection, new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );
                        }
                        else if (observer.Tile.Position.CanSee(fromPosition) )
                        {
                            context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                        }
                        else if (observer.Tile.Position.CanSee(toPosition) )
                        {
                            uint removeId;

                            if (observer.Client.CreatureCollection.IsKnownCreature(Creature.Id, out removeId) )
                            {
                                context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, Creature) );
                            }
                            else
                            {
                                context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, removeId, Creature) );
                            }
                        }
                    }                    
                }

                base.Execute(server, context);
            }
        }
    }
}