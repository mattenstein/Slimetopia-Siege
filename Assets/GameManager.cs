using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Position
    {
        [SerializeField] public float x;
        [SerializeField] public float y;

        public Vector2 GetXY()
        {
            return new Vector2(x, y);
        }
    }

    [System.Serializable]
    public struct MovementNode
    {
        [SerializeField, Tooltip("Local x & y position (Relative to 'Grid' in heirarchy) - Middle of grid tile\nOnly valid pathways required")] Position nodePosition;
        [SerializeField] int[] nextConnectedNodeIndexes;
        [SerializeField] int[] previousConnectedNodeIndexes;

        MovementNode[] nextConnectedNodes;
        MovementNode[] previousConnectedNodes;

        public Position GetPosition()
        {
            return nodePosition;
        }

        public MovementNode[] GetNextConnectedNodes()
        {
            return nextConnectedNodes;
        }

        public MovementNode[] GetPreviousConnectedNodes()
        {
            return previousConnectedNodes;
        }
    }

    // Level manager stuff:
    [System.Serializable]
    private class Level
    {
        //[System.Serializable]
        //public struct Position
        //{
        //    [SerializeField] public float x;
        //    [SerializeField] public float y;
        //}

        //[System.Serializable]
        //public struct MovementNode
        //{
        //    [SerializeField, Tooltip("Local x & y position (Relative to 'Grid' in heirarchy) - Middle of grid tile\nOnly valid pathways required")] Position nodePosition;
        //    [SerializeField] int[] nextConnectedNodeIndexes;
        //    [SerializeField] int[] previousConnectedNodeIndexes;
            
        //    MovementNode[] nextConnectedNodes;
        //    MovementNode[] previousConnectedNodes;
        //}

        [System.Serializable]
        public struct SpawnPoints
        {
            [System.Serializable]
            public struct SpawnPoint
            {
                [SerializeField] public Position pos;

                public Position GetPosition()
                {
                    return pos;
                }
            }

            [SerializeField] SpawnPoint[] friendlyPoints;
            [SerializeField] SpawnPoint[] hostilePoints;

            public SpawnPoint[] GetFriendlySpawnPoints()
            {
                return friendlyPoints;
            }

            public SpawnPoint[] GetHostileSpawnPoints()
            {
                return hostilePoints;
            }
        }

        [System.Serializable]
        public struct Turret
        {
            [SerializeField] Position pos;
        }

        [SerializeField] GameObject backgroundObj;
        [SerializeField] MovementNode[] nodes;
        [SerializeField] SpawnPoints spawns;
        [SerializeField] Turret[] turrets;

        public GameObject GetBackgroundObj()
        {
            return backgroundObj;
        }

        public MovementNode[] GetNodes()
        {
            return nodes;
        }

        public SpawnPoints GetSpawnPoints()
        {
            return spawns;
        }

        public Turret[] GetTurrets()
        {
            return turrets;
        }
    }

    [SerializeField] Level[] possibleLevels;

    [SerializeField] GameObject goodMinion;

    List<GameObject> goodMinions;
    List<GameObject> hostileMinions;
    MovementNode[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        goodMinions = new List<GameObject>();
        hostileMinions = new List<GameObject>();
        Vector2 tempVec = possibleLevels[0].GetSpawnPoints().GetFriendlySpawnPoints()[0].GetPosition().GetXY();
        GameObject temp = Instantiate(goodMinion, tempVec, Quaternion.identity, transform);
        temp.AddComponent<MinionManager>();
        temp.GetComponent<MinionManager>().SetupMinion(nodes[0].GetPosition().GetXY());
        goodMinions.Add(temp);
        nodes = possibleLevels[0].GetNodes();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var minion in goodMinions)
        {
            // iterate through list of minions
            // check if at target
            // if at target, find next node and update target
            if (!minion.GetComponent<MinionManager>().IsTargetPositionReached()) // if we are not at target position
            {
                // do nothing? -- Minion Manager handles movement
            }
            else // if we are at target position
            {
                Vector2 oldTarget = minion.GetComponent<MinionManager>().GetTargetPosition();
                // iterate through nodes to find next node
                foreach (var node in nodes)
                {
                    // is this the current target node (old node)
                    if (oldTarget == node.GetPosition().GetXY())
                    {
                        minion.GetComponent<MinionManager>().UpdateTargetPosition(node.GetNextConnectedNodes()[0].GetPosition().GetXY());
                        minion.GetComponent<MinionManager>().ResetAtTarget();
                        break;
                    }
                }
            }
        }
    }
}
