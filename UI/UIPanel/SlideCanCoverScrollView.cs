using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class SlideCanCoverScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    public GameObject contentPar;
    public Text pageNum;
    private float lastProportion;
    private float currentProportion;
    private int currentNum;
    private ScrollRect scrollRect;
    private RectTransform content;

    Tween dh;

    private void Awake()
    {
        currentNum = 1;
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
    }
    public int  totalTtemNumber(){
        return contentPar.transform.childCount;
    }
    public float  oneItemProportion(){
        float bl=1.0f / (totalTtemNumber() - 1);
        return bl;
    }
    public void InitSildeCanCoverScrollView()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dh.Kill();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.uiManager.mUIFacade.PlayPagingAudioClip();
        currentProportion = scrollRect.horizontalNormalizedPosition;
        if (currentProportion < 0) currentProportion = 0;
        if (currentProportion > 1) currentProportion = 1;
        float currentRatio;
        int moveNumber = (int)(currentProportion/ (oneItemProportion() / 2) + 1) / 2;
        
            currentRatio = moveNumber * oneItemProportion();
            currentNum = moveNumber + 1;

        dh = DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpVaule => scrollRect.horizontalNormalizedPosition = lerpVaule, currentRatio, 0.1f);
        if(pageNum!=null)pageNum.text = currentNum.ToString()+"/11";
    }

    public void Init()
    {
        if (content == null) return;
        content.localPosition = Vector3.zero;
        if (pageNum == null) return;
        pageNum.text =  "1/11";
    }
}
