using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonUpLevel : MonoBehaviour
{

    private int price;
    private Button button;
    private Text text;
    private Image image;
    private Sprite canUpLevelSprite;
    private Sprite cantUpLevelSprite;
    private Sprite rechHighestLevel;
    private GameController gameController;

    private void OnEnable()
    {
        if (text == null)
        {
            return;
        }
        UpdateUIView();
    }

    private void Start()
    {
#if Game
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(UpLevel);
        canUpLevelSprite = gameController.GetSprite("NormalModel/Game/Tower/Btn_CanUpLevel");
        cantUpLevelSprite = gameController.GetSprite("NormalModel/Game/Tower/Btn_CantUpLevel");
        rechHighestLevel = gameController.GetSprite("NormalModel/Game/Tower/Btn_ReachHighestLevel");
        text = transform.Find("Text").GetComponent<Text>();
        image = GetComponent<Image>();
#endif
    }

    void UpLevel()
    {
        gameController.towerBuild.m_towerID = gameController.selectGrid.tower.towerID;
        gameController.towerBuild.m_towerLevel = gameController.selectGrid.towerLevel+1;
        gameController.selectGrid.towerPersonalProperty.UpLevelTower();
        GameObject towerGo = gameController.towerBuild.GetProduct();
        towerGo.transform.SetParent(gameController.selectGrid.transform);
        towerGo.transform.position = gameController.selectGrid.transform.position;

#if Game
        gameController.selectGrid.AfterBuild();
        gameController.selectGrid.HideGrid();
#endif
        gameController.selectGrid = null;

    }

    private void UpdateUIView()
    {
        if (gameController.selectGrid.towerLevel >= 3)
        {
            image.sprite = rechHighestLevel;
            button.enabled = false;
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
            price = gameController.selectGrid.towerPersonalProperty.upLevelPrice;
            text.text = price.ToString();
            if (gameController.coin >= price)
            {
                image.sprite = canUpLevelSprite;
                button.enabled = true;
            }
            else
            {
                image.sprite = cantUpLevelSprite;
                button.enabled = false;
            }

        }
    }
}
