using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using System.IO;
using UnityEngine.SceneManagement;
using jsontype;
using static SceneController;

namespace jsontype

{
    [System.Serializable]
    public class Tile
    {
        public string texture;

    }
    [System.Serializable]
    public class Size
    {
        public int x;
        public int y;

    }
    [System.Serializable]
    public class Start
    {
        public int x;
        public int y;

    }
    [System.Serializable]
    public class Goal
    {
        public int x;
        public int y;

    }
    [System.Serializable]
    public class Cells
    {
        public int x;
        public int y;
        public List<bool> wireInterface;
        public string texture;

    }
    [System.Serializable]
    public class Map
    {
        public Tile tile;
        public Size size;
        public Start start;
        public Goal goal;

    }
    [System.Serializable]
    public class Pieces
    {
        public string type;
        public List<Cells> cells;
        
    }
    
    [System.Serializable]
    public class data
    {
        public int stageNum;
        public Map map;
        public List<Pieces> pieces;

    }
}

public class JsonSettings : MonoBehaviour
{
    public GameObject gamemanager;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadSettings());
    }


    // Update is called once per frame
    void Update()
    {

    }

    //JSON�t�@�C����ǂݍ��ށB
    public IEnumerator loadSettings()
    {
        string jsonPath =  "https://mono-1729.github.io/json-upload/" + selectedStage.stage + ".json";
        Debug.Log(jsonPath);
        data obj;
        UnityWebRequest www = UnityWebRequest.Get(jsonPath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // データの利用例（必要に応じて）
            string jsonContent = www.downloadHandler.text;
            obj = JsonUtility.FromJson<data>(jsonContent);
            gamemanager.GetComponent<GameManager>().initializeMap(obj);
        }
        else
        {
            Debug.LogError("Failed to load JSON file: " + www.error);
        }  
    }
}