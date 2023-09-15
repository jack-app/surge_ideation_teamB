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
    public Vector3 worldPosition = Vector3.zero;
    public bool dragging = false;
    public float wheel = 0;
    public Vector3 rotationPoint;

    private void OnRotate()
    {
        //��]�̎擾
        wheel += Input.GetAxis("Mouse ScrollWheel")*300;
        if (((int)wheel / 90) * 90 != 0)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), ((int)wheel / 90) * 90);
            wheel = 0;
        }
    }

    public bool CanPieceBePlaced()
    {
        //������}�X���ǂ����̔�����s��

        return canBePlaced;
    }

    public Vector2 GetNearestCell()
    {
        //OnMouseDrag�Ŏg�p�����ϐ�worldPosition����}�E�X�̈ʒu���擾���C�����ɂ���return
        
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
