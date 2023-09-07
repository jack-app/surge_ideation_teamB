using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Start
{
    public int x ;
    public int y ;

}
[System.Serializable]
public class Goal
{
    public int x ;
    public int y ;

}
[System.Serializable]
public class Cells
{
    public int x ;
    public int y ;
    public IList<bool> wireInterface ;
    public string texture ;

}
[System.Serializable]
public class Electronics
{
    public IList<Cells> cells ;

}
[System.Serializable]
public class Map
{
    public Start start ;
    public Goal goal ;
    public IList<Electronics> electronics ;

}
[System.Serializable]
public class Pieces
{
    public IList<Cells> cells ;

}
[System.Serializable]
public class Application
{
    public Map map ;
    public IList<Pieces> pieces ;

}

public class JsonSettings : MonoBehaviour
{
    //JSON�t�@�C���̃p�X���L�ڂ���B
    private string jsonPath = "Assets/test.json";


    // Start is called before the first frame update
    void Start()
    {
        loadSettings();
    }


    // Update is called once per frame
    void Update()
    {

    }

    //JSON�t�@�C����ǂݍ��ށB
    private void loadSettings()
    {
        //�t�@�C�������Ԉ���Ă�ꍇ�̓G���[���o���Ƃ�
        if (!File.Exists(jsonPath))
        {
            Debug.Log("setting File not Exists");
            return;
        }

        //JSON�t�@�C����ǂݍ���
        var json = File.ReadAllText(jsonPath);

        //�I�u�W�F�N�g������
        Application obj = JsonUtility.FromJson<Application>(json);


        //�f�o�b�O�ɕ\������B
        Debug.Log(obj.map.start.x);
    }

}