using UnityEngine;

public class Bullet : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Wall"){
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Tail"){
            other.GetComponent<Tail>().TakeDamage();
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player"){
            other.GetComponent<Player>().TakeDamage();
            Destroy(gameObject);
        }

    }
}