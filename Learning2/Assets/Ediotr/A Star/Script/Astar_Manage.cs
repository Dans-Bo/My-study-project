using System;
using System.Collections.Generic;
using BehaviourTrees;
using Unity.VisualScripting;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// A星寻路管理
    /// </summary>
    public class Astar_Manage
    {
        private static readonly Lazy<Astar_Manage> instance = new();
        public static Astar_Manage Instance => instance.Value;


        private int mapWidth; //地图宽
        private int mapHeight; //地图高

        private AStar_Node[,] nodes; //地图所有格子的容器
        public AStar_Node[,] Nodes {get { return nodes; } }

        private List<AStar_Node> openList = new(); //开启列表
        private List<AStar_Node> closeList = new(); //关闭列表

        /// <summary>
        /// 初始化地图
        /// </summary>
        /// <param name="w">地图宽</param>
        /// <param name="h">地图高</param>
        public void InitMap(int w, int h)
        {
            //记录宽高
            mapWidth = w;
            mapHeight = h;

            nodes = new AStar_Node[w, h]; //初始化地图格子

            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    //TODO
                    //暂时随机阻挡，后期根据地图配置文件来读取阻挡物
                    AStar_Node node = new(i, j, UnityEngine.Random.Range(0, 100) < 20 ? AS_NodeStatus.Stop : AS_NodeStatus.Walk);
                    nodes[i, j] = node;
                }
            }
        }
        /// <summary>
        /// 寻路方法
        /// </summary>
        /// <param name="startPos">起始点</param>
        /// <param name="endPos">终点</param>
        /// <returns></returns>
        public List<AStar_Node> FindPath(Vector2 startPos, Vector2 endPos)
        {
            

            //如果在地图范围内,从列表中取出判断是否是可走的
            if (startPos.x < 0 || startPos.y < 0 || startPos.x >= mapWidth || startPos.y >= mapHeight
                || endPos.x < 0 || endPos.y < 0 || endPos.x >= mapWidth || endPos.y >= mapHeight)
            {
                return null;
            }

            AStar_Node start = nodes[(int)startPos.x, (int)startPos.y];
            AStar_Node end = nodes[(int)endPos.x, (int)endPos.y];

            if (start.status == AS_NodeStatus.Stop || end.status == AS_NodeStatus.Stop) return null;
            openList.Clear();
            closeList.Clear();

            //把起点放入开启列表中
            start.father = null;
            start.f = start.g + start.h;
            start.g = 0;
            start.h = Mathf.Abs(end.gridX - start.gridX) + Mathf.Abs(end.gridY - start.gridY);;
            openList.Add(start);



            //循环寻路
            while (openList.Count > 0)
            {

                //从起始点开始，找周围的点，并存入开启列表中
                for (int i = -1; i <= 1; ++i)
                {
                    for (int j = -1; j <= 1; ++j)
                    {
                        if (i == 0 && j == 0) continue; //跳过自身
                        float g = (Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1) ? 1.4f : 1;

                        FindNearlyNodeToOpenlist(start.gridX + i, start.gridY + j, g, start, end);
                    }
                }
                 //从小到大排序
                 openList.Sort(SortOpenList);
                //最小值f的放入关闭列表，另开始点为这个最小的点，再删除开启列表中存入关闭列表的点
                closeList.Add(openList[0]);
                AStar_Node current = openList[0];
                openList.RemoveAt(0);

                if (current == end)
                {
                    List<AStar_Node> path = new();
                    path.Add(end);
                    //从终点开始，回溯父节点
                    while (end.father != null)
                    {
                        path.Add(end.father);
                        end = end.father;
                    }
                    path.Reverse(); //翻转列表
                    return path;
                } 
            }
            return null;
        }

        /// <summary>
        /// 寻找临近点放入开启列表
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="g"></param>
        /// <param name="father"></param>
        /// <param name="end"></param>
        private void FindNearlyNodeToOpenlist(int x, int y, float g, AStar_Node father, AStar_Node end)
        {
            if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) return;

            AStar_Node node = nodes[x, y];
            if (node == null || node.status == AS_NodeStatus.Stop) return;
            if (closeList.Contains(node) || openList.Contains(node)) return;

            //计算f值 f=g+h
            node.father = father;
            node.g = father.g + g;
            node.h = Mathf.Abs(end.gridX - node.gridX) + Mathf.Abs(end.gridY - node.gridY);
            node.f = node.g + node.h;
            openList.Add(node);
        }
        
        /// <summary>
        /// 排序函数
        /// </summary>
        private int SortOpenList(AStar_Node a , AStar_Node b)
        {
            return a.f.CompareTo(b.f);         
        }
    }

}
