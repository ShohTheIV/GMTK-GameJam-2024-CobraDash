using UnityEngine;

public class RoomObject : MonoBehaviour
{   
    public int type;
    public Vector2 getGridPosition(){
        return new Vector2(transform.localPosition.x/2, -transform.localPosition.y/2);
    }
}
