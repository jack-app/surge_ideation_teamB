using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public string getStagePath()
    {
        string jsonPath = "";
        jsonPath = Application.streamingAssetsPath+"/" + selectedStage.stage + ".json";

        return jsonPath;
    }

    private string jsonPath;


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    //JSON�t�@�C����ǂݍ��ށB
    public void loadSettings()
    {
        jsonPath = getStagePath();
        Debug.Log(jsonPath);
        //�t�@�C�������Ԉ���Ă�ꍇ�̓G���[���o���Ƃ�
        if (!File.Exists(jsonPath))
        {
            Debug.Log("setting File not Exists");
            return;
        }
        data obj;

        StreamReader reader = new StreamReader(jsonPath);
        string getone = reader.ReadToEnd();
        reader.Close();

        obj = JsonUtility.FromJson<data>(getone);
        gamemanager.GetComponent<GameManager>().initializeMap(obj);
    }
}