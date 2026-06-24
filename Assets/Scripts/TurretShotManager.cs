using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShotManager : MonoBehaviour
{
    GameObject target;
    float damageValue = 1.0f;
    float boltSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // confirm we have not reached targets position
        if (transform.position != target.transform.position)
        {
            // move towards target
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, boltSpeed * Time.deltaTime);
            // should follow target, even if target is moving
        }
    }

    public void Setup(GameObject _target)
    {
        target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // are we overlapping with the intended target?
        if (collision.gameObject.GetInstanceID() == target.GetInstanceID()) // yes - damage target
        {
            // if this would kill the minion, let the minion handle its own destruction
            collision.GetComponent<MinionManager>().DamageThisMinion(damageValue);
            Destroy(this.gameObject);
        }
        // no - do nothing
    }
}
