using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    List<Vector3> vertices;
    List<int> triangles;

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 position)
        {
            this.position = position;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 position, bool active, float squareSize) : base(position)
        {
            this.active = active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }

    public class Square
    {
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node centerTop, centerBottom, centerRight, centerLeft;
        public int configuration;

        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
            this.bottomRight = bottomRight;

            this.centerTop = topLeft.right;
            this.centerBottom = bottomLeft.right;
            this.centerRight = bottomRight.above;
            this.centerLeft = bottomLeft.above;

            if(topLeft.active) configuration += 8;
            if(topRight.active) configuration += 4;
            if(bottomRight.active) configuration += 2;
            if(bottomLeft.active) configuration += 1;

        }
    }

    public class SquareGrid {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }
            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x, y], controlNodes[x + 1, y]);
                }
            }
        }
    }
    void AssignVertices(Node[] points)
    {
        for(int i = 0; i < points.Length; i++)
        {

        }
    }
    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
    }

    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {

            case 0:
                break;

            // 1 Points
            case 1:
                MeshFromPoints(square.centerBottom, square.bottomLeft, square.centerLeft);
                break;
            case 2:
                MeshFromPoints(square.centerRight, square.bottomRight, square.centerBottom);
                break;
            case 4:
                MeshFromPoints(square.centerTop, square.topRight, square.centerLeft);
                break;
            case 8:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerLeft);
                break;

            // 2 Points
            case 3:
                MeshFromPoints(square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                break;
            case 5:
                MeshFromPoints(square.centerTop, square.topRight, square.centerRight, square.centerBottom);
                break;
            case 6:
                MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.centerBottom,square.bottomLeft, square.centerLeft);
                break;
            case 9:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
                break;
            case 10:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight,square.centerBottom,square.centerLeft);
                break;
            case 12:
                MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerLeft);
                break;

            // 3 Points
            case 7:
                MeshFromPoints(square.centerTop, square.topRight, square.bottomLeft, square.bottomRight, square.centerLeft);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
                break;
            // 4 Points
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomRight, square.bottomLeft);
                break;
        }
    }

   
    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (squareGrid != null)
        {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    Gizmos.color = (squareGrid.squares[x, y].topLeft.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.position, Vector3.one * .4f);

                    Gizmos.color = (squareGrid.squares[x, y].topRight.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topRight.position, Vector3.one * .4f);

                    Gizmos.color = (squareGrid.squares[x, y].bottomLeft.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.position, Vector3.one * .4f);

                    Gizmos.color = (squareGrid.squares[x, y].bottomRight.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.position, Vector3.one * .4f);

                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerTop.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerRight.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerBottom.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centerLeft.position, Vector3.one * .15f);
                }
            }
        }
    }
}
