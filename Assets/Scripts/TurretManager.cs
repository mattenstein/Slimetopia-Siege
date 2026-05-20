using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script for managing each individual turret
/// </summary>
public class TurretManager : MonoBehaviour
{
    GameObject currentTarget;
    List<GameObject> nearbyMinions;
    float turretHealth = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // we check health before doing any targetting shenanigans
        CheckHealth();
        // if turret is dead, it never makes it this far
        UpdateTarget();
    }

    // Setup this turret to be ready for use in game
    public void Setup()
    {
        // Set up whatever needs to be set up
        currentTarget = null;
        nearbyMinions = new List<GameObject>();
    }

    // A minion has entered the circle collider of this turret
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Good minion
        // Need to tell minion to stop in tower range to attack tower
        // Set first minion found this way as target
        // Create list of all minions within range, so when target destroyed we know which minion to target next
        if (collision.tag == "GoodMinion")
        {
            // add this minion to the list of nearby minions for targetting purposes
            nearbyMinions.Add(collision.gameObject);

            // add logic to let each minion know they are within range of a turret and need to stop within range of turret to attack it.
            // give each minion a position within range of turret to move to as their target?
        }
    }

    // A minion has been destroyed
    private void OnTriggerExit2D(Collider2D collision)
    {
        // iterate through list of minions
        foreach (var minion in nearbyMinions)
        {
            // if the minion which has died is the minion in focus on the current iteration
            if (minion.GetInstanceID() == collision.GetInstanceID())
            {
                // this minion is dead, remove from list
                nearbyMinions.Remove(minion);
            }
        }
    }

    // check if we have a target
    void UpdateTarget()
    {
        // if we have no target, see if we can set a target
        if (currentTarget == null)
        {
            // if list of minions in range of turret has any minions in it, make the minion at the start of the list the target
            if (nearbyMinions.Count > 0) currentTarget = nearbyMinions[0];
        }
    }

    // Check if turret is still alive
    void CheckHealth()
    {
        // turret is still alive
        if (turretHealth > 0.0f)
        {

        }
        // turret is not alive
        else
        {
            ThisTurretDeath();
            DestroyImmediate(this.gameObject);
        }
    }

    // This turret has died, let all minions know it is dead so they can move to next target position
    void ThisTurretDeath()
    {
        // let each minion know they can move to next node
        foreach (var minion in nearbyMinions)
        {

        }
    }
}
