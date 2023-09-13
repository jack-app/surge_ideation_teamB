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
        //������}�X���ǂ����̔�����s��

        return canBePlaced;
    }

    Vector2 GetNearestCell()
    {
        //OnMouseDrag�Ŏg�p�����ϐ�worldPosition����}�E�X�̈ʒu���擾���C�����ɂ���return
        
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
