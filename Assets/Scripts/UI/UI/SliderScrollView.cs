using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SliderScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect scrollRect;
    private float beginMousePosX;
    private float endMousePosX;
    private float oneItemLength;//滑动一个单元格需要的距离

    private Vector3 currentContentLocalPos;
    private Vector3 contentInitPos;

    private GridLayoutGroup layoutGroup;
    private int cellLength;//每个单元格长度
    private int spacing;//间隙
    private int totalItemNum;//共有几个单元格
    private int currentIndex;//当前单元格索引

    public bool needSendMessage;
    public Text pageText;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        layoutGroup = transform.GetChild(0).GetChild(0).GetComponent<GridLayoutGroup>();
        currentIndex = 1;
        currentContentLocalPos = scrollRect.content.localPosition;
        contentInitPos = scrollRect.content.localPosition;
        scrollRect.horizontalNormalizedPosition = 0;
        cellLength = (int)layoutGroup.cellSize.x;
        spacing = (int)layoutGroup.spacing.x;
        UpdateChildrenCount();
        oneItemLength = cellLength + spacing;
        if (pageText != null)
        {
            pageText.text = currentIndex + "/" + totalItemNum;
        }
    }

    public void Init()
    {
        currentIndex = 1;
        if (scrollRect != null)
        {
            scrollRect.content.localPosition = contentInitPos;
            currentContentLocalPos = contentInitPos;
            if (pageText != null)
            {
                pageText.text = currentIndex + "/" + totalItemNum;
            }
        }
       
    }

    public void ToNextPage()
    {
        float moveDistance = 0;
        if (currentIndex >= totalItemNum)
        {
            return;
        }
        moveDistance = -oneItemLength;
        currentIndex++;
        if (pageText != null)
        {
            pageText.text = currentIndex + "/" + totalItemNum;
        }

        UpdatePanel(true);
        DOTween.To(() => scrollRect.content.localPosition, lerpValue => scrollRect.content.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.InOutQuint);
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
        
    }
    public void ToLastPage()
    {
        float moveDistance = 0;
        if (currentIndex <= 1)
        {
            return;
        }
        moveDistance = oneItemLength;
        currentIndex--;
        if (pageText != null)
        {
            pageText.text = currentIndex + "/" + totalItemNum;
        }
        UpdatePanel(false);
        DOTween.To(() => scrollRect.content.localPosition, lerpValue => scrollRect.content.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.InOutQuint);
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePosX = Input.mousePosition.x;
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        float offsetX = 0;
        float moveDistance = 0;
        endMousePosX = Input.mousePosition.x;
        offsetX = beginMousePosX - endMousePosX;
        if (offsetX > 0)//右滑
        {
            if (currentIndex >= totalItemNum)
            {
                return;
            }
            UpdatePanel(true);
            moveDistance = -oneItemLength;
            currentIndex++;
        }
        else//左滑
        {
            if (currentIndex <= 1)
            {
                return;
            }
            UpdatePanel(false);
            moveDistance = oneItemLength;
            currentIndex--;
        }
        if (pageText != null)
        {
            pageText.text = currentIndex + "/" + totalItemNum;
        }

        DOTween.To(() => scrollRect.content.localPosition, lerpValue => scrollRect.content.localPosition = lerpValue, currentContentLocalPos+new Vector3(moveDistance,0,0), 0.5f).SetEase(Ease.InOutQuint);
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();

        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }


    public void UpdateChildrenCount()
    {
        totalItemNum = layoutGroup.gameObject.transform.childCount;
    }

    public void UpdatePanel(bool toNext)
    {
        if (transform.parent.GetComponent<GameNormLevelPanel>() != null)
        {
            if (toNext)
            {
                gameObject.SendMessageUpwards("ToNextLevel");
            }
            else
            {
                gameObject.SendMessageUpwards("ToLastLevel");
            }
        }
        
    }
}
