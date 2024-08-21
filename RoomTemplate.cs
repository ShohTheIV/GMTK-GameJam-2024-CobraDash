using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    private int[,] grid = new int[10, 10];
    public List<Transform> doors = new List<Transform>();

    public int[,] GetGrid(){
        List<Tuple<Vector2, int>> objects = new List<Tuple<Vector2, int>>();
        foreach(RoomObject b in GetComponentsInChildren<RoomObject>()){
            objects.Add(new Tuple<Vector2, int>(b.getGridPosition(), b.type));
        }

        foreach(Tuple<Vector2, int> b in objects){
            grid[(int)b.Item1.y, (int)b.Item1.x] = b.Item2;
        }

        return grid;
    }

}
