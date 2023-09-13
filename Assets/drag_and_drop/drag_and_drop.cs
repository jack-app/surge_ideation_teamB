using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class drag_and_drop : MonoBehaviour
{
    Vector3 worldPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {


        
        Vector3 thisPosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
        worldPosition.z = 0f;
        this.transform.position = worldPosition;

    }
    private void OnMouseUp()
    {
        
        Vector2 putPosition = new Vector2(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
        this.transform.position = putPosition;

    }
    
}
