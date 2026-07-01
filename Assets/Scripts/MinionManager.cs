using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for managing each individual minion
/// </summary>
public class MinionManager : MonoBehaviour
{
    Vector2 currentTargetPosition;
    Vector2 currentTowerTargetPosition;
    float moveSpeed = 0.5f;
    bool atTarget = false;
    bool hasTowerTarget = false;
    bool atTowerTarget = false;
    float targetTolerance = 0.1f;
    float minionHealth = 1.0f;
    float attackSpeed = 0.15f;
    float attackDamage = 0.1f;
    float maxAttackRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentTowerTargetPosition = new Vector2();
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
        if (!hasTowerTarget) // not moving towards/at target set by tower
        {
            if (!atTarget) // if we are not at the target, move
            {
                transform.position = Vector2.MoveTowards(transform.position, currentTargetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, currentTargetPosition) < targetTolerance) atTarget = true;
            }
            else // if we are at the target, do not move
            {

            }
        }
        else //moving towards/at target set by tower
        {
            if (!atTowerTarget) // if we are not at the target, move
            {
                transform.position = Vector2.MoveTowards(transform.position, currentTowerTargetPosition, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, currentTowerTargetPosition) < targetTolerance) atTowerTarget = true;
            }
            else // if we are at the target, do not move
            {

            }
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

    public void SetHasTowerTarget(bool _hasTowerTarget)
    {
        if (_hasTowerTarget == false) // if we have no target set by tower (tower destroyed), reset all data relating to a target position set by tower
        {
            atTowerTarget = false;
            UpdateTowerTargetPosition(new Vector2());
        }
        hasTowerTarget = _hasTowerTarget;
    }

    public void UpdateTowerTargetPosition(Vector2 _towerTargetPosition)
    {
        currentTowerTargetPosition = _towerTargetPosition;
    }
}
