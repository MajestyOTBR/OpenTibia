﻿using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class LogInCommand : Command
    {
        public LogInCommand(IConnection connection, SelectedCharacterIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public SelectedCharacterIncomingPacket Packet { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            if (Connection.Keys == null)
            {
                Connection.Keys = Packet.Keys;

                if (Packet.Version != 860)
                {
                    context.Write(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.OnlyProtocol86Allowed) );

                    context.Disconnect(Connection);
                }
                else
                {
                    var account = new PlayerRepository().GetPlayer(Packet.Account, Packet.Password, Packet.Character);

                    if (account == null)
                    {
                        context.Write(Connection, new OpenSorryDialogOutgoingPacket(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                        context.Disconnect(Connection);
                    }
                    else
                    {
                        Position toPosition = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ);

                        Tile toTile = server.Map.GetTile(toPosition);

                        if (toTile != null)
                        {
                            Player player = new Player()
                            {
                                Name = account.Name
                            };

                            Connection.Client = new Client(server)
                            {
                                Player = player
                            };

                            //Act

                            server.Map.AddCreature(player);

                            byte toIndex = toTile.AddContent(player);

                            //Notify

                            foreach (var observer in server.Map.GetPlayers() )
                            {
                                if (observer == player)
                                {
                                    context.Write(observer.Client.Connection, new SendInfoOutgoingPacket(player.Id, player.CanReportBugs), 

                                                                              new SetSpecialConditionOutgoingPacket(SpecialCondition.None),

                                                                              new SendStatusOutgoingPacket(player.Health, player.MaxHealth, 

                                                                                                           player.Capacity, 
                                                                                                          
                                                                                                           player.Experience, player.Level, player.LevelPercent, 
                                                                                                          
                                                                                                           player.Mana, player.MaxMana, 0, 0, player.Soul,
                                                                                                          
                                                                                                           player.Stamina),

                                                                              new SendSkillsOutgoingPacket(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0),

                                                                              new SetEnvironmentLightOutgoingPacket(server.Clock.Light),

                                                                              new SendTilesOutgoingPacket(server.Map, player.Client, toPosition),

                                                                              new ShowMagicEffectOutgoingPacket(toPosition, MagicEffectType.Teleport) );
                                }
                                else
                                {
                                    if (observer.Tile.Position.CanSee(toPosition) )
                                    {
                                        uint removeId;

                                        if (observer.Client.CreatureCollection.IsKnownCreature(player.Id, out removeId) )
                                        {
                                            context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, player),

                                                                                      new ShowMagicEffectOutgoingPacket(toPosition, MagicEffectType.Teleport) );
                                        }
                                        else
                                        {
                                            context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toPosition, toIndex, removeId, player),

                                                                                      new ShowMagicEffectOutgoingPacket(toPosition, MagicEffectType.Teleport) );
                                        }
                                    }
                                }                                
                            }

                            foreach (var component in player.GetComponents<Behaviour>() )
                            {
                                component.Start(server);
                            }

                            //Event

                            if (server.Events.Login != null)
                            {
                                server.Events.Login(this, new LoginEventArgs(player, toTile, toIndex, server, context) );
                            }

                            if (server.Events.TileAddCreature != null)
                            {
                                server.Events.TileAddCreature(this, new TileAddCreatureEventArgs(player, toTile, toIndex, server, context) );
                            }
                            
                            base.Execute(server, context);
                        }
                    }
                }
            }
        }
    }
}