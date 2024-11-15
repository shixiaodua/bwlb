using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class SlideScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    public GameNormalLevelPanel gameNormalLevelPanel;
    public int totalTtemNumber;
    public Text pageNum;
    private float lastProportion;
    private float currentProportion;
    private float oneItemProportion;
    public int currentNum;
    private ScrollRect scrollRect;
    private RectTransform content;
    

    Tween dh;
    private void Awake()
    {   
        currentNum = 1;
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        oneItemProportion = 1.0f / (totalTtemNumber - 1);
    }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        dh.Kill();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.uiManager.mUIFacade.PlayPagingAudioClip();
        currentProportion = scrollRect.horizontalNormalizedPosition;
        float currentRatio;
        int moveNumber = (int)(Mathf.Abs(currentProportion - lastProportion) / (oneItemProportion / 2)+1)/2;
        
        if (currentProportion < lastProportion)
        {
            currentRatio = lastProportion - Mathf.Min(moveNumber,1) * oneItemProportion;
            if (currentRatio < 0) currentRatio = 0;
            currentNum = Mathf.Max(currentNum - Mathf.Min(moveNumber, 1),1);
        }
        else
        {
            
            currentRatio = lastProportion + Mathf.Min(moveNumber, 1) * oneItemProportion;
            if (currentRatio > 1) currentRatio = 1;
            currentNum =Mathf.Min(currentNum + Mathf.Min(moveNumber, 1),totalTtemNumber);
        }
        lastProportion = currentRatio;
        dh = DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpVaule => scrollRect.horizontalNormalizedPosition = lerpVaule, currentRatio, 0.1f);
        if(pageNum!=null)pageNum.text = currentNum.ToString()+"/"+totalTtemNumber.ToString();

        if (gameNormalLevelPanel != null) { gameNormalLevelPanel.UpdateStaticUI(currentNum);}
    }

    public void Init()
    {
        if (content == null) return;
        content.localPosition = Vector3.zero;
        if (pageNum == null) return;
        pageNum.text = "1/"+totalTtemNumber.ToString();
    }

    public void ToLastPage()
    {
        float currentRatio = lastProportion - oneItemProportion;
        if (currentRatio < 0) currentRatio = 0;
        lastProportion = currentRatio;
        dh = DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpVaule => scrollRect.horizontalNormalizedPosition = lerpVaule, currentRatio, 0.1f);
    }
    public void ToNextPage()
    {
        float currentRatio = lastProportion + oneItemProportion;
        if (currentRatio > 1) currentRatio = 1;
        lastProportion = currentRatio;
        dh = DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpVaule => scrollRect.horizontalNormalizedPosition = lerpVaule, currentRatio, 0.1f);
    }


}
