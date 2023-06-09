/* 
    Dungeon Generation

    Given a dungeon with empty rooms, and a list prefabs
    Instantiates the rooms in the layout following a 
    hierarchy of the rooms.

    Rooms must be added as a pre-order traversal of a 
    binary tree, describing the layout of the dungeon.

    Joaquin Badillo 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DungeonGenerator : MonoBehaviour {
    // Room prefabs sent as a pre-order traversal of a tree
    [SerializeField]
    List<GameObject> roomPrefabs = new List<GameObject>();

    // List of rooms, formatted to make it easier to choose a random room
    private List<List<GameObject>> rooms = new List<List<GameObject>>();

    // Number of variations of each room (we have 3 variations of each room)
    [SerializeField]
    int roomVariations;

    // Number of categories of rooms (Big Battle, Hard Battle, Chest Room, etc.)
    [SerializeField]
    int roomCategoires;

    // A counter to keep track of the room we are inserting
    private int roomCount;

    [SerializeField] AstarPath path;

    [SerializeField]
    List<GameObject> characters = new List<GameObject>();

    [SerializeField]
    Transform playerSpawnPoint;

    [SerializeField] GameObject vcam;

    void Start() {
        Debug.Log("Seed: " + PlayerPrefs.GetInt("seed"));
        Random.InitState(PlayerPrefs.GetInt("seed"));

        // We have a lot of fun with the 4th character (berserkr), so we make it the default
        if (!PlayerPrefs.HasKey("classIndex"))
            PlayerPrefs.SetInt("classIndex", 4);
        else if (PlayerPrefs.GetInt("classIndex") < 1)
            PlayerPrefs.SetInt("classIndex", 4);
        
        GameObject player = Instantiate(characters[PlayerPrefs.GetInt("classIndex") - 1], playerSpawnPoint.position, Quaternion.identity);
        vcam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;

        StartGenerator();
    }

    // Inserts room as a child of the reference, and checks if it should continue inserting
    private void InsertRoom(GameObject reference, GameObject room) {
        bool continueInsert = reference.transform.childCount > 0;
        this.roomCount += 1;
        Instantiate(room, reference.transform);

        if (continueInsert) {
            int randomPosition = Random.Range(0, 2);

            // Choose the children of the reference to insert the room in random order
            GameObject firstRef = reference.transform.GetChild(randomPosition).gameObject;
            GameObject secondRef = reference.transform.GetChild(randomPosition == 0 ? 1 : 0).gameObject;

            // Recursive calls to insert in the left and straight subrooms.
            InsertRoom(firstRef, rooms[roomCount][Random.Range(0, 3)]);
            InsertRoom(secondRef, rooms[roomCount][Random.Range(0, 3)]);
        }
    }

    private void StartGenerator() {
        roomCount = -1;
        GameObject baseRoom = roomPrefabs[0];

        // Format the list of rooms
        for (int i = 0; i < roomCategoires; i++) {
            rooms.Add(new List<GameObject>());
            for (int j = 0; j < roomVariations; j++)
                rooms[i].Add(roomPrefabs[3*i + j + 1]);
        }

        // Call the recursive function to insert rooms, starting with the base
        InsertRoom(gameObject, baseRoom);
        path.Scan();
    }
}
