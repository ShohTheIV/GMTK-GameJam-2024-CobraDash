using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    Transform player;
    public float activeDistance;
    Rigidbody2D rb;
    public float speed;
    public EnemyCollisionDetectors[] detectors;
    private RaycastHit2D hit;

    [Header("Shooting")]
    public float reloadSpeed;
    public float shootingDistance;
    private float countUp;
    public GameObject bullet;
    public float bulletSpeed = 10f;
    public Room father;
    public LayerMask playerMask;


    [Header("Combat")]
    public GameObject enemyDeath;

    private void Start() {
        
        player = Player.Instance.GetTransform();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        hit = Physics2D.Raycast(transform.position, player.position - transform.position,Vector2.Distance(player.position, transform.position),playerMask);
        if (Vector2.Distance(player.position, transform.position) <= activeDistance && Vector2.Distance(transform.position, player.position) >= shootingDistance){
            FollowPlayer();
        }
        Look(player.position);
    }

    void FollowPlayer (){
        List<Transform> availbleColliders = new List<Transform>();
        foreach(EnemyCollisionDetectors detector in detectors){
            if(!detector.isColliding){
                availbleColliders.Add(detector.transform);
            }
        }
        if(availbleColliders.Count > 0){
            Transform closest = availbleColliders[Random.Range(0, availbleColliders.Count-1)];
            foreach(Transform t in availbleColliders){
                if(Vector2.Distance(t.position, player.position) < Vector2.Distance(closest.position, player.position)){
                    closest = t;
                }
            }
            
            Move(closest.position - transform.position);
        }
    }

    private void Update() {
        countUp += Time.deltaTime;

        if (countUp >= reloadSpeed && Vector2.Distance(player.position, transform.position) <= shootingDistance){
            Shoot();
        }
    }

    void Shoot(){
        if(!hit) return;

        countUp = 0f;

        GameObject b = Instantiate(bullet, transform.position + (transform.up * 0.388f), Quaternion.identity);
        Rigidbody2D brb = b.GetComponent<Rigidbody2D>();

        Vector2 vel = transform.up * bulletSpeed * Time.deltaTime;
        brb.velocity = vel;
    }

    void Look(Vector3 pos){
        Vector2 dif = pos - transform.position;
        float a = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, a);

    }

    void Move(Vector3 dir){
        rb.velocity = dir.normalized * speed;
    }

    public void Kill(){
        father.EnemyKilled(this);
        GameObject g = Instantiate(enemyDeath, transform.position, Quaternion.identity);
        Destroy(g, 1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == playerMask){
            Kill();
        }
    }
}
