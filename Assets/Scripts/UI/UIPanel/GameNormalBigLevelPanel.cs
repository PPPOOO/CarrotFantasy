using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameNormalBigLevelPanel : BasePanel
{
    public Transform bigLevelContentTrans;
    public int bigLevelPageCount;
    private SliderScrollView sliderScrollView;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;

    private bool hasRigisterEvent;


    protected override void Awake()
    {
        base.Awake();
        playerManager = mUIFacade.mPlayerManager;
        bigLevelPage = new Transform[bigLevelPageCount];
        sliderScrollView = transform.Find("Scroll View").GetComponent<SliderScrollView>();
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i]= bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNormalModelBigLevelList[i], playerManager.unLockedeNormalModelLevelNum[i], 5, bigLevelPage[i],i+1);
        }
        hasRigisterEvent = true;
    }

    private void OnEnable()
    {
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNormalModelBigLevelList[i], playerManager.unLockedeNormalModelLevelNum[i], 5, bigLevelPage[i], i + 1);
        }
    }

    public void ShowBigLevelState(bool unLocked,int unLockedLevelNum,int totalNum,Transform theBigLevelButtonTrans,int bigLevelID)
    {
        if (unLocked)
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").Find("Tex_Page").GetComponent<Text>().text = unLockedLevelNum + "/" + totalNum;
            Button theBigLevelButtonCom = theBigLevelButtonTrans.GetComponent<Button>();
            theBigLevelButtonCom.interactable = true;
            if (!hasRigisterEvent)
            {
                theBigLevelButtonCom.onClick.AddListener(() =>
                {

                    mUIFacade.PlayButtonAudioClip();
                    mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                    GameNormLevelPanel gameNormLevelPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormLevelPanel;
                    gameNormLevelPanel.ToThisPanel(bigLevelID);
                    GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                    gameNormalOptionPanel.isInBigLevelPanel = false;
                });
            }
           
        }
        else
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true); theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;
        }
    }
    public override void EnterPanel()
    {
        base.EnterPanel();
        sliderScrollView.Init();
        gameObject.SetActive(true);
    }
    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    public void ToNextPage()
    {
        mUIFacade.PlayButtonAudioClip();
        sliderScrollView.ToNextPage();
    }
    public void ToLastPage()
    {
        mUIFacade.PlayButtonAudioClip();
        sliderScrollView.ToLastPage();
    }
}
