using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDrag : MonoBehaviour
{
     public static bool boxFlag;

    void OnMouseDrag()
    {
　　　　//ドラッグ中は吸い込んではだめ
        boxFlag = true;
        //以下四行はドラッグした時にオブジェクトを動かすコード
        Vector3 thisPosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
        worldPosition.z = 0f;
        this.transform.position = worldPosition;
    }

     void OnMouseUp()
    {
　　　　//ドラッグ終了、吸い込んでよし
        boxFlag = false;
    }
}
