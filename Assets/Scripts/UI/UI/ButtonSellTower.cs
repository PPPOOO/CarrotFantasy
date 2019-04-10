using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonSellTower : MonoBehaviour
{
    public int price;
    private Button button;
    private Text text;
    private GameController gameController;



    // Use this for initialization
    void Start()
    {
        button=GetComponent<Button>();
        text =transform.Find("Text"). GetComponent<Text>();
        button.onClick.AddListener(SellTower);
        gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SellTower()
    {
        gameController.selectGrid.towerPersonalProperty.SellTower();
        gameController.selectGrid.InitGrid();
        gameController.selectGrid.handleTowerCanvasGo.SetActive(false);
#if Game
              gameController.selectGrid.HideGrid();
#endif


        gameController.selectGrid = null;
    }

    public void UpdateUI()
    {
        if (text == null)
        {
            button = GetComponent<Button>();
            text = transform.Find("Text").GetComponent<Text>();
            button.onClick.AddListener(SellTower);
            gameController = GameController.Instance;
        }
        price = gameController.selectGrid.towerPersonalProperty.sellPrice;
        text.text = price.ToString();
    }
}
