using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using System;
using System.Linq;
namespace Match3G_PlayerData
{
    public enum TileState
    {
        None, Freezed, A, B, C, D, E, F, G
    }
    public class Match3G_GroupInfo
    {
        public const int TileStateStart = (int)TileState.A;
        private static Match3G_Manager game;
        public static Match3G_Manager Game 
        { 
            get 
            { 
                if (game == null) 
                    game = GameObject.FindObjectOfType<Match3G_Manager>();
                return game; } 
        }
        private static bool showMask = false;
        public static bool ShowMask
        {
            get
            {
                return showMask;
            }
            set
            {
                if(showMask == value)return;
                showMask = value;
                Game.Mask.enabled = showMask;
            }
        }
        public static GroupType groupTurn;
        public enum GroupType
        {
            GroupA,
            GroupB,
            NotReady
        }
    }
    public enum MoveDirection
    {
        None, Up, Right, Down, Left
    }
#region 过时
    // public struct Grid2D<T>
    // {
    //     T[] cells;
    //     int2 size;
    //     public T this[int x, int y]
    //     {
    //         get => cells[y * size.x + x];
    //         set => cells[y * size.x + x] = value;
    //     }
    //     public T this[int2 c]
    //     {
    //         get => cells[c.y * size.x + c.x];
    //         set => cells[c.y * size.x + c.x] = value;
    //     }
        
    //     public int2 Size => size;
    //     public int SizeX => size.x;
    //     public int SizeY => size.y;
    //     public bool IsUndefined => cells == null || cells.Length == 0;
       
    //     public Grid2D (int2 size)
    //     {
    //         this.size = size;
    //         cells = new T[size.x * size.y];
    //     }
    //     public bool AreValidCoordinates (int2 c) =>
    //         0 <= c.x && c.x < size.x && 0 <= c.y && c.y < size.y;
    //     public void Swap (int2 a, int2 b) => (this[a], this[b]) = (this[b], this[a]);
        
    // }
    // public struct Grid2D<T>
    // {
    //     List<T> cells;
    //     int2 size;

    //     public T this[int x, int y]
    //     {
    //         get => cells[y * size.x + x];
    //         set => cells[y * size.x + x] = value;
    //     }

    //     public T this[int2 c]
    //     {
    //         get => cells[c.y * size.x + c.x];
    //         set => cells[c.y * size.x + c.x] = value;
    //     }

    //     public int2 Size => size;
    //     public int SizeX => size.x;
    //     public int SizeY => size.y;
    //     public bool IsUndefined => cells == null || cells.Count == 0;

    //     public Grid2D(int2 size)
    //     {
    //         this.size = size;
    //         cells = new List<T>(size.x * size.y);
    //         for (int i = 0; i < size.x * size.y; i++)
    //         {
    //             cells.Add(default(T));
    //         }
    //     }

    //     public bool AreValidCoordinates(int2 c) =>
    //         0 <= c.x && c.x < size.x && 0 <= c.y && c.y < size.y;

    //     public void Swap(int2 a, int2 b)
    //     {
    //         int indexA = a.y * size.x + a.x;
    //         int indexB = b.y * size.x + b.x;
    //         T temp = cells[indexA];
    //         cells[indexA] = cells[indexB];
    //         cells[indexB] = temp;
    //     }
    // }
#endregion 过时
    public struct Grid2D<T>
    {
        List<List<T>> cells;
        int2 size;

        public T this[int x, int y]
        {
            get => cells[y][x];
            set => cells[y][x] = value;
        }

        public T this[int2 c]
        {
            get => cells[c.y][c.x];
            set => cells[c.y][c.x] = value;
        }

        public int2 Size => size;
        public int SizeX => size.x;
        public int SizeY => size.y;
        public bool IsUndefined => cells == null || cells.Count == 0;

        public Grid2D(int2 size)
        {
            this.size = size;
            cells = new List<List<T>>(size.y);
            for (int y = 0; y < size.y; y++)
            {
                cells.Add(new List<T>(size.x));
                for (int x = 0; x < size.x; x++)
                {
                    cells[y].Add(default(T));
                }
            }
        }

        public bool AreValidCoordinates(int2 c) =>
            0 <= c.x && c.x < size.x && 0 <= c.y && c.y < size.y;

        public void Swap(int2 a, int2 b)
        {
            T temp = cells[a.y][a.x];
            cells[a.y][a.x] = cells[b.y][b.x];
            cells[b.y][b.x] = temp;
        }

        public void AddCell(int2 c, T data)
        {
            if (!AreValidCoordinates(c))
            {
                throw new System.IndexOutOfRangeException("Coordinates are out of range.");
            }

            cells[c.y].Insert(c.x, data);
            size.x++;
        }

        public void RemoveCell(int2 c)
        {
            if (!AreValidCoordinates(c))
            {
                throw new System.IndexOutOfRangeException("Coordinates are out of range.");
            }

            cells[c.y].RemoveAt(c.x);
            size.x--;
        }
        public void CreateAdditionalCellInColumn(int column, T data)
        {
            if (column < 0 || column >= size.x)
            {
                throw new System.IndexOutOfRangeException("Invalid column index.");
            }

            for (int y = 0; y < size.y; y++)
            {
                cells[y].Insert(column, data);
            }
            size.x++;
        }
        public void CreateAdditionalCellInRow(int row, T data)
        {
            if (row < 0 || row >= size.y)
            {
                throw new System.IndexOutOfRangeException("Invalid column index.");
            }

            for (int x = 0; x < size.x; x++)
            {
                cells[x].Insert(row, data);
            }
            size.y++;
        }
    }
   
    public struct Move
    {
        public MoveDirection Direction
	    { get; private set; }
    

	    public int2 From
	    { get; private set; }

	    public int2 To
	    { get; private set; }

	    public bool IsValid => Direction != MoveDirection.None;
        

	    public Move (int2 coordinates, MoveDirection direction)
        {
            Direction = direction;
            From = coordinates;
            To = coordinates + direction switch
            {
                MoveDirection.Up => int2(0, 1),
                MoveDirection.Right => int2(1, 0),
                MoveDirection.Down => int2(0, -1),
                _ => int2(-1, 0)
            };
        }
        
	    public static Move FindMove (Match3G_Group game)
        {
            int2 s = game.size;
            
            for (int2 c = 0; c.y < s.y; c.y++)
            {
                for (c.x = 0; c.x < s.x; c.x++)
                {
                    TileState t = game[c];

                    if (c.x >= 3 && game[c.x - 2, c.y] == t && game[c.x - 3, c.y] == t)
                    {
                        return new Move(c, MoveDirection.Left);
                    }

                    if (c.x + 3 < s.x && game[c.x + 2, c.y] == t && game[c.x + 3, c.y] == t)
                    {
                        return new Move(c, MoveDirection.Right);
                    }

                    if (c.y >= 3 && game[c.x, c.y - 2] == t && game[c.x, c.y - 3] == t)
                    {
                        return new Move(c, MoveDirection.Down);
                    }

                    if (c.y + 3 < s.y && game[c.x, c.y + 2] == t && game[c.x, c.y + 3] == t)
                    {
                        return new Move(c, MoveDirection.Up);
                    }

                    if (c.y > 1)
                    {
                        if (c.x > 1 && game[c.x - 1, c.y - 1] == t)
                        {
                            if (
                                c.x >= 2 && game[c.x - 2, c.y - 1] == t ||
                                c.x + 1 < s.x && game[c.x + 1, c.y - 1] == t
                            )
                            {
                                return new Move(c, MoveDirection.Down);
                            }
                            if (
                                c.y >= 2 && game[c.x - 1, c.y - 2] == t ||
                                c.y + 1 < s.y && game[c.x - 1, c.y + 1] == t
                            )
                            {
                                return new Move(c, MoveDirection.Left);
                            }
                        }

                        if (c.x + 1 < s.x && game[c.x + 1, c.y - 1] == t)
                        {
                            if (c.x + 2 < s.x && game[c.x + 2, c.y - 1] == t)
                            {
                                return new Move(c, MoveDirection.Down);
                            }
                            if (
                                c.y >= 2 && game[c.x + 1, c.y - 2] == t ||
                                c.y + 1 < s.y && game[c.x + 1, c.y + 1] == t
                            )
                            {
                                return new Move(c, MoveDirection.Right);
                            }
                        }
                    }

                    if (c.y + 1 < s.y)
                    {
                        if (c.x > 1 && game[c.x - 1, c.y + 1] == t)
                        {
                            if (
                                c.x >= 2 && game[c.x - 2, c.y + 1] == t ||
                                c.x + 1 < s.x && game[c.x + 1, c.y + 1] == t
                            )	
                            {
                                return new Move(c, MoveDirection.Up);
                            }
                            if (c.y + 2 < s.y && game[c.x - 1, c.y + 2] == t)
                            {
                                return new Move(c, MoveDirection.Left);
                            }
                        }

                        if (c.x + 1 < s.x && game[c.x + 1, c.y + 1] == t)
                        {
                            if (c.x + 2 < s.x && game[c.x + 2, c.y + 1] == t)
                            {
                                return new Move(c, MoveDirection.Up);
                            }
                            if (c.y + 2 < s.y && game[c.x + 1, c.y + 2] == t)
                            {
                                return new Move(c, MoveDirection.Right);
                            }
                        }
                    }
                }
            }

            return default;
        
        }
        
    }
    public struct Match
    {
        public int2 coordinates;
        public int length;
        public bool isHorizontal;
        public Match (int x, int y, int length, bool isHorizontal)
        {
            coordinates.x = x;
            coordinates.y = y;
            this.length = length;
            this.isHorizontal = isHorizontal;
        }
    }
     [System.Serializable]
    public class TileSwapper
    {
        [SerializeField, Range(0.1f, 10f)]
        float duration = 0.25f;

        [SerializeField, Range(0f, 1f)]
        float maxDepthOffset = 0.5f;

        Match3G_Unit tileA, tileB;

        Vector3 positionA, positionB;

        float progress = -1f;

        bool pingPong;

        public float Swap (Match3G_Unit a, Match3G_Unit b, bool pingPong)
        {
            tileA = a;
            tileB = b;
            positionA = a.transform.localPosition;
            positionB = b.transform.localPosition;
            this.pingPong = pingPong;
            progress = 0f;
            return pingPong ? 2f * duration : duration;
        }

        public void Update ()
        {
            if (progress < 0f)return;

            progress += Time.deltaTime;
            if (progress >= duration)
            {
                if (pingPong)
                {
                    progress -= duration;
                    pingPong = false;
                    (tileA, tileB) = (tileB, tileA);
                }
                else
                {
                    progress = -1f;
                    tileA.transform.localPosition = positionB;
                    tileB.transform.localPosition = positionA;
                    return;
                }
            }

            float t = progress / duration;
            float z = Mathf.Sin(Mathf.PI * t) * maxDepthOffset;
            Vector3 p = Vector3.Lerp(positionA, positionB, t);
            p.z = -z;
            tileA.transform.localPosition = p;
            p = Vector3.Lerp(positionA, positionB, 1f - t);
            p.z = z;
            tileB.transform.localPosition = p;
        }
    }
    [System.Serializable]
    public struct TileDrop
    {
        public int2 coordinates;

        public int fromY;

        public TileDrop (int x, int y, int distance)
        {
            coordinates.x = x;
            coordinates.y = y;
            fromY = y + distance;
        }
        
    }
    [System.Serializable]
    public struct SingleEnegy
    {
        public Vector3 positionFrom;
        public Vector3 positionTo;

        public int value;
        public Color color;
    }
}
