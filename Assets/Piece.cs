using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int id = 0;
    Type type;
    //Array[PieceCell] cells;
    public Vector2 initialPosition = Vector2.zero;
    public bool canBePlaced = false;
    Vector3 worldPosition = Vector3.zero;

    void OnMouseUp()
    {
        if (CanPieceBePlaced())
        {
            this.transform.position = GetNearestCell();
        }
        else
        {
            MoveToInitialPosition();
        }
    }

     void OnMouseDrag()
    {
        Vector3 thisPosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
        worldPosition.z = 0f;
        this.transform.position = worldPosition;
    }

    private void OnRotate()
    {
        
    }

    bool CanPieceBePlaced()
    {
        //おけるマスかどうかの判定を行う

        return canBePlaced;
    }

    Vector2 GetNearestCell()
    {
        //OnMouseDragで使用した変数worldPositionからマウスの位置を取得し，整数にしてreturn
        
        Vector2 nearestCell = new Vector2(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
        Debug.Log(nearestCell);
        return nearestCell;
    }

    void MoveToInitialPosition()
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
        
    }
}
