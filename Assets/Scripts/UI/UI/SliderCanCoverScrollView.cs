using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class SliderCanCoverScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    private ScrollRect scrollRect;
    public float contentLength;
    public float beginMousePosX;
    public float endMousePosX;
    public float lastProportion;//上一个位置比例
    public float oneItemLength;//滑动一个单元格需要的距离
    public float oneItemProportion;//滑动一个单元所占比例

    private GridLayoutGroup layoutGroup;
    public int cellLength;//每个单元格长度
    public int spacing;//间隙
    public int leftOffset;
    public int rightOffset;
    public int totalItemNum;//共有几个单元格
    private int currentIndex;//当前单元格索引

    public Text pageText;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        layoutGroup = transform.GetChild(0).GetChild(0).GetComponent<GridLayoutGroup>();
        currentIndex = 1;
        scrollRect.horizontalNormalizedPosition = 0;
        cellLength = (int)layoutGroup.cellSize.x;
        spacing = (int)layoutGroup.spacing.x;
        leftOffset = layoutGroup.padding.left;
        rightOffset = layoutGroup.padding.right;
        totalItemNum = layoutGroup.gameObject.transform.childCount;
        contentLength = (totalItemNum - 1)*cellLength ;
        oneItemProportion = 1 / (float)(totalItemNum - 1);
        oneItemLength = oneItemProportion * contentLength;
        if (pageText != null)
        {
            pageText.text = currentIndex + "/" + totalItemNum;
        }
    }

    public void Init()
    {
        currentIndex = 1;
        lastProportion = 0;
        if (scrollRect != null)
        {
            scrollRect.horizontalNormalizedPosition = 0;
            pageText.text = currentIndex + "/" + totalItemNum;
        }
        
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePosX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float offsetX = 0;
        endMousePosX = Input.mousePosition.x;
        offsetX = (beginMousePosX - endMousePosX)*2;
        if (Mathf.Abs(offsetX) > oneItemLength / 2)
        {
            if(offsetX>0)//右滑
            {
                if (currentIndex >= totalItemNum)
                {
                    return;
                }
                int moveCount = (int)((offsetX- oneItemLength/2) / oneItemLength)+1;
                currentIndex += moveCount;
                if (currentIndex >= totalItemNum)
                {
                    currentIndex = totalItemNum;
                }
                lastProportion += oneItemProportion * moveCount;
                lastProportion = Mathf.Min(lastProportion, 1);
            }
            else//左滑
            {
                if (currentIndex <= 1)
                {
                    return;
                }
                int moveCount = (int)((offsetX + oneItemLength/2) / oneItemLength) - 1;
                currentIndex += moveCount;
                if (currentIndex <= 1)
                {
                    currentIndex = 1;
                }
                lastProportion += oneItemProportion * moveCount;
                lastProportion = Mathf.Max(lastProportion, 0);
            }

            if (pageText != null)
            {
                pageText.text = currentIndex + "/" + totalItemNum;
            }
           
        }
        
        DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.5f).SetEase(Ease.InOutQuint);
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
    }
}
