using UnityEngine;

public class EnemyCollisionDetectors : MonoBehaviour
{
    public bool isColliding;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall"){
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Wall"){
            isColliding = false;
        }
    }
}
