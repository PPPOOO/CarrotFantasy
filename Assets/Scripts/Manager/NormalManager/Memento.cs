using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Memento
{
    public void SaveByJson()
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        string filePath = Application.streamingAssetsPath + "/Json" + "/playerManager.json";
        string saveJsonStr = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close(); 
    }

    public PlayerManager LoadByJson()
    {
        PlayerManager playerManager = new PlayerManager();
        string filePath="";
        if (GameManager.Instance.initPlayerManager)
        {
            filePath = Application.streamingAssetsPath + "/Json" + "/playerManagerInitData.json";
        }
        else
        {
            filePath= Application.streamingAssetsPath + "/Json" + "/playerManager.json";
        }
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
            for (int i = 0; i < playerManager.unLockedNormalModelLevelList.Count; i++)
            {
                //Debug.Log(playerManager.unLockedNormalModelLevelList[i].mTotalRound);
            }
            return playerManager;
        }
        else
        {
            Debug.Log("playerManager读取失败");
        }
        return null;
    }

}
