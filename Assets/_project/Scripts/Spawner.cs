using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public float spawnDelay;
    private bool spawning;

    [SerializeField] private Transform spawnposition;
    private Transform leftEnd;
    private Transform rightEnd;
    [SerializeField] private Transform enemyParent;
    [SerializeField]

    void Start()
    {
        if (enemies == null)
        {
            Debug.Log("No enemy units found");
        }
        leftEnd = transform.GetChild(0);
        rightEnd = transform.GetChild(1);
        spawning = true;
        StartCoroutine(Spawn());

    }

    IEnumerator Spawn()
    {
        while (spawning)
        {
            float roll = UnityEngine.Random.Range(0, 100);
            GameObject spawn;
            spawn = SpawnShip();
            spawn.transform.parent = enemyParent;

            yield return new WaitForSeconds(spawnDelay);
        }
    }


    private GameObject SpawnShip()
    {
        //Vector3 position = GetRandomPosition();
        GameObject enemy = Instantiate(enemies[0], spawnposition.position, Quaternion.identity);
        return enemy;
    }

    //private Vector3 GetRandomPosition()
    //{
    //    float xPos = UnityEngine.Random.Range(leftEnd.position.x, rightEnd.position.x);
    //    float yPos = UnityEngine.Random.Range(leftEnd.position.y, rightEnd.position.y);
    //    return new Vector3(xPos, yPos, 0);
    //}

}
