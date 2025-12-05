using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioPanel : BasePanel,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] private RectTransform UICloseButton;
    [SerializeField] private Transform UIBgmSlide;
    [SerializeField] private Transform UISfxSlide;
    [SerializeField] private Transform UIHomeButton;
    private Slider bgmSlider;
    private Slider sfxSlider;
    private AudioManage audioManage;
    [Header("可拖动区域")]
    //[SerializeField] private float dragAreaHeight = 50f; //顶部可拖动区域高度
    [SerializeField] RectTransform topGreenBar;

    [Tooltip("是否限制拖动范围在屏幕内")]

    public bool limitInScreen = true;
    private RectTransform panelRect; //面板的transform缓存
    private Vector2 offset; //鼠标与面板的偏移量
    private bool canDrag = false; //是否可以拖动

    //private Vector2 originPanelPos;//鼠标按下时，面板的初始锚点位置
    //private Vector2 originMousePos;//开始拖动时鼠标的初始本地坐标

    void Awake()
    {
        panelRect = GetComponent<RectTransform>();

    }

    void  Start() 
    {
        audioManage = GameManage.Instance.audioManage;
        InitUI();
        InitClick();
        InitSliderValues();
    }
    void InitUI()
    {
        bgmSlider = UIBgmSlide.GetComponentInChildren<Slider>();
        if(bgmSlider == null) Debug.Log($"{bgmSlider}为空");
        sfxSlider = UISfxSlide.GetComponentInChildren<Slider>();
        if(sfxSlider == null) Debug.Log($"{sfxSlider}为空");
    }

    void InitClick()
    {
        UICloseButton.GetComponent<Button>().onClick.AddListener(OnclickClose);
        UIHomeButton.GetComponent<Button>().onClick.AddListener(OnClickHome);
        bgmSlider. onValueChanged.AddListener(OnBgmVolumeChange);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChange);
    }

    private void OnClickHome()
    {
        ClosePanel();
    }

    /// <summary>
    /// 初始化音量，从Audiomanager获取当前音量
    /// </summary>
    void InitSliderValues()
    { 
        bgmSlider.value = audioManage.GetBgmVolume();
        sfxSlider.value = audioManage.GetSfxVolume();
        
    }

    private void OnSfxVolumeChange(float arg0)
    {
        audioManage.SetSfxVolume(arg0);
    }

    private void OnBgmVolumeChange(float arg0)
    {
        audioManage.SetBgmVolume(arg0);
    }

    private void OnclickClose()
    {
        ClosePanel();
    }
/// <summary>
/// 开始拖动
/// </summary>
/// <param name="eventData"></param>
/// <exception cref="NotImplementedException"></exception>
   public void OnBeginDrag(PointerEventData eventData)
    {
        //判断鼠标是否在顶部绿色条内
       bool isInTopArea = RectTransformUtility.RectangleContainsScreenPoint(
        topGreenBar,
        eventData.position,
        eventData.pressEventCamera);

        //判断鼠标是否在关闭按钮内
        bool isInCloseButton = RectTransformUtility.RectangleContainsScreenPoint(
            UICloseButton,
            eventData.position,
            eventData.pressEventCamera);

         // 计算偏移量时使用正确的相机（适配Canvas模式）
        Camera uiCamera = eventData.pressEventCamera;
        
        if(isInTopArea && !isInCloseButton)
        {
            canDrag = true;
            Debug.Log("可以拖拽");
            
            //计算偏移量
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panelRect.parent.GetComponent<RectTransform>(),
                eventData.position,
                uiCamera,     //eventData.pressEventCamera,
                out Vector2 mouseCanvasPos);

            offset = panelRect.anchoredPosition - mouseCanvasPos;
        }else canDrag = false; 
    }
/*   public void OnBeginDrag(PointerEventData eventData)
    {
        //判断鼠标是否在顶部区域
        if(RectTransformUtility.RectangleContainsScreenPoint(
            topGreenBar,
            eventData.position,
            eventData.pressEventCamera
        ))
        {
            canDrag = true;
            //计算偏移量
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    panelRect.parent.GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 mouseCanvasPos);

                offset = panelRect.anchoredPosition - mouseCanvasPos;

        }else canDrag = false;
    } */
        

/* public void OnBeginDrag(PointerEventData eventData)
    {
        //将鼠标点击位置转换为面板自身的本地坐标
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
        panelRect, //目标的transform
        eventData.position,         //鼠标屏幕位置
        null,                       //eventData.pressEventCamera, 交互相机（overlaay模式为null
        out Vector2 localClickPos ))  //输出面板本地坐标
        {
            float panelTopY = panelRect.rect.yMax; //面板顶部的本地y坐标
            float dragAreaMinY = panelTopY - dragAreaHeight; //可拖动区域下边界

            //只有鼠标点击在顶部区域，才允许拖动
            canDrag = localClickPos.y >= dragAreaMinY && localClickPos.y <= panelTopY;

            if(canDrag)
            {
                //计算鼠标与面板锚点的偏移量
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    panelRect.parent.GetComponent<RectTransform>(),
                    eventData.position,
                    null,
                    out Vector2 mouseCanvasPos);

                offset = panelRect.anchoredPosition - mouseCanvasPos;

            }
        }
        else canDrag = false; 
    }*/
    
/// <summary>
/// 拖拽中
/// </summary>
/// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if(!canDrag) return;

        Camera uiCamera = eventData.pressEventCamera;

        //转换鼠标位置到本地Canvas本地坐标，计算面板新位置
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRect.parent.GetComponent<RectTransform>(),
            eventData.position,
            uiCamera,
            out Vector2 mouseCanvasPos))
        {
            //移动面板
            Vector2 targetPos = mouseCanvasPos + offset;

            if(limitInScreen)
            {
                targetPos = ClampPosInScreen(targetPos,uiCamera);
            }
            panelRect.anchoredPosition = targetPos;
        }
    }
/// <summary>
/// 拖拽结束，重置拖动状态
/// </summary>
/// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        canDrag = false;
    }
/// <summary>
/// 限制面板在屏幕内
/// </summary>
/// <param name="pos"></param>
/// <returns></returns>

    private Vector2 ClampPosInScreen(Vector2 pos,Camera uiCamera)
    {
        //获取面板尺寸，考虑缩放
        float panelWidth = panelRect.rect.width * panelRect.lossyScale.x;
        float panelHeight = panelRect.rect.height* panelRect.lossyScale.y;
        float halfWidth = panelWidth/2;
        float halfHeight = panelHeight/2;

        //获取屏幕边界
        float screenMinX,screenMaxX,screenMinY,screenMaxY;

        screenMinX = halfWidth;
        screenMaxX = Screen.width - halfWidth;
        screenMinY = halfHeight;
        screenMaxY = Screen.height - halfHeight;

        //将屏幕边界转换为面板父物体的本地坐标
        Vector2 minLocalPos = ScreenPointToLocalPos(new Vector2(screenMinX, screenMinY), uiCamera);
        Vector2 maxLocalPos = ScreenPointToLocalPos(new Vector2(screenMaxX, screenMaxY), uiCamera);

        //钳制目标位置
        pos.x = Mathf.Clamp(pos.x, minLocalPos.x, maxLocalPos.x);
        pos.y = Mathf.Clamp(pos.y, minLocalPos.y, maxLocalPos.y);

        return pos;
    }

    /// <summary>
    /// 辅助：将屏幕坐标转换为面板父物体的本地坐标
    /// </summary>
    private Vector2 ScreenPointToLocalPos(Vector2 screenPos, Camera uiCamera)
    {
        RectTransform parentRect = panelRect.parent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            screenPos,
            uiCamera,
            out Vector2 localPos);
        return localPos;
    }
    /// <summary>
/// 限制面板在屏幕内
/// </summary>
/// <param name="pos"></param>
/// <returns></returns>
    private Vector2 ClampPosInScreen(Vector2 pos)
    {
        
        RectTransform canvasRect = panelRect.parent.GetComponent<RectTransform>();
        float halfWidth = panelRect.rect.width/2;
        float halfHeight = panelRect.rect.height/2;

        pos.x= Mathf.Clamp(pos.x,halfWidth,canvasRect.rect.width - halfWidth);
        pos.y = Mathf.Clamp(pos.y,halfHeight,canvasRect.rect.height - halfHeight);

        return pos;
    }


}
