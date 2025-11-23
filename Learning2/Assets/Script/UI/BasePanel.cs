using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected new string name = "";

    public virtual void UIActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string UIname)
    {
        if(!string.IsNullOrEmpty(UIname))
        {
            this.name = UIname;
        } 
        else
        {
            #if UNITY_EDITOR
            Debug.LogError($"打开面板失败：UIname 为 null 或空！");
            #endif
        } 
        UIActive(true);
    }

    public void ClosePanel()
    {
        UIActive(false);

        if(!string.IsNullOrEmpty(name) && UIManager.Instance != null)
        {
            if(UIManager.Instance.openPanelsDic.ContainsKey(name))
            {
                UIManager.Instance.openPanelsDic.Remove(name);
                #if UNITY_EDITOR
                Debug.Log($"面板 {name} 已从缓存中移除");
                #endif
            }
            else
            {
                #if UNITY_EDITOR
                 Debug.LogWarning($"面板 {name} 未在打开面板缓存中");
                 #endif
            }
        }
        else
        {
            #if UNITY_EDITOR
            Debug.LogError("关闭面板失败：name 为 null/空 或 UIManager 实例不存在！");
            #endif
        }

        Destroy(gameObject);
    }   
}
