using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Singleton;

    [SerializeField] List<GameObject> startPartList;
    [SerializeField] List<GameObject> allParts = new List<GameObject>();

    Queue<GameObject> partsList = new Queue<GameObject>();

    float speed = 5f;
    public float Speed { get { return speed; } }

    bool gameover = false;
    GameObject lastPart;

    private void Start()
    {
        Singleton = this;
        foreach (GameObject part in startPartList)
        { 
            partsList.Enqueue(part);
            lastPart = part;
        }
    }

    void Update()
    {
        if (gameover)
            return;

        if (partsList.Count < 4)
        {
            while (partsList.Count < 4)
            {
                int rnd = Random.Range(0, allParts.Count);
                var temp = Instantiate(allParts[rnd], transform);
                //instantiate new part near the last part
                Debug.Log($"{lastPart.transform.position.x} | {lastPart.GetComponent<Renderer>().bounds.size.x/2} | {temp.GetComponent<Renderer>().bounds.size.x / 2}");
                temp.transform.position = new Vector3(lastPart.transform.position.x + lastPart.GetComponent<Renderer>().bounds.size.x/2 + temp.GetComponent<Renderer>().bounds.size.x/2, 0, 0);
                partsList.Enqueue(temp);
                lastPart = temp;
            }
        }

        foreach (GameObject part in partsList)
        {
            part.transform.position -= new Vector3(Speed * Time.deltaTime, 0, 0);
        }

        if (partsList.Peek().transform.position.x + partsList.Peek().GetComponent<Renderer>().bounds.size.x / 2 < 0 - Screen.width/100/2)
        {
            Destroy(partsList.Dequeue());
        }
    }

    public void GameOver()
    {
        gameover = true;
    }
}
