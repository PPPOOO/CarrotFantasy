using UnityEngine;
using System.Collections;

public class TowerBuild : IBuilder<Tower>
{
    public int m_towerID;
    private GameObject towerGo;
    public int m_towerLevel;

    public Tower GetProductClass(GameObject gameobject)
    {
        return gameobject.GetComponent<Tower>();
    }

    public GameObject GetProduct()
    {
        GameObject go = GameController.Instance.GetGameObjectResource("Tower/ID" + m_towerID + "/TowerSet/" + m_towerLevel);
        Tower tower = GetProductClass(go);
        GetData(tower);
        GetOtherResource(tower);
        return go;
    }

    public void GetData(Tower productClassGo)
    {
        productClassGo.towerID = m_towerID;
    }

    public void GetOtherResource(Tower productClassGo)
    {
        productClassGo.GetTowerProperty();
    }
}
