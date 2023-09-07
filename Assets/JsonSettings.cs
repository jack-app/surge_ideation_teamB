using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using jsontype;

namespace jsontype
{
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
        public IList<bool> wireInterface;
        public string texture;

    }
    [System.Serializable]
    public class Electronics
    {
        public IList<Cells> cells;

    }
    [System.Serializable]
    public class Map
    {
        public Start start;
        public Goal goal;
        public IList<Electronics> electronics;

    }
    [System.Serializable]
    public class Pieces
    {
        public IList<Cells> cells;

    }
    [System.Serializable]
    public class data
    {
        public Map map;
        public IList<Pieces> pieces;

    }
}

public class JsonSettings : MonoBehaviour
{
    //JSONファイルのパスを記載する。
    private string jsonPath = "Assets/test.json";


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    //JSONファイルを読み込む。
    public data loadSettings()
    {
        //ファイル名が間違ってる場合はエラーを出しとく
        if (!File.Exists(jsonPath))
        {
            Debug.Log("setting File not Exists");
            return new data();
        }

        //JSONファイルを読み込む
        var json = File.ReadAllText(jsonPath);

        //オブジェクト化する
        data obj = JsonUtility.FromJson<data>(json);

        return obj;
    }

}