﻿using System;

namespace OpenTibia.Common.Structures
{
    public class Position
    {
        public Position(int x, int y, int z)
        {
            this.x = (ushort)x;

            this.y = (ushort)y;

            this.z = (byte)z;
        }

        private ushort x;

        public ushort X
        {
            get
            {
                return x;
            }
        }

        private ushort y;

        public ushort Y
        {
            get
            {
                return y;
            }
        }

        private byte z;

        public byte Z
        {
            get
            {
                return z;
            }
        }

        public bool IsInventory
        {
            get
            {
                if (x == 65535)
                {
                    if (y < 64)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        
        public byte InventoryIndex
        {
            get
            {
                return (byte)(y);
            }
        }

        public bool IsContainer
        {
            get
            {
                if (x == 65535)
                {
                    if (y >= 64)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public byte ContainerId
        {
            get
            {
                return (byte)(y - 64);
            }
        }

        public byte ContainerIndex
        {
            get
            {
                return (byte)(z);
            }
        }

        public Position Offset(int x, int y, int z)
        {
            return new Position(X + x, Y + y, Z + z);
        }

        public Position Offset(Direction direction)
        {
            switch (direction)
            {
                case Direction.East:

                    return new Position(this.X + 1, this.Y, this.Z);

                case Direction.North:

                    return new Position(this.X, this.Y - 1, this.Z);

                case Direction.West:

                    return new Position(this.X - 1, this.Y, this.Z);

                default:

                    return new Position(this.X, this.Y + 1, this.Z);
            }
        }

        public Position Offset(MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case MoveDirection.East:

                    return new Position(this.X + 1, this.Y, this.Z);

                case MoveDirection.NorthEast:

                    return new Position(this.X + 1, this.Y - 1, this.Z);

                case MoveDirection.North:

                    return new Position(this.X, this.Y - 1, this.Z);

                case MoveDirection.NorthWest:

                    return new Position(this.X - 1, this.Y - 1, this.Z);

                case MoveDirection.West:

                    return new Position(this.X - 1, this.Y, this.Z);

                case MoveDirection.SouthWest:

                    return new Position(this.X - 1, this.Y + 1, this.Z);

                case MoveDirection.South:

                    return new Position(this.X, this.Y + 1, this.Z);

                default:

                    return new Position(this.X + 1, this.Y + 1, this.Z);
            }
        }
        
        public Direction ToDirection(Position that)
        {
            int deltaY = that.y - this.y;

            int deltaX = that.x - this.x;

            if (deltaX < 0)
            {
                return Direction.West;
            }
            else if (deltaX == 0)
            {
                if (deltaY < 0)
                {
                    return Direction.North;
                }
                else if (deltaY > 0)
                {
                    return Direction.South;
                }
            }
            else if (deltaX > 0)
            {
                return Direction.East;
            }

            return Direction.South;
        }

        public MoveDirection ToMoveDirection(Position that)
        {
            int deltaY = that.y - this.y;

            int deltaX = that.x - this.x;

            if (deltaY < 0)
            {
                if (deltaX < 0)
                {
                    return MoveDirection.NorthWest;
                }
                else if (deltaX == 0)
                {
                    return MoveDirection.North;
                }
                else if (deltaX > 0)
                {
                    return MoveDirection.NorthEast;
                }
            }
            else if (deltaY == 0)
            {
                if (deltaX < 0)
                {
                    return MoveDirection.West;
                }
                else if (deltaX > 0)
                {
                    return MoveDirection.East;
                }
            }
            else if (deltaY > 0)
            {
                if (deltaX < 0)
                {
                    return MoveDirection.SouthWest;
                }
                else if (deltaX == 0)
                {
                    return MoveDirection.South;
                }
                else if (deltaX > 0)
                {
                    return MoveDirection.SouthEast;
                }
            }

            return MoveDirection.South;
        }
         
        public bool IsInClientRange(Position that)
        {
            throw new NotImplementedException();
        }

        public bool IsInPlayerRange(Position that)
        {
            int deltaZ = that.z - this.z;

            if (deltaZ != 0)
            {
                return false;
            }

            int deltaY = that.y - this.y;

            if (deltaY < -6 || deltaY > 7)
            {
                return false;
            }

            int deltaX = that.x - this.x;

            if (deltaX < -8 || deltaX > 9)
            {
                return false;
            }

            return true;
        }

        public bool IsNextTo(Position that)
        {
            int deltaZ = that.z - this.z;

            if (deltaZ != 0)
            {
                return false;
            }

            int deltaY = that.y - this.y;

            if (deltaY < -1 || deltaY > 1)
            {
                return false;
            }

            int deltaX = that.x - this.x;

            if (deltaX < -1 || deltaX > 1)
            {
                return false;
            }

            return true;
        }

        public bool CanSee(Position that)
        {
            int deltaZ = that.z - this.z;

            int deltaY = that.y - this.y + deltaZ;

            int deltaX = that.x - this.x + deltaZ;

            if (this.z >= 8)
            {
                if (deltaZ < -2 || deltaZ > 2)
                {
                    return false;
                }
            }

            if (this.z <= 7) 
            {
                if (that.z >= 8)
                {
                    return false;
                }
            }

            if (deltaX < -8 || deltaX > 9 || deltaY <-6 || deltaY > 7)
            {
                return false;
            }

            return true;
        }

        public bool CanHearSay(Position that)
        {
            return IsInPlayerRange(that);
        }

        public bool CanHearWhisper(Position that)
        {
            return IsNextTo(that);
        }

        public bool CanHearYell(Position that)
        {
            int deltaZ = that.z - this.z;

            int deltaY = that.y - this.y + deltaZ;

            int deltaX = that.x - this.x + deltaZ;

            if (this.z >= 8 || that.z >= 8)
            {
                if (deltaZ != 0)
                {
                    return false;
                }
            }

            if (deltaX < -30 || deltaX > 30 || deltaY < -30 || deltaY > 30)
            {
                return false;
            }

            return true;
        }

        public static bool operator ==(Position a, Position b)
        {
            if ( object.ReferenceEquals(a, b) )
            {
                return true;
            }

            if ( object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null) )
            {
                return false;
            }

            return (a.x == b.x) && (a.y == b.y) && (a.z == b.z);
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public override bool Equals(object position)
        {
            return Equals(position as Position);
        }

        public bool Equals(Position position)
        {
            if (position == null)
            {
                return false;
            }

            return (x == position.x) && (y == position.y) && (z == position.z);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 23 + x.GetHashCode();

            hashCode = hashCode * 23 + y.GetHashCode();

            hashCode = hashCode * 23 + z.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            if (IsInventory)
            {
                return "Slot index: " + InventoryIndex;
            }

            if (IsContainer)
            {
                return "Container id: " + ContainerId + " index: " + ContainerIndex;
            }

            return "Coordinate x: " + x + " y: " + y + " z: " + z;
        }
    }
}