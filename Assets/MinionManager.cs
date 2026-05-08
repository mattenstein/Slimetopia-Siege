using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    Vector2 currentTargetPosition;
    float moveSpeed = 1.0f;
    bool atTarget = false;
    float targetTolerance = 0.1f;

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
}
