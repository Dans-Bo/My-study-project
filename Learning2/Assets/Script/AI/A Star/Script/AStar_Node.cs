
namespace AStar
{
    public enum AS_NodeStatus
    {
        Walk, Stop
    }
        /// <summary>
        /// A星算法格子
        /// </summary>
    public class AStar_Node
    {
        //格子对象坐标
        public int gridX;
        public int gridY;

        public float f; //寻路消耗
        public float g; //距离起点的距离
        public float h; //距离终点的距离
        public AStar_Node father;
        public AS_NodeStatus status;

        public AStar_Node(int axisX, int axisY, AS_NodeStatus status)
        {
            this.gridX = axisX;
            this.gridY = axisY;
            this.status = status;
        }

    }
}

