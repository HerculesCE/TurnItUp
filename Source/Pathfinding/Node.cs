﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Pathfinding
{
    public class Node
    {
        public IBoard Board { get; private set; }
        public Position Position { get; private set; }
        public Node Parent { get; set; }
        public int H { get; set; }
        private int _g;

        public Node(IBoard board, int x, int y, Node parent = null)
        {
            Board = board;
            Position = new Position(x, y);
            Parent = parent;
        }

        public int G
        {
            get
            {
                if (Parent == null)
                {
                    return _g;
                }
                else
                {
                    if (Position.X == Parent.Position.X || Position.Y == Parent.Position.Y)
                    {
                        return Parent.G + 10;
                    }
                    else
                    {
                        return Parent.G + 14;
                    }
                }
            }

            set
            {
                _g = value;
            }
        }

        public int F
        {
            get
            {
                return G + H;
            }
        }

        public void CalculateH(int endingX, int endingY)
        {
            H = (Math.Abs(endingX - Position.X) + Math.Abs(endingY - Position.Y)) * 10;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;

            return this == (Node)obj;
        }

        public override int GetHashCode()
        {
            return Position.X.GetHashCode() ^ Position.Y.GetHashCode();
        }

        public static bool operator ==(Node node1, Node node2)
        {
            if (object.ReferenceEquals(node1, null))
            {
                return object.ReferenceEquals(node2, null);
            }
            if (object.ReferenceEquals(node2, null))
            {
                return object.ReferenceEquals(node1, null);
            }

            return node1.Position == node2.Position;
        }

        public static bool operator !=(Node node1, Node node2)
        {
            return !(node1 == node2);
        }

        public List<Node> GetAdjacentNodes(bool allowDiagonalNodes = true)
        {
            List<Node> returnValue = new List<Node>();

            for (int x = Position.X - 1; x <= Position.X + 1; x++)
            {
                for (int y = Position.Y - 1; y <= Position.Y + 1; y++)
                {
                    if (x == Position.X && y == Position.Y) continue;

                    returnValue.Add(new Node(Board, x, y));
                }
            }

            // Remove nodes that are diagonal if we only want to find orthogonal nodes. 
            // This is used when the PathFinder is not allowed to use diagonal movement.
            if (!allowDiagonalNodes)
            {
                returnValue = returnValue.FindAll(n => n.IsOrthogonalTo(this));
            }

            return returnValue.FindAll(n => n.IsWithinBounds());
        }

        public bool IsWithinBounds()
        {
            return (Position.X >= 0 && Position.X <= (Board.Map.Width - 1) &&
                    Position.Y >= 0 && Position.Y <= (Board.Map.Height - 1));
        }

        public bool IsWalkable()
        {
            return (IsWithinBounds() && !Board.IsObstacle(Position.X, Position.Y)) && !Board.CharacterManager.IsCharacterAt(Position.X, Position.Y);
        }

        public bool IsOrthogonalTo(Node other)
        {
            return (Position.X == other.Position.X || Position.Y == other.Position.Y);
        }
    }
}
