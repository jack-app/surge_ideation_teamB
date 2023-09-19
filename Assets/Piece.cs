using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject tester;
    public GameManager manager;
    public Vector2 initialPosition = Vector2.zero;
    public Vector3 worldPosition = Vector3.zero;
    public bool dragging = false;
    public float wheel = 0;
    public Vector3 rotationPoint;
    public int max_x = 0;
    public int max_y = 0;

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

    public void OnSet()
    {
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x + 0.5f) - 0.5f;
            float roundY = Mathf.Round(children.transform.position.y + 0.5f) - 0.5f;
            manager.board[(int)roundX, (int)roundY] = true;
        }
    }

    public void OnRemove()
    {
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x + 0.5f) - 0.5f;
            float roundY = Mathf.Round(children.transform.position.y + 0.5f) - 0.5f;
            if (roundX < 0 || roundX >= max_x || roundY < 0 || roundY >= max_y) continue;
            manager.board[(int)roundX, (int)roundY] = false;
        }
    }

    public bool CanPieceBePlaced()
    {
        //������}�X���ǂ����̔�����s��
        //Todo �}�X�̒u����͈͂�GameManager�������Ă���
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x+0.5f)-0.5f;
            float roundY = Mathf.Round(children.transform.position.y+0.5f)-0.5f;

            // mino���X�e�[�W���͂ݏo���Ȃ��悤�ɐ���
            if (roundX < 0 || roundX >= max_x || roundY < 0 || roundY >= max_y)
            {
                return false;
            }
            if (manager.board[(int)roundX,(int)roundY])
            {
                return false;
            }
        }
        return true;
    }

    public Vector2 GetNearestCell()
    {
        //OnMouseDrag�Ŏg�p�����ϐ�worldPosition����}�E�X�̈ʒu���擾���C�ł��߂��Z���̒��S�̍��W��Ԃ�
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
        tester = GameObject.Find("Tester");
        manager = tester.GetComponent<GameManager>();

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
