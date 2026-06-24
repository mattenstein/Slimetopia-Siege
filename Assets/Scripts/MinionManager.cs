using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for managing each individual minion
/// </summary>
public class MinionManager : MonoBehaviour
{
    Vector2 currentTargetPosition;
    float moveSpeed = 0.5f;
    bool atTarget = false;
    float targetTolerance = 0.1f;
    float minionHealth = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTargetPosition();
    }

    public void SetupMinion(Vector2 _targetPosition)
    {
        UpdateTargetPosition(_targetPosition);
    }

    public void UpdateTargetPosition(Vector2 _targetPosition)
    {
        currentTargetPosition = _targetPosition;
    }

    void MoveTowardsTargetPosition()
    {
        if (!atTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, currentTargetPosition) < targetTolerance) atTarget = true;
        }
    }

    public bool IsTargetPositionReached()
    {
        return atTarget;
    }

    public void ResetAtTarget()
    {
        atTarget = false;
    }

    public Vector2 GetTargetPosition()
    {
        return currentTargetPosition;
    }

    public void DamageThisMinion(float _damageValue)
    {
        Debug.Log("Damaged");
        minionHealth -= _damageValue;
    }

    // function to let game manager kill minions and remove from list. True == alive, false == dead
    public bool HealthCheck()
    {
        // should minion be dead?
        if (minionHealth <= 0.0f)
        {
            // minion should be dead
            return false;
        }
        return true;
    }
}
