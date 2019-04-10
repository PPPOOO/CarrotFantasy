using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{

    public int adventrueModelNum;
    public int burriedLevelNum;
    public int bossModelNum;
    public int coin;
    public int killMonsterNum;
    public int killBossNum;
    public int clearItemNum;
    public List<bool> unLockedNormalModelBigLevelList;
    public List<Stage> unLockedNormalModelLevelList;
    public List<int> unLockedeNormalModelLevelNum;

    public int cookies;
    public int milk;
    public int nest;
    public int diamands;
    public List<MonsterPetData> monsterPetDataList;
    //public PlayerManager()
    //{
    //    //adventrueModelNum = 100;
    //    //burriedLevelNum = 100;
    //    //bossModelNum = 100;
    //    //coin = 100;
    //    //killMonsterNum = 100;
    //    //killBossNum = 100;
    //    //clearItemNum = 100;
    //    //unLockedNormalModelLevelNumList = new List<int>()
    //    //{
    //    //    2,2,2
    //    //};
    //    //unLockedNormalModelBigLevelList = new List<bool>()
    //    //{
    //    //    true,true,true
    //    //};
    //    //unLockedNormalModelLevelList = new List<Stage>()
    //    //{
    //    //    new Stage(10,2,new int[]{1,2},false,0,1,1,true,false),
    //    //    new Stage(10,2,new int[]{3,2},false,0,2,1,false,false),
    //    //    new Stage(10,2,new int[]{4,2},false,0,3,1,true,false),
    //    //    new Stage(10,2,new int[]{5,2},false,0,4,1,false,false),
    //    //    new Stage(10,2,new int[]{5,2},false,0,5,1,false,true),
    //    //    new Stage(10,2,new int[]{2,2},false,0,2,1,true,false),
    //    //    new Stage(10,2,new int[]{3,2},false,0,2,2,true,false),
    //    //};
    //}

    public void SaveData()
    {
        Memento memento = new Memento();
        memento.SaveByJson();
    }

    public void ReadData()
    {
        Memento memento = new Memento();
        PlayerManager playerManager = memento.LoadByJson();
        adventrueModelNum = playerManager.adventrueModelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModelNum = playerManager.bossModelNum;
        coin = playerManager.coin;
        killMonsterNum = playerManager.killMonsterNum;
        killBossNum = playerManager.killBossNum;
        clearItemNum = playerManager.clearItemNum;
        cookies = playerManager.cookies;
        milk = playerManager.milk;
        nest = playerManager.nest;
        diamands = playerManager.diamands;

        unLockedNormalModelBigLevelList = playerManager.unLockedNormalModelBigLevelList;
        unLockedNormalModelLevelList = playerManager.unLockedNormalModelLevelList;
        unLockedeNormalModelLevelNum = playerManager.unLockedeNormalModelLevelNum;
        monsterPetDataList = playerManager.monsterPetDataList;
    }
}
