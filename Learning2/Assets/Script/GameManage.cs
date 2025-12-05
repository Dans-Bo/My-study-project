using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public static GameManage Instance {get ; private set; }
    [SerializeField] private GameObject audioManagePrefab; 
    public AudioManage audioManage {get ; private set;}

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
          
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioManage = Instantiate(audioManagePrefab).GetComponent<AudioManage>();
        DontDestroyOnLoad(audioManage.gameObject);
    }
}

