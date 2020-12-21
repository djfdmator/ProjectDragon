// ==============================================================
// Cracked t_Grid
//
//  AUTHOR: Yang SeEun
// CREATED:
// UPDATED: 2020-01-01
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_Grid : MonoBehaviour
{
    public bool displayGridGizmos = false;
    public LayerMask wallMask;
    public LayerMask objectMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

  

    public t_Node[,] gridNode;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }


    private void Awake()
    {
        //wallMask = LayerMask.GetMask("Wall");
        objectMask = LayerMask.GetMask("Object");
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    }

    private void Start()
    {
        CreateGrid();
    }


    public void CreateGrid()
    {
        gridNode = new t_Node[gridSizeX, gridSizeY];
        Vector3 BottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = BottonLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = true;

                if (Physics2D.OverlapBox(worldPoint, new Vector2(nodeDiameter, nodeDiameter), 0.0f, wallMask))
                {
                    walkable = false;
                }

                gridNode[x, y] = new t_Node(walkable, worldPoint, x, y);

                if (Physics2D.OverlapBox(worldPoint, new Vector2(nodeDiameter, nodeDiameter), 0.0f, objectMask))
                {
                    gridNode[x, y].IsObject = true;
                }
            }
        }
    }
    //Object 파괴시 FinalPath를 재탐색한다.
    public void RescanPath(Collider2D _collider)
    {
        //collider에 있는 노드를 읽어온다.
        NodeFromWorldPosition(_collider.transform.position).IsObject = false;

        //콜라이더 종류에 따른 오버랩된 노드 계산
        int nodeOverlapCountX = 0, nodeOverlapCountY = 0;

        if (_collider is BoxCollider2D)
        {
            BoxCollider2D boxCol = _collider as BoxCollider2D;
            nodeOverlapCountX = CalcOverlapNodeCount(boxCol.size.x);
            nodeOverlapCountY = CalcOverlapNodeCount(boxCol.size.y);
        }

        else if (_collider is CapsuleCollider2D)
        {
            CapsuleCollider2D capsuleCol = _collider as CapsuleCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountX = CalcOverlapNodeCount(capsuleCol.size.x);
            nodeOverlapCountY = CalcOverlapNodeCount(capsuleCol.size.y);

        }

        else if (_collider is CircleCollider2D)
        {
            CircleCollider2D circleCol = _collider as CircleCollider2D;

            //GetOverlapNodeCount
            nodeOverlapCountY = nodeOverlapCountX = CalcOverlapNodeCount(circleCol.radius * 2);
        }


        if (nodeOverlapCountX > 0 || nodeOverlapCountY > 0)
        {
            //겹친 노드 읽어오기
            foreach (t_Node OverlapNode in GetNeighboringNodes(NodeFromWorldPosition(_collider.transform.position), nodeOverlapCountX, nodeOverlapCountY, true))
            {
                if (OverlapNode.IsObject)
                {
                    OverlapNode.IsObject = false;
                }
            }
        }
    }

    /// <summary>
    /// 월드위치를 노드정보로 읽어오기
    /// </summary>
    /// <param name="_worldPosition"></param>
    /// <returns></returns>
    public t_Node NodeFromWorldPosition(Vector3 _worldPosition)
    {
        float xPoint = (((_worldPosition.x - transform.parent.position.x + (transform.parent.position.x-transform.position.x)) + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = (((_worldPosition.y - transform.parent.position.y + (transform.parent.position.y - transform.position.y)) + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return gridNode[x, y];
    }

   

    //오브젝트와 노드가 겹치는 노드갯수 계산
    public int CalcOverlapNodeCount(float objBoxSize)
    {
        return Mathf.RoundToInt((objBoxSize - nodeRadius) / nodeDiameter);
    }


    //           / / / @ / 
    //           / - / @ / 
    //           / / / @ / 
    //           / / / / / 
    //@ 노드 찾기
    public List<t_Node> GetNeighboringLineNode(t_Node _currentNode, t_Node _neighboringNode, int nodeOverlapCountX, int nodeOverlapCountY)
    {
        List<t_Node> NeighboringLineNode = new List<t_Node>();

        Vector3 direction = (_neighboringNode.Pos - _currentNode.Pos).normalized;

        int xCheck = 0, yCheck = 0;

        if (direction == Vector3.up)
        {
            xCheck = _neighboringNode.gridX;
            yCheck = _neighboringNode.gridY + nodeOverlapCountY;

            if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
            {
                for (int i = -nodeOverlapCountX; i <= nodeOverlapCountX; i++)
                {
                    NeighboringLineNode.Add(gridNode[xCheck + i, yCheck]);

                }
            }

        }
        else if (direction == Vector3.down)
        {
            xCheck = _neighboringNode.gridX;
            yCheck = _neighboringNode.gridY - nodeOverlapCountY;

            if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
            {
                for (int i = -nodeOverlapCountX; i <= nodeOverlapCountX; i++)
                {
                    NeighboringLineNode.Add(gridNode[xCheck + i, yCheck]);

                }
            }
        }
        else if (direction == Vector3.right)
        {
            xCheck = _neighboringNode.gridX + nodeOverlapCountX;
            yCheck = _neighboringNode.gridY;

            if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
            {
                for (int i = -nodeOverlapCountY; i <= nodeOverlapCountY; i++)
                {
                    NeighboringLineNode.Add(gridNode[xCheck, yCheck + i]);

                }
            }
        }
        else if (direction == Vector3.left)
        {
            xCheck = _neighboringNode.gridX - nodeOverlapCountX;
            yCheck = _neighboringNode.gridY;

            if (xCheck >= 0 && xCheck < gridSizeX && yCheck >= 0 && yCheck < gridSizeY)
            {
                for (int i = -nodeOverlapCountY; i <= nodeOverlapCountY; i++)
                {
                    NeighboringLineNode.Add(gridNode[xCheck, yCheck + i]);

                }
            }
        }

       
        return NeighboringLineNode;
    }

    //주변노드만 찾기
    public List<t_Node> GetNeighboringNodes(t_Node _Node, int nodeOverlapCountX, int nodeOverlapCountY, bool isDiagonal)
    {
        List<t_Node> NeighboringNodes = new List<t_Node>();

        //default value : nodeOverlapCountX,nodeOverlapCountY = 0
        for (int x = -nodeOverlapCountX - 1; x <= nodeOverlapCountX + 1; x++)
        {
            for (int y = -nodeOverlapCountY - 1; y <= nodeOverlapCountY + 1; y++)
            {
                if (isDiagonal)
                {
                    if (x == 0 && y == 0)
                        continue;
                }
                else
                {
                    if ((x == 0 && y == 0) || (x != 0 && y != 0))
                        continue;
                }

                int checkX = _Node.gridX + x + (x * nodeOverlapCountX);
                int checkY = _Node.gridY + y + (y * nodeOverlapCountY);

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    NeighboringNodes.Add(gridNode[checkX, checkY]);
                }
            }
        }
        return NeighboringNodes;
    }
    
    #region NodeDraw
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        RoomManager RoomManager = GameObject.FindWithTag("RoomManager").GetComponent<RoomManager>();
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));


        if (gridNode != null && displayGridGizmos)
        {

            foreach (t_Node n in gridNode)
            {
                Gizmos.color = (n.Walkable&& !n.IsObject) ? Color.white : Color.red;
                

                for (int i = 0; i < RoomManager.PlayerLocationRoomMonsterData().Count; ++i)
                {
                    if (RoomManager.PlayerLocationRoomMonsterData()[i].transform.GetComponent<Tracking>().pathFinding.finalPath != null)
                    {
                        if (RoomManager.PlayerLocationRoomMonsterData()[i].transform.GetComponent<Tracking>().pathFinding.finalPath.Contains(n))
                        {
                            Gizmos.color = Color.blue;
                        }
                    }
                }


                Gizmos.DrawCube(n.Pos, Vector3.one * (nodeDiameter - 0.005f));

            }

        }
#endif
    }
    #endregion
    
}
