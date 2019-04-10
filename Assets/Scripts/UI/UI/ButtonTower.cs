using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour
{
    public int towerID;
    public int price;
    private Button button;
    private Sprite canClickSprite;
    private Sprite cantClickSprite;
    private Image image;
    private GameController gameController;

    private void OnEnable()
    {
        if (price == 0)
        {
            return;
        }
        UpdateIcon();
    }

    private void Start()
    {
        gameController = GameController.Instance;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        canClickSprite = gameController.GetSprite("NormalModel/Game/Tower/" + towerID + "/CanClick1");
        cantClickSprite = gameController.GetSprite("NormalModel/Game/Tower/" + towerID + "/CanClick0");
        UpdateIcon();
        price = gameController.towerPriceDict[towerID];
        button.onClick.AddListener(BuildTower);
    }


    private void BuildTower()
    {
        gameController.PlayEffectMusic("NormalModel/Tower/TowerBulid");
        gameController.towerBuild.m_towerID = towerID;
        gameController.towerBuild.m_towerLevel = 1;
        GameObject towerGo = gameController.towerBuild.GetProduct();
        towerGo.transform.SetParent(gameController.selectGrid.transform);
        towerGo.transform.localPosition = Vector3.zero;
        GameObject effectGo = gameController.GetGameObjectResource("BuildEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = towerGo.transform.position;
#if Game
        gameController.selectGrid.HideGrid();
        gameController.selectGrid.AfterBuild();
        gameController.selectGrid.HideGrid();
#endif
        gameController.selectGrid.hasTower = true;
        gameController.ChangeCoin(-price);

        gameController.selectGrid = null;
        gameController.handleTowerCanvas.SetActive(false);
    }

    public void UpdateIcon()
    {
        if (gameController.coin>price)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = cantClickSprite;
            button.interactable = false;
        }
    }

}
