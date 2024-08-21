using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public class Marker{
        public Vector3 position;
        public Quaternion rotation;

        public Marker(Vector3 pos, Quaternion rot){
            position = pos;
            rotation = rot; 
        }
    }

    public List<Marker> markerList = new List<Marker>();

    public void UpdateMarkerList(){
        markerList.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList(){
        markerList.Clear();
        markerList.Add(new Marker(transform.position, transform.rotation)); 
    }

    private void FixedUpdate() {
        UpdateMarkerList();
    }
}
