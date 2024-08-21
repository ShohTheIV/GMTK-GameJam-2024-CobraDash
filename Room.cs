using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public List<Vector2> connections = new List<Vector2>();
    public BoxCollider2D[] collisions; // up right down left

    private List<BoxCollider2D> disabledCollisions = new List<BoxCollider2D>();

    private List<Enemy> enemiesSpawned = new List<Enemy>();
    private int wavesOfEnemies;
    [SerializeField] private int minWaves,maxWaves;

    public int enemiesInWaves;

    private RoomTemplate template;

    public int[,] grid = new int[10, 10];

    public int roomType; // 1: spawn, 2: battle, 3: end

    public Transform pivot;
    public Transform[] corners;

    public GameObject apple;

    private bool alreadyBattled = false;

    public GameObject portal;

    public void SetUp(){
        foreach(Vector2 v in connections){
            if(v == Vector2.up){
                collisions[0].enabled = false;
                disabledCollisions.Add(collisions[0]);
            }else if(v== Vector2.down){
                collisions[1].enabled = false;
                disabledCollisions.Add(collisions[1]);
            }else if(v == Vector2.right){
                collisions[2].enabled = false;
                disabledCollisions.Add(collisions[2]);
            }else if(v == Vector2.left){
                collisions[3].enabled = false;
                disabledCollisions.Add(collisions[3]);
            }
        }

        if (roomType == 2){
            SetTemplate();
            grid = template.GetGrid();
        }else if (roomType == 3){
            Instantiate(portal, transform.position, transform.rotation);
        }
    }

    public void SetTemplate(){
        template = RoomTemplateDictionary.instance.GetRoomTemplate(connections).GetComponent<RoomTemplate>();

        grid = template.GetGrid();

        GameObject t = Instantiate(template.gameObject, transform.position, template.transform.rotation, transform);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && roomType == 2){
            startBattle();
        }
    }

    void SetupColliders(){
        foreach(BoxCollider2D b in collisions){
            b.enabled = true;
        }
    }

    private void startBattle(){

        if (alreadyBattled == true) return;

        if (roomType == 2){
            Invoke(nameof(SetupColliders), 0.5f);

            wavesOfEnemies = Random.Range(minWaves, maxWaves);
            StartWave();
        }
    }

    List<Vector2> availablePositions (){
        List<Vector2> temp = new List<Vector2>();
        for (int i = 0;i < 10 - 1; i++)
        {
            for (int j = 0; j < 10 - 1; j++)
            {
                if(grid[i, j] == 0){
                    temp.Add(new Vector2(j, i));
                }
            }
        }

        return temp;
    }

    void StartWave(){
        List<Vector2> availablePos = availablePositions();
        for (int i = 0; i < enemiesInWaves - 1; i++)
        {
            Vector2 pos = availablePos[Random.Range(0, availablePos.Count -1)];
            GameObject g = Instantiate(RoomTemplateDictionary.instance.GetEnemy(), pivot.transform);
            g.transform.localPosition = new Vector2(pos.x/10, -pos.y / 10);
            enemiesSpawned.Add(g.GetComponent<Enemy>());
            g.GetComponent<Enemy>().father = this;
            DungeonGenerator.Instance.dungeon.Add(g);

            if (g.transform.position.x<=corners[0].position.x || g.transform.position.x >= corners[1].position.x
                || g.transform.position.y<=corners[1].position.y || g.transform.position.y >= corners[0].position.y){
                    g.GetComponent<Enemy>().Kill();
                }

        }

        wavesOfEnemies--;
    }

    void EndBattle(){
        foreach(BoxCollider2D b in disabledCollisions){
            b.enabled = false;
        }
        enemiesSpawned.Clear();
        wavesOfEnemies = 0;
        List<Vector2> availablePos = availablePositions();

        Vector2 pos = availablePos[Random.Range(0, availablePos.Count -1)];

        GameObject g = Instantiate(apple, pivot.transform);
        g.transform.localPosition = new Vector2(pos.x / 10, -pos.y / 10);
        alreadyBattled = true;

        DungeonGenerator.Instance.dungeon.Add(g);

    }

    public void EnemyKilled(Enemy e){
        enemiesSpawned.Remove(e);
        DungeonGenerator.Instance.dungeon.Remove(e.gameObject);

        if(enemiesSpawned.Count == 0){
            if(wavesOfEnemies > 0){
                StartWave();
            }else{
                EndBattle();
            }
        }
    }

}
