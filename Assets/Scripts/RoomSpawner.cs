using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int direction; //number for each direction 1-4 clockwise (1 is up, 2 is right etc)
    private RoomTemplates templates;
    private int rand;
    public bool hasSpawned = false;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnRoom", 0.5f);
    }

    // Update is called once per frame
    void SpawnRoom()
    {
        if (hasSpawned == false)
        {
            switch (direction)
            {
                //spawn a room with a door to match (so right for left, up for down etc)
                case 1:
                    rand = Random.Range(0, 8);
                    Instantiate(templates.downRooms[rand], transform.position, Quaternion.identity);
                    break;
                case 2:
                    rand = Random.Range(0, 8);
                    Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
                    break;
                case 3:
                    rand = Random.Range(0, 8);
                    Instantiate(templates.upRooms[rand], transform.position, Quaternion.identity);
                    break;
                case 4:
                    rand = Random.Range(0, 8);
                    Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
                    break;
            }
            hasSpawned = true;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().hasSpawned == true)
        {
            Destroy(gameObject);
        }

    }
}
