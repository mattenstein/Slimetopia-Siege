using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Script for managing the actual game -- equivalent to main()
/// </summary>
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

        public List<MovementNode>/*MovementNode[]*/ GetNextConnectedNodes()
        {
            List<MovementNode> result = new List<MovementNode>();
            foreach (MovementNode node in nextConnectedNodes)
            {
                result.Add(node);
            }
            return result;
            //return nextConnectedNodes;
        }

        public List<MovementNode> GetPreviousConnectedNodes()
        {
            List<MovementNode> result = new List<MovementNode>();
            foreach (MovementNode node in previousConnectedNodes)
            {
                result.Add(node);
            }
            return result;
        }

        public int[] GetNextNodeIndexes()
        {
            return nextConnectedNodeIndexes;
        }

        public int[] GetPreviousNodeIndexes()
        {
            return previousConnectedNodeIndexes;
        }

        public void SetNextNodeArray(MovementNode[] _nodes)
        {
            nextConnectedNodes = _nodes;
        }

        public void SetPreviousNodeArray(MovementNode[] _nodes)
        {
            previousConnectedNodes = _nodes;
        }

        public void CheckNode()
        {
            Debug.Log(nodePosition.GetXY());
            Debug.Log("NextConnectedNodeIndexes size = " + nextConnectedNodeIndexes.Length);
            Debug.Log("NextConnectedNodes size = " + nextConnectedNodes.Length);
            Debug.Log("PreviousConnectedNodeIndexes size = " + previousConnectedNodeIndexes.Length);
            Debug.Log("PreviousConnectedNodes size = " + previousConnectedNodes.Length);
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

            public Position GetPosition()
            {
                return pos;
            }
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

        public void SetupNodes()
        {
            int j = 0;
            foreach(var node in nodes)
            {
                // get indexes + setup arrays
                int[] tempNextIndexes = node.GetNextNodeIndexes();
                int[] tempPreviousIndexes = node.GetPreviousNodeIndexes();

                //Debug.Log("Element " + j + " @ " + node.GetPosition().GetXY() + "\nNext Indexes size = " + tempNextIndexes.Length + "\nPrevious Indexes size = " + tempPreviousIndexes.Length);
                string debugLog1 = "Element " + j + " @ " + node.GetPosition().GetXY();
                string debugLog2 = "\nNext Indexes size = " + tempNextIndexes.Length + " \nIndexes = ";
                foreach (int index in tempNextIndexes)
                {
                    debugLog2 = debugLog2 + index.ToString() + ", ";
                }
                debugLog2 = debugLog2.Remove(debugLog2.Length - 2);
                string debugLog3 = "\nPrevious Indexes size = " + tempPreviousIndexes.Length + " \nIndexes = ";
                foreach (int index in tempPreviousIndexes)
                {
                    debugLog3 = debugLog3 + index.ToString() + ", ";
                }
                debugLog3 = debugLog3.Remove(debugLog3.Length - 2);

                // only do things if there are indexes in array
                if (tempNextIndexes.Length > 0)
                {
                    MovementNode[] tempNodes = new MovementNode[tempNextIndexes.Length];
                    for (int i = 0; i < tempNextIndexes.Length; i++)
                    {
                        tempNodes[i] = nodes[tempNextIndexes[i]];
                    }
                    node.SetNextNodeArray(tempNodes);
                }
                else node.SetNextNodeArray(null);
                if (tempPreviousIndexes.Length > 0)
                {
                    MovementNode[] tempNodes = new MovementNode[tempPreviousIndexes.Length];
                    for (int i = 0; i < tempPreviousIndexes.Length; i++)
                    {
                        tempNodes[i] = nodes[tempPreviousIndexes[i]];
                    }
                    node.SetPreviousNodeArray(tempNodes);
                }
                else node.SetPreviousNodeArray(null);

                //Debug.Log(debugLog1 + debugLog2 + debugLog3);
                j++;
            }
        }

        public void CheckNode(int _index)
        {
            Debug.Log("Element " + _index + " @ " + nodes[_index].GetPosition().GetXY() + "\n ");
        }

        public void CheckNodes(int _i)
        {
            Debug.Log(nodes[_i].GetPosition().GetXY());
            int[] nextNodeIndexes = nodes[_i].GetNextNodeIndexes();
            if (nextNodeIndexes.Length != 0) CheckNodes(nextNodeIndexes[0]);
        }

        public List<GameObject> SetupTurrets(GameObject _prefab)
        {
            List<GameObject> tempList = new List<GameObject>();
            foreach (var turret in turrets)
            {
                GameObject temp = Instantiate(_prefab, turret.GetPosition().GetXY(), Quaternion.identity);
                temp.AddComponent<TurretManager>();
                temp.GetComponent<TurretManager>().Setup();
                tempList.Add(temp);
            }
            return tempList;
        }
    }

    [SerializeField] Level[] possibleLevels;

    [SerializeField] GameObject goodMinionPrefab;
    [SerializeField] GameObject hostileMinionPrefab;
    [SerializeField] GameObject turretPrefab;

    List<GameObject> goodMinions;
    List<GameObject> hostileMinions;
    List<GameObject> turrets;
    
    //MovementNode[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        // initialise lists
        goodMinions = new List<GameObject>();
        hostileMinions = new List<GameObject>();

        // get nodes for minion pathing
        possibleLevels[0].SetupNodes();
        //possibleLevels[0].CheckNodes(0);
        //nodes = possibleLevels[0].GetNodes();
        ////Debug.Log(nodes.Length);
        //foreach(var node in nodes)
        //{
        //    node.CheckNode();
        //}

        // spawn turrets
        turrets = new List<GameObject>();
        turrets = possibleLevels[0].SetupTurrets(turretPrefab);

        // spawn good minion for testing
        Vector2 tempVec = possibleLevels[0].GetSpawnPoints().GetFriendlySpawnPoints()[0].GetPosition().GetXY();
        GameObject temp = Instantiate(goodMinionPrefab, tempVec, Quaternion.identity, transform);
        temp.AddComponent<MinionManager>();
        temp.GetComponent<MinionManager>().SetupMinion(possibleLevels[0].GetNodes()[0].GetPosition().GetXY());
        goodMinions.Add(temp);

        
    }

    // Update is called once per frame
    void Update()
    {
        // basically reverse everything in this for hostile minions
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

                // make sure target is not the end point
                if (oldTarget != possibleLevels[0].GetNodes()[possibleLevels[0].GetNodes().Length - 1].GetPosition().GetXY())
                {
                    // iterate through nodes to find next node
                    foreach (var node in possibleLevels[0].GetNodes())
                    {
                        // is this the current target node (old node)
                        if (oldTarget == node.GetPosition().GetXY())
                        {
                            // only really does something if tempNodes has more than one node in it, otherwise will just get next node
                            int[] indexes = node.GetNextNodeIndexes();

                            int indexesIndex = Random.Range(0, indexes.Length);
                            int index = indexes[indexesIndex];

                            // update the minion target and reset atTarget check on minion
                            minion.GetComponent<MinionManager>().UpdateTargetPosition(possibleLevels[0].GetNodes()[index].GetPosition().GetXY()/*node.GetNextConnectedNodes()[index].GetPosition().GetXY()*/);
                            minion.GetComponent<MinionManager>().ResetAtTarget();
                            // after target node + atTarget updated break loop to do everything again with next minion
                            break;
                        }
                    }
                }
                else // minion is at target in the final node
                {
                    // do stuff? delete minion? damage enemy champion?
                }
            }
        }

        foreach(var minion in hostileMinions)
        {

        }
    }
}
