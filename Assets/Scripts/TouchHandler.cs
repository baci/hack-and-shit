using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchHandler : MonoBehaviour 
{
    Dictionary<int, Touchable> touchIds = new Dictionary<int,Touchable>();

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                if (touchIds.ContainsKey(i))
                    touchIds[i].OnTouchMove(Input.GetTouch(i));
                continue;
            } 
            
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                if (!touchIds.ContainsKey(i)) continue;

                touchIds[i].OnTouchEnd(Input.GetTouch(i));
                touchIds.Remove(i);
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
            RaycastHit info;
            if (Physics.Raycast(ray, out info))
            {
                var touchable = info.transform.GetComponent<Touchable>();
                if (touchable != null)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        touchIds[i] = touchable;
                        touchable.OnTouchDown(Input.GetTouch(i), info.point);
                    }
                }
            }
        }
	}
}
