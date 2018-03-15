using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform prefab;
    public float spawnRate = 2f;
    public float spawnChance = 0.5f;

    private float nextSpawn;

    void Start()
    {
        nextSpawn = Time.time + spawnRate;
    }

    void Update()
    {
        if (nextSpawn < Time.time)
        {
            if (Random.value < spawnChance)
            {
                Spawn();
            }
            nextSpawn = Time.time + spawnRate;
        }
    }

    public void Spawn()
    {
        Instantiate(prefab, prefab.position + transform.position, Quaternion.identity);
    }
}
