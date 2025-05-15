

using UnityEngine;

public class PageTurn : MonoBehaviour {

	

	public int direction;



	void OnMouseDown () {

	

        if (direction == 0)
            SendMessageUpwards("TurnToPage", int.Parse(name.Substring(3, 2)));
        else
    		SendMessageUpwards("TurnPage", direction);
		
	}

}