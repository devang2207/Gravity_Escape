using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pool;

internal class BulletSpawner : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    Queue<GameObject> BulletPool;
    int poolSize = 10;

    [SerializeField]Transform SpawnPoint;
    Bullet bulletScript;

    // Start is called before the first frame update
    void Start()
    {
        BulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet =  Instantiate(bullet,bullet.transform.position,Quaternion.identity);
            newBullet.SetActive(false);
            BulletPool.Enqueue(newBullet);
                
            Debug.Log("Bullet " + i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(BulletPool.Count > 0)
            {
                GameObject dequeuedBullet = BulletPool.Dequeue();
                dequeuedBullet.transform.position = SpawnPoint.position;
                dequeuedBullet.SetActive(true);
            }
            else
            {
                Instantiate(bullet,SpawnPoint.position, Quaternion.identity);

            }
        }
    }
    internal void BulletEnqueue(GameObject bullet)
    {
        bullet.SetActive(false); 
        BulletPool.Enqueue(bullet);
    }

}
