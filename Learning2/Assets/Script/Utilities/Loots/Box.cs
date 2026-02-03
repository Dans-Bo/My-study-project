using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Box : MonoBehaviour,IInteractable
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;
    private SpriteRenderer spriteRenderer;
    private bool isDone = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        spriteRenderer.sprite = isDone? openSprite : closeSprite;
    }
    
    public void TriggerAction()
    {
        Debug.Log("打开箱子");
        if(!isDone)
        {
            OpenBox();
        }
        UIManager.Instance.OpenPanel(ConstUIName.lootsBoxPanel);
    }

    private void OpenBox()
    {
        spriteRenderer.sprite = openSprite;
        GameManage.Instance.audioManage.PlaySFX(AudioType.SFX_OpenBox);
        isDone = true;
    }
}
