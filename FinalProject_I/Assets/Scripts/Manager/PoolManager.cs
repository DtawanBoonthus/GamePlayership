using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        public enum PoolObjectType
        {
            PlayerBullet,
            EnemyBullet,
            BossEnemyBullet
        }

        [System.Serializable]
        public class PoolObjectSet
        {
            public PoolObjectType poolObjectType;
            public int amountToPool;
            public GameObject objectToPool;
            public bool shouldExpand;
            public List<GameObject> pooledObjects;
        }

        public List<PoolObjectSet> poolObjectSets;
        private Dictionary<PoolObjectType, PoolObjectSet> poolObjectSetDictionary;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            poolObjectSetDictionary = new Dictionary<PoolObjectType, PoolObjectSet>();
            foreach (PoolObjectSet poolObjectSet in poolObjectSets)
            {
                poolObjectSet.pooledObjects = new List<GameObject>();
                for (int i = 0; i < poolObjectSet.amountToPool; i++)
                {
                    var obj = Instantiate(poolObjectSet.objectToPool);
                    obj.SetActive(false);
                    poolObjectSet.pooledObjects.Add(obj);
                }

                poolObjectSetDictionary.Add(poolObjectSet.poolObjectType, poolObjectSet);
            }
        }

        public GameObject GetPooledObject(PoolObjectType pooledObjectType)
        {
            var poolObjectSet = poolObjectSetDictionary[pooledObjectType];
            var pooledObjects = poolObjectSet.pooledObjects;

            foreach (var pooledObject in pooledObjects)
            {
                if (!pooledObject.activeInHierarchy)
                {
                    return pooledObject;
                }
            }

            if (poolObjectSet.shouldExpand)
            {
                GameObject obj = (GameObject) Instantiate(poolObjectSet.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);

                return obj;
            }

            return null;
        }
    }
}