using System.Collections.Generic;
using AStar;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class TestAStar : MonoBehaviour
{
    //左上角第一个块位置
    public int beginX ;
    public int beginY;
    
    //每个格子间的偏移距离
    public int offsetX;
    public int offsetY;

    //地图格子总宽高
    public int mapW;
    public int mapH;

    public Material red;
    public Material yellow;
    public Material green;
    private Vector2 beginPos = Vector2.right * -1;
    private Dictionary<string, GameObject> cubeName = new();
    PlayerInputController playerInputActions;
     List<AStar_Node> list = new();
    public bool isPressed;
    public Material normal;

    void Awake()
    {
        playerInputActions = new PlayerInputController();
    }

    void OnEnable()
    {
        playerInputActions.Game.Attack.performed += OnAttackPerformed;
        playerInputActions.Game.Attack.canceled += OnAttackCanceled;
    }
    private void OnDisable() {
       playerInputActions.Game.Attack.performed -= OnAttackPerformed;
        playerInputActions.Game.Attack.canceled -= OnAttackCanceled;
    }

     private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        isPressed = true;
    } 

     private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        isPressed = false;
    } 

    void Start()
    {

        playerInputActions.Game.Enable();

        Astar_Manage.Instance.InitMap(mapW,mapH);
        for (int i = 0; i< mapW; ++i)
        {
            for(int j = 0; j < mapH; ++j) 
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);

                obj.name = i + "_" + j;
                cubeName.Add(obj.name, obj);
                //得到格子类型
                AStar_Node node = Astar_Manage.Instance.Nodes[i, j];
                if(node.status == AS_NodeStatus.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed )
        {
            //isPressed = false;
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if(Physics.Raycast(ray ,out  info,1000))
            {


                if (beginPos == Vector2.right * -1)
                {
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubeName[list[i].gridX + "_" + list[i].gridY].GetComponent<MeshRenderer>().material =  normal;
                        }
                    }
                    string[] str = info.collider.gameObject.name.Split('_');
                    //得到行列位置，就是开始的位置
                    beginPos = new Vector2(int.Parse(str[0]), int.Parse(str[1]));
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                }
                else
                {
                    string[] str = info.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(str[0]), int.Parse(str[1]));

                    list = Astar_Manage.Instance.FindPath(beginPos, endPos);

                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubeName[list[i].gridX + "_" + list[i].gridY].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                    
                    beginPos = Vector2.right * -1;
                }
            }
        }
    }
}
