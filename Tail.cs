using UnityEngine;

public class Tail : MonoBehaviour
{
    public Color[] colors;
    public int health = 0;
    

    public void TakeDamage(){
        health++;
        if(health >= colors.Length - 1){
            Player.Instance.TailDestroyed(gameObject);
            Destroy(gameObject);
        } 
        GetComponent<SpriteRenderer>().color = colors[health];

    } 

    public void SetHealth(int h){
        health = h;
        GetComponent<SpriteRenderer>().color = colors[health];
    }

    public void AddHealth(int h){
        health -= h;
        if(health < 0){
            health = 0;
        }
        GetComponent<SpriteRenderer>().color = colors[health];
    }
}
