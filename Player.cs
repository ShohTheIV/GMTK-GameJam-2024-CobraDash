using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }

    public float speed = 7; // Speed of the snake
    public float turnspeed = 100f;
    private Rigidbody2D rb;

    public float dashDistance = 100f;

    // Tail ----------------------------------------------------------------

    [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();  // Length of the tail. This is not used to make the tail move! Its used to instantiate the tail
    private List<GameObject> snakeBody = new List<GameObject>(); // The actual body where the tail moves. Make sure to add the head of the snake to this list for this to work

    private List<GameObject> body = new List<GameObject>();

    [SerializeField] private float distanceBetween;
    [SerializeField] private Transform bodyParent;

    private GameObject tail;

    float countUp = 0;
    float dashCoolDown = 0f;

    // Dashing ----------------------------------------------------------------
    private bool isDashing;
    public float dashTime;
    private Vector2 dashDirection;
    bool canDash = true;

    List<int> tailHealth = new List<int>();

    // Collision ----------------------------------------------------------------
    public Transform orientation;

    public GameObject particles;


    private void Awake() {
        Instance = this;
        tail = bodyParts[1];
        body = new List<GameObject>(bodyParts);

        for (int i = 0; i < bodyParts.Count; i++){
            tailHealth.Add(0);
        }
    }

    void Start()
    {
        distanceBetween = 0.722449f/speed; // This equation automatically calculates the distance between snake bodies
        // Adding the head to the body of the snake for the tail to follow
        snakeBody.Add(gameObject);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Don't touch this
        if(bodyParts.Count > 0) CreateBodyPart();
        
        Move();

        if(isDashing){
            if (dashCoolDown > 0){
                rb.velocity = dashDirection;
            }else{
                EndDash();
            }
        }
    }

    private void Update() {
        if(Input.GetAxisRaw("Vertical") == 1 && canDash && bodyParts.Count == 0){
            Dash(transform.up);
        }
        dashCoolDown -= Time.deltaTime;

    }

    void Move(){
        if(isDashing) return;
        // This can be changed
        rb.velocity = snakeBody[0].transform.up * speed;

        if(Input.GetAxis("Horizontal") != 0){
            transform.Rotate(0, 0, Input.GetAxis("Horizontal") * -turnspeed);
        }
        // This is to update the tail to follow the head
        if(snakeBody.Count > 1){
            for(int i = 1; i < snakeBody.Count; i++){
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void Dash(Vector2 dir){
        canDash = false;
        dashCoolDown = dashTime;
        dashDirection = dir * dashDistance;
        isDashing = true;

        
        GameObject bruh = Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(bruh, 1f);
        bodyParts.Clear();
        snakeBody.RemoveAt(0);
        foreach(Transform g in bodyParent.GetComponentsInChildren<Transform>()){
            if(g != bodyParent){
                Destroy(g.gameObject);
            }
        }
        // bodyParts.AddRange(snakeBody);

        tailHealth.Clear();
        for (int i = 0; i < snakeBody.Count; i++)
        {
            bodyParts.Add(tail);
            tailHealth.Add(snakeBody[i].GetComponent<Tail>().health); 
        }

        dashCoolDown = dashTime;

        countUp = 0;
        snakeBody = new List<GameObject>
        {
            gameObject
        };
    }

    void EndDash(){
        isDashing = false;
        Invoke(nameof(RechargeDash), 0.5f);
        CreateBodyPart();
    }

    void RechargeDash(){
        canDash = true;
    }

    // Creating the Tail, each tail object needs to have a MarkerManager component
    void CreateBodyPart(){
        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if (countUp == 0) markM.ClearMarkerList();
        GetComponent<MarkerManager>().ClearMarkerList();

        countUp += Time.deltaTime;

        if (countUp >= distanceBetween){
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, bodyParent);
            temp.GetComponent<Tail>().SetHealth(tailHealth[snakeBody.Count - 1]);
            temp.transform.localScale = new Vector3(1, 1, 1);

            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Wall"){
            // Apply a force to the snake to knock it back
            KnockBack(-transform.up);
            TakeDamage();
        }
        else if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<Enemy>().Kill();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Apple"){
            AddHealth(1);
            DungeonGenerator.Instance.dungeon.Add(other.gameObject);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Respawn"){
            DungeonGenerator.Instance.GenerateDungeon();
        }
    }

    void KnockBack(Vector2 dir){
        rb.AddForce(dir * 9000);
        GameObject bruh = Instantiate(particles, transform.position, Quaternion.identity);

        Destroy(bruh, 1f);
    }

    public Transform GetTransform(){
        return transform;
    }

    public void TailDestroyed(GameObject tail){
        snakeBody.Remove(tail);
        body.Remove(tail);
    }

    public void TakeDamage(){
        if(snakeBody.Count > 2){
            snakeBody[1].GetComponent<Tail>().TakeDamage();
        }else if(snakeBody.Count < 2 && bodyParts.Count < 2){
            Die();
        }
    }

    void Die(){
        Menu.instance.Die();
    }

    public void AddHealth(int h){
        if(snakeBody.Count > 1){
            for(int i = 1; i < snakeBody.Count; i++){
                snakeBody[i].GetComponent<Tail>().AddHealth(h);
            }
        }
    }
}
