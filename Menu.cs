using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject[] disableGameStart;

    public TextMeshProUGUI scoreText;
    public GameObject[] enableDeath;

    public static Menu instance;

    private void Awake() {
        instance = this;
    }

    public void Die(){
        foreach (GameObject g in enableDeath){
            g.SetActive(true);
        }

        scoreText.text = "You reached floor : " + DungeonGenerator.Instance.currentFloor;
    }

    public void ClickStart(){
        foreach(GameObject obj in disableGameStart){
            obj.SetActive(false);
        }

        DungeonGenerator.Instance.GenerateDungeon();
    }
}
