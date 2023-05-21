using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
    [SerializeField]
    List<GameObject> roomPrefabs = new List<GameObject>();
    private List<List<GameObject>> rooms = new List<List<GameObject>>();
    [SerializeField]
    int roomVariations;
    [SerializeField]
    int roomCategoires;
    private GameObject baseRoom;
    private int roomCount;
    void Start() {
        roomCount = -1;
        baseRoom = roomPrefabs[0];

        for (int i = 0; i < roomCategoires; i++) {
            rooms.Add(new List<GameObject>());
            for (int j = 0; j < roomVariations; j++)
                rooms[i].Add(roomPrefabs[3*i + j + 1]);
        }

        InsertRoom(gameObject, baseRoom);
    }

    private void InsertRoom(GameObject reference, GameObject room) {
        bool continueInsert = reference.transform.childCount > 0;
        this.roomCount += 1;
        Instantiate(room, reference.transform);

        if (continueInsert) {
            int randomPosition = Random.Range(0, 2);

            GameObject firstRef = reference.transform.GetChild(randomPosition).gameObject;
            GameObject secondRef = reference.transform.GetChild(randomPosition == 0 ? 1 : 0).gameObject;

            InsertRoom(firstRef, rooms[roomCount][Random.Range(0, 3)]);
            InsertRoom(secondRef, rooms[roomCount][Random.Range(0, 3)]);
        }
    }
}
