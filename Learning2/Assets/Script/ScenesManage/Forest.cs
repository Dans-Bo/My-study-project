using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    void Start()
    {
        GameManage.Instance.audioManage.PlayBGM(AudioType.BGM_Forest);
    }
}
