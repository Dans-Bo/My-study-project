
using Cinemachine;
using UnityEngine;

public class BGFollowCamera : MonoBehaviour
{
    
    [Header("跟随的相机")]
    [SerializeField] CinemachineVirtualCamera targetVcam;
    [Header("背景偏移")]
    [SerializeField] Vector2 offset = Vector2.zero;

    private Transform camTransform; // 相机的Transform缓存

    void Start()
    {
        // 获取虚拟相机的实际相机Transform
        if (targetVcam != null)
        {
            camTransform = targetVcam.VirtualCameraGameObject.transform;
        }
        else
        {
            //如果没拖虚拟相机，取主相机
            camTransform = Camera.main.transform;
            Debug.LogWarning("未指定Cinemachine虚拟相机，使用主相机");
        }
    }

    void LateUpdate()
    {
        // 只同步X/Y轴（
        transform.position = new Vector3(
            camTransform.position.x + offset.x,
            camTransform.position.y + offset.y,
            transform.position.z
        );
    }
}
