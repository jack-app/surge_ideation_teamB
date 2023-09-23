using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCell : MonoBehaviour
{
    GameObject parentPiece;
    Piece parentScript;
    public bool isCurrentFlowing=false;
    public List<bool> wireInterfase;
    void OnMouseDown()
    {
        if (!parentScript.canDrug)
        {
            return;
        }
        parentScript.dragging = true;
        parentScript.OnRemove();
    }

    void OnMouseUp()
    {
        if (!parentScript.canDrug)
        {
            return;
        }
        parentScript.dragging = false;
        if (parentScript.CanPieceBePlaced())
        {
            parentScript.OnSet();
            parentScript.transform.position = parentScript.GetNearestCell();
        }
        else
        {
            parentScript.MoveToInitialPosition();
        }
    }

    void OnMouseDrag()
    {
        if (!parentScript.canDrug)
        {
            return;
        }
        Vector3 thisPosition = Input.mousePosition;
        parentScript.worldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
        parentScript.worldPosition.z = -3f;
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
