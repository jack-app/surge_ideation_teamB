using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //id,typeはいらないかも
    public GameObject[] cells;
    public Vector2 initialPosition = Vector2.zero;
    public bool canBePlaced = false;
    public Vector3 worldPosition = Vector3.zero;
    public bool dragging = false;
    public float wheel = 0;
    public Vector3 rotationPoint;

    private void OnRotate()
    {
        //回転の取得
        wheel += Input.GetAxis("Mouse ScrollWheel")*300;
        if (((int)wheel / 90) * 90 != 0)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), ((int)wheel / 90) * 90);
            wheel = 0;
        }
    }

    public bool CanPieceBePlaced()
    {
        //おけるマスかどうかの判定を行う

        return canBePlaced;
    }

    public Vector2 GetNearestCell()
    {
        //OnMouseDragで使用した変数worldPositionからマウスの位置を取得し，整数にしてreturn
        
        Vector2 nearestCell = new Vector2(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
        Debug.Log(nearestCell);
        return nearestCell;
    }

    public void MoveToInitialPosition()
    {
        this.transform.position = initialPosition;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            OnRotate();
        }
    }
}
