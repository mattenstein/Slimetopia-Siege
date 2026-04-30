using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Level manager stuff:
    [System.Serializable]
    private class Level
    {
        [System.Serializable]
        public struct Position
        {
            [SerializeField] public float x;
            [SerializeField] public float y;
        }

        [System.Serializable]
        public struct MovementNode
        {
            [SerializeField, Tooltip("Local x & y position (Relative to 'Grid' in heirarchy) - Middle of grid tile\nOnly valid pathways required")] Position nodePosition;
            [SerializeField] int[] nextConnectedNodeIndexes;
            [SerializeField] int[] previousConnectedNodeIndexes;
            
            MovementNode[] nextConnectedNodes;
            MovementNode[] previousConnectedNodes;
        }

        [System.Serializable]
        public struct SpawnPoints
        {
            [System.Serializable]
            public struct SpawnPoint
            {
                [SerializeField] public Position pos;
            }

            [SerializeField] SpawnPoint[] friendlyPoints;
            [SerializeField] SpawnPoint[] hostilePoints;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
