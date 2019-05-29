﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallex : MonoBehaviour {

class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t) { transform = t; }
        public void Use()
        {
            inUse = true;
        }
        public void Dispose()
        {
            inUse = false;
        }
    }
    [System.Serializable]
    public struct YSpawnRange
    {
        public float min, max;
    }
    public GameObject Prefab;
    public int poolSize;
    public float ShiftSpeed;
    public float SpawnRate;
    public Vector3 defaultSpawnPos;
    public YSpawnRange ySpawnRange;

    public bool spawnImmediate;
    public Vector3 immediatespawnpos;
    public Vector2 targetAspectRatio;
    float SpawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameManager game;
    void Awake()    
    {
        Configure();
    }
    void Start()
    {
        game = GameManager.Instance;    
    }
    void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
    }
   
    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;

    }
    void OnGameOver()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }
    void Update()
    {
        if (game.GameOver) return;
        Shift();
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer > SpawnRate)
        {
            Spawn();
            SpawnTimer = 0;
        }
    }
    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    } 
    void Spawn()
    {
        Transform t = GetPoolObjects();
        if (t == null) return;
        Vector3 pos = Vector3.zero;
        pos.x = defaultSpawnPos.x/**Camera.main.aspect)/targetAspect;*/;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
    }
    void SpawnImmediate()
    {
        Transform t = GetPoolObjects();
        if (t == null) return;
        Vector3 pos = Vector3.zero;
        pos.x = immediatespawnpos.x; /*defaultSpawnPos.x *//** Camera.main.aspect) / targetAspect;*/
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }
    void Shift()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.position += -Vector3.right * ShiftSpeed * Time.deltaTime;
            CheckDesposeObject(poolObjects[i]);
        }
    }
    void CheckDesposeObject(PoolObject poolObject)
    {
        if(poolObject.transform.position.x< (-defaultSpawnPos.x*Camera.main.aspect)/targetAspect)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }
  Transform GetPoolObjects()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse) {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
