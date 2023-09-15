using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCell : MonoBehaviour
{
    GameObject parentPiece;
    Piece parentScript;
    public bool isCurrentFlowing=false;
    public bool[] wireInterfase;
    void OnMouseDown()
    {
        parentScript.dragging = true;
    }

    void OnMouseUp()
    {
        if (parentScript.CanPieceBePlaced())
        {
            parentScript.transform.position = parentScript.GetNearestCell();
        }
        else
        {
            parentScript.MoveToInitialPosition();
        }
        parentScript.dragging = false;
    }

    void OnMouseDrag()
    {
        Vector3 thisPosition = Input.mousePosition;
        parentScript.worldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
        parentScript.worldPosition.z = 0f;
        parentScript.transform.position = parentScript.worldPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        parentPiece = transform.parent.gameObject;
        parentScript = parentPiece.GetComponent<Piece>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
