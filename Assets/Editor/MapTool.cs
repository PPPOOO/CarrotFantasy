using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if Tool
[CustomEditor(typeof(MapMaker))]
public class MapTool : Editor {

    private MapMaker mapMaker;

    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;

    private int selectIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instance;
            EditorGUILayout.BeginHorizontal();
            fileNameList = GetName(fileList);
            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList);
            if (currentIndex != selectIndex)
            {
                selectIndex = currentIndex;
                mapMaker.InitMap();
                mapMaker.LoadLevelFile(mapMaker.LoadLevelInfoFile(fileNameList[selectIndex]));
            }

            if (GUILayout.Button("读取关卡列表"))
            {
                LoadLevelFiles();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("恢复地图编辑器默认状态")) 
            {
                mapMaker.RecoverTowerPoint();
            }

            if (GUILayout.Button("清除怪物路店"))
            {
                mapMaker.ClearMonsterPath();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存当前关卡数据"))
            {
                mapMaker.SaveLevelFileByJson();
            }
        }
    }

    private void LoadLevelFiles()
    {
        ClearList();
        fileList = GetLevelFiles();
    }

    private void ClearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }

    private List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Application.streamingAssetsPath + "/Json/Level/", "*.json");
        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
        
    }

    private string[] GetName(List<FileInfo> fileInfos)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in fileInfos)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }
}
#endif