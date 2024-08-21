//----------------------------------------------------------------
// Terrible Dungeon Generation script by ShokTheIV
//----------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using TMPro;

public class DungeonGenerator : MonoBehaviour {

    // Variables ----------------------------------------------------------------
    [Header("Variables")]
    public float roomDistance;
    public int minRoomNumber;
    public int maxRoomNumber;
    public int currentFloor = 0;
    public Animator floor;

    // Objects ------------------------------------------------------------------
    [Header("Objects")]
    public GameObject roomObject;
    public GameObject corridor;
    public GameObject cam;
    public GameObject snake;

    // Lists --------------------------------------------------------------------
    [Header("Lists")]
    public List<Vector2> roomPositions = new List<Vector2>();
    private List<Room> rooms = new List<Room>();
    public List<GameObject> dungeon = new List<GameObject>();
    private readonly Vector2[] directions = {Vector2.down, Vector2.up, Vector2.right, Vector2.left};

    // Singleton ----------------------------------------------------------------
    public static DungeonGenerator Instance;


    public void GenerateDungeon(){

        foreach(GameObject g in dungeon){
            if(g != null) Destroy(g);
        }

        dungeon.Clear();
        rooms.Clear();
        roomPositions.Clear();

        currentFloor++;
        floor.gameObject.GetComponent<TextMeshProUGUI>().text = "Floor: " + currentFloor;
        floor.SetTrigger("newFloor");

        int roomCount = Random.Range(minRoomNumber, maxRoomNumber + 1);
        Vector2 currentPosition = Vector2.zero;
        Vector2 lastDir = Vector2.zero;
        for (int i = 0; i < roomCount; i++) {
            List<Tuple<Vector2, Vector2>> availableDirections = new List<Tuple<Vector2, Vector2>>();
            
            foreach(Vector2 d in directions){
                if(!roomPositions.Contains((d * roomDistance) + currentPosition) && d != lastDir){
                    availableDirections.Add(new Tuple<Vector2, Vector2>(currentPosition, d));
                }
            }
            foreach(Vector2 p in roomPositions){
                foreach(Vector2 d in directions){
                    if(!roomPositions.Contains((d * roomDistance) + p) && d != lastDir){
                        availableDirections.Add(new Tuple<Vector2, Vector2>(p, d));
                    }
                }
            }

            if(availableDirections.Count > 0){
                Tuple<Vector2, Vector2> newPos = availableDirections[Random.Range(0, availableDirections.Count-1)];
                Vector2 current_dir = newPos.Item2;

                currentPosition = newPos.Item1 + (current_dir * roomDistance);

                GameObject newRoom = Instantiate(roomObject, currentPosition, Quaternion.identity);
                roomPositions.Add(currentPosition);
                rooms.Add(newRoom.GetComponent<Room>());
                dungeon.Add(newRoom);

                if(i > 0 && i < roomCount){
                    GameObject newCorridor = Instantiate(corridor, (currentPosition + newPos.Item1) / 2, Quaternion.identity);
                    if (currentPosition.y != newPos.Item1.y) newCorridor.transform.rotation = Quaternion.Euler(0,0, 90);
                    findRoom(newPos.Item1).connections.Add(current_dir);
                    newRoom.GetComponent<Room>().connections.Add(-current_dir);
                    newRoom.GetComponent<Room>().roomType = 2;
                    dungeon.Add(newCorridor);
                }else if (i == 0){
                    snake.transform.position = currentPosition;
                    cam.transform.position = new Vector3(currentPosition.x, currentPosition.y, -10);
                    newRoom.GetComponent<Room>().roomType = 1;
                }

                lastDir = current_dir;
            }
        }

        Room furthestRoom = rooms[0];

        foreach(Room room in rooms){
            if(Vector2.Distance(furthestRoom.transform.position, snake.transform.position) <  Vector2.Distance(room.transform.position, snake.transform.position)){
                furthestRoom = room;
            }
        }

        furthestRoom.roomType = 3;

        foreach(Room room in rooms){
            room.SetUp();
        }
    }

    private void Awake() {
        Instance = this;
        roomDistance = roomObject.transform.localScale.x * 1.1f;
    }

    private Room findRoom(Vector2 pos){
        foreach(Room room in rooms){
            if((Vector2)room.transform.position == pos){
                return room;
            }
        }
        return null;    
    }
}