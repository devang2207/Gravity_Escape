using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int speed = 30;
    Rigidbody rb;
    private BulletSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        spawner = FindObjectOfType<BulletSpawner>();
    }
    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector3.up * speed *Time.deltaTime, ForceMode.Impulse);    
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Bullet")||collision.collider.CompareTag("Player"))
        {
           spawner.BulletEnqueue(this.gameObject);
        }
    }
}
