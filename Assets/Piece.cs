using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public List<GameObject> cells = new List<GameObject>();
    public List<PieceCell> cellScripts = new List<PieceCell>();
    public GameObject tester;
    public GameManager manager;
    public Vector3 initialPosition = Vector3.zero;
    public Vector3 worldPosition = Vector3.zero;
    public bool dragging = false;
    public float wheel = 0;
    public Vector3 rotationPoint;
    public int max_x = 0;
    public int max_y = 0;
    public int rotate = 0;
    public bool canDrug=true; 
    public bool smallen=true;

    private void OnRotate()
    {
        //�ｽ�ｽ]�ｽﾌ取得
        wheel += Input.GetAxis("Mouse ScrollWheel")*300;
        if (((int)wheel / 90) * 90 != 0)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), ((int)wheel / 90) * 90);
            rotate = (rotate + 40 + (int)wheel / 90) % 4;
            wheel = 0;
        }
    }

    public void OnSet()
    {
        //ピースを配置
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x + 0.5f) - 0.5f;
            float roundY = Mathf.Round(children.transform.position.y + 0.5f) - 0.5f;
            manager.board[(int)roundX, (int)roundY] = true;
        }
        manager.OnUpdate();
    }

    public void OnRemove()
    {
        //ピースをボードから取り除く
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x + 0.5f) - 0.5f;
            float roundY = Mathf.Round(children.transform.position.y + 0.5f) - 0.5f;
            if (roundX < 0 || roundX >= max_x || roundY < 0 || roundY >= max_y) continue;
            manager.board[(int)roundX, (int)roundY] = false;
        }
        manager.OnUpdate();
    }

    public bool CanPieceBePlaced()
    {
        foreach (Transform children in transform)
        {
            float roundX = Mathf.Round(children.transform.position.x+0.5f)-0.5f;
            float roundY = Mathf.Round(children.transform.position.y+0.5f)-0.5f;

            // mino�ｽ�ｽ�ｽX�ｽe�ｽ[�ｽW�ｽ�ｽ�ｽﾍみ出�ｽ�ｽ�ｽﾈゑｿｽ�ｽ謔､�ｽﾉ撰ｿｽ�ｽ�ｽ
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
        //OnMouseDrag�ｽﾅ使�ｽp�ｽ�ｽ�ｽ�ｽ�ｽﾏ撰ｿｽworldPosition�ｽ�ｽ�ｽ�ｽ}�ｽE�ｽX�ｽﾌ位置�ｽ�ｽ�ｽ謫ｾ�ｽ�ｽ�ｽC�ｽﾅゑｿｽ�ｽﾟゑｿｽ�ｽZ�ｽ�ｽ�ｽﾌ抵ｿｽ�ｽS�ｽﾌ搾ｿｽ�ｽW�ｽ�ｽﾔゑｿｽ
        Vector2 nearestCell = new Vector2(Mathf.Round(worldPosition.x+0.5f)-0.5f, Mathf.Round(worldPosition.y+0.5f)-0.5f);
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
            if (smallen==false && canDrug){
                this.transform.localScale = Vector3.one;
            }
            smallen = true;
        }
        else{
            if (smallen && canDrug && this.transform.position == initialPosition){
                this.transform.localScale = Vector3.one * 0.5f;
            }
            smallen = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (dragging){
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (dragging){
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }
    }
}
