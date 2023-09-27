using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//int num = GameManager.furnitureNum;
public class TestFurnitureNum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameManager.furnitureNum + "この家具が破壊されました");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
