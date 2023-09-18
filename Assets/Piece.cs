using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //id,typeはいらないかも
    public List<GameObject> cells;
    public Vector2 initialPosition = Vector2.zero;
    public Vector3 worldPosition = Vector3.zero;
    public bool dragging = false;
    public float wheel = 0;
    public Vector3 rotationPoint;
    public int max_x = 0;
    public int max_y = 0;

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
        //Todo マスの置ける範囲をGameManagerから取ってくる
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x+0.5f)-0.5f;
            float roundY = Mathf.Round(children.transform.position.y+0.5f)-0.5f;

            // minoがステージよりはみ出さないように制御
            if (roundX < 0 || roundX >= max_x || roundY < 0 || roundY >= max_y)
            {
                return false;
            }
        }
        return true;
    }

    public Vector2 GetNearestCell()
    {
        //OnMouseDragで使用した変数worldPositionからマウスの位置を取得し，最も近いセルの中心の座標を返す
        Vector2 nearestCell = new Vector2(Mathf.Round(worldPosition.x+0.5f)-0.5f, Mathf.Round(worldPosition.y+0.5f)-0.5f);
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
