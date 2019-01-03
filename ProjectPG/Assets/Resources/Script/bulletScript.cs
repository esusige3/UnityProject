using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField]private float speed;
    private float damage = 5f;
    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collisionObj)
    {
        if (collisionObj.CompareTag("Enemy"))
        {
            collisionObj.GetComponent<EnemyUnit>().takeDmg(damage);
            Destroy(gameObject);
        }
        if (collisionObj.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
