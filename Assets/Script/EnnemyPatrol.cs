using UnityEngine;

public class EnnemyPatrol : MonoBehaviour
{

    public float speed;
    public Transform[] waypoints;

    public int damageOnCollision = 20;

    public SpriteRenderer graphics;
    private Transform target;
    private int desPoint = 0;
    
    void Start()
    {
        target = waypoints[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // si l'ennemi est bientot arriver a destination
        if(Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            desPoint = (desPoint + 1) % waypoints.Length;
            target = waypoints[desPoint];
            graphics.flipX = !graphics.flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDammage(damageOnCollision);
        }
    }

}
