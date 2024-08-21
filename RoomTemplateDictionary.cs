using System.Collections.Generic;
using UnityEngine;

public class RoomTemplateDictionary : MonoBehaviour
{   
    public static RoomTemplateDictionary instance;

    private void Awake() {
        instance = this;
    }

    [Header("One Sided")] 
    [SerializeField] private GameObject[] u;
    [SerializeField] private GameObject[] d, r, l;
    [Header("Two Sided")] 
    [SerializeField] private GameObject[] ur;
    [SerializeField] private GameObject[] rd, dl, lu;
    [SerializeField] private GameObject[] ud, rl;

    [Header("Three Sided")]
    [SerializeField] private GameObject[] lur;
    [SerializeField] private GameObject[] urd, rdl, dlu;

    [Header("Four Sided")]
    [SerializeField] private GameObject[] urdl;

    [Header("Enemies")]
    [SerializeField] private GameObject[] enemies;

    public GameObject GetRoomTemplate(List<Vector2> connections){
        if(connections.Count == 4){
            return urdl[Random.Range(0, urdl.Length - 1)];
        } 

        if(connections.Count == 1){
            if (connections[0] == Vector2.up) return u[Random.Range(0, u.Length - 1)];
            else if (connections[0] == Vector2.right) return r[Random.Range(0, r.Length - 1)];
            else if (connections[0] == Vector2.down) return d[Random.Range(0, d.Length - 1)];
            else if (connections[0] == Vector2.left) return l[Random.Range(0, l.Length - 1)];
        }

        if(connections.Count == 2){
            if(connections.Contains(Vector2.up) && connections.Contains(Vector2.right)) return ur[Random.Range(0, ur.Length - 1)];
            else if(connections.Contains(Vector2.up) && connections.Contains(Vector2.down)) return ud[Random.Range(0, ud.Length - 1)];
            else if(connections.Contains(Vector2.up) && connections.Contains(Vector2.left)) return lu[Random.Range(0, lu.Length - 1)];
            else if(connections.Contains(Vector2.right) && connections.Contains(Vector2.down)) return rd[Random.Range(0, rd.Length - 1)];
            else if(connections.Contains(Vector2.right) && connections.Contains(Vector2.left)) return rl[Random.Range(0, rl.Length - 1)];
            else if(connections.Contains(Vector2.down) && connections.Contains(Vector2.left)) return dl[Random.Range(0, dl.Length - 1)];
        }

        if(connections.Count == 3){
            if(connections.Contains(Vector2.up) && connections.Contains(Vector2.right) && connections.Contains(Vector2.down)) return urd[Random.Range(0, urd.Length - 1)];
            else if(connections.Contains(Vector2.up) && connections.Contains(Vector2.right) && connections.Contains(Vector2.left)) return lur[Random.Range(0, rdl.Length - 1)];
            else if(connections.Contains(Vector2.up) && connections.Contains(Vector2.down) && connections.Contains(Vector2.left)) return dlu[Random.Range(0, dlu.Length - 1)];
            else if(connections.Contains(Vector2.right) && connections.Contains(Vector2.down) && connections.Contains(Vector2.left)) return rdl[Random.Range(0, rdl.Length - 1)];
        }

        return null;  // Return null if no suitable template is found.  This should never happen.  But it's good practice to add this check.  It will help you find and fix bugs if something goes wrong.  Good luck!  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)  :)
    }

    public GameObject GetEnemy(){
        return enemies[Random.Range(0, enemies.Length - 1)];
    }
}
