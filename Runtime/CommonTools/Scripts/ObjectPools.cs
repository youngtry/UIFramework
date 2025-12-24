using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池对象可以实现此接口来自定义重置逻辑
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// 从对象池取出时调用
    /// </summary>
    void OnSpawn();
    
    /// <summary>
    /// 回收到对象池时调用
    /// </summary>
    void OnDespawn();
}

/// <summary>
/// 通用对象池管理器 - 单例模式
/// 用于管理和复用 GameObject 对象，减少频繁创建和销毁带来的性能开销
/// 优化版本：支持异步预加载、对象重置接口、分帧清理等功能
/// </summary>
public class ObjectPools : MonoBehaviour
{
    private static ObjectPools _instance;
    
    /// <summary>
    /// 单例实例
    /// </summary>
    public static ObjectPools Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ObjectPools");
                _instance = go.AddComponent<ObjectPools>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    /// <summary>
    /// 对象池数据类
    /// </summary>
    private class Pool
    {
        public GameObject prefab;                    // 预制体引用
        public Queue<GameObject> inactiveObjects;    // 未激活的对象队列
        public Transform parent;                     // 对象池父节点
        public int maxSize;                          // 最大容量（0表示无限制）
        public int totalCreated;                     // 已创建的总对象数

        public Pool(GameObject prefab, Transform parent, int maxSize = 0)
        {
            this.prefab = prefab;
            this.inactiveObjects = new Queue<GameObject>();
            this.parent = parent;
            this.maxSize = maxSize;
            this.totalCreated = 0;
        }
    }

    // 存储所有对象池的字典，key为预制体名称
    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 创建对象池（同步方式，可能造成卡顿）
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="initialSize">初始大小</param>
    /// <param name="maxSize">最大容量（0表示无限制）</param>
    public void CreatePool(GameObject prefab, int initialSize = 0, int maxSize = 0)
    {
        if (prefab == null)
        {
            Debug.LogError("ObjectPools: 预制体不能为空！");
            return;
        }

        string poolKey = prefab.name;
        
        if (pools.ContainsKey(poolKey))
        {
            Debug.LogWarning($"ObjectPools: 对象池 '{poolKey}' 已存在！");
            return;
        }

        // 创建对象池父节点
        Transform poolParent = new GameObject($"Pool_{poolKey}").transform;
        poolParent.SetParent(transform);

        Pool pool = new Pool(prefab, poolParent, maxSize);
        pools[poolKey] = pool;

        // 预创建对象
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewObject(pool);
            obj.SetActive(false);
            pool.inactiveObjects.Enqueue(obj);
        }

        Debug.Log($"ObjectPools: 创建对象池 '{poolKey}'，初始大小: {initialSize}");
    }

    /// <summary>
    /// 异步创建对象池（分帧加载，避免卡顿）
    /// 推荐使用此方法来创建大型对象池
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="initialSize">初始大小</param>
    /// <param name="maxSize">最大容量（0表示无限制）</param>
    /// <param name="maxPerFrame">每帧最多创建的对象数量</param>
    /// <returns>协程</returns>
    public IEnumerator CreatePoolAsync(GameObject prefab, int initialSize = 0, int maxSize = 0, int maxPerFrame = 5)
    {
        if (prefab == null)
        {
            Debug.LogError("ObjectPools: 预制体不能为空！");
            yield break;
        }

        string poolKey = prefab.name;
        
        if (pools.ContainsKey(poolKey))
        {
            Debug.LogWarning($"ObjectPools: 对象池 '{poolKey}' 已存在！");
            yield break;
        }

        // 创建对象池父节点
        Transform poolParent = new GameObject($"Pool_{poolKey}").transform;
        poolParent.SetParent(transform);

        Pool pool = new Pool(prefab, poolParent, maxSize);
        pools[poolKey] = pool;

        Debug.Log($"ObjectPools: 开始异步创建对象池 '{poolKey}'，目标大小: {initialSize}");

        // 分帧创建对象
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewObject(pool);
            obj.SetActive(false);
            pool.inactiveObjects.Enqueue(obj);

            // 每帧最多创建 maxPerFrame 个对象
            if ((i + 1) % maxPerFrame == 0)
            {
                yield return null; // 等待下一帧
            }
        }

        Debug.Log($"ObjectPools: 异步创建对象池 '{poolKey}' 完成，实际大小: {pool.inactiveObjects.Count}");
    }

    /// <summary>
    /// 预热对象池（为已存在的对象池添加更多对象）
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="count">要添加的对象数量</param>
    /// <param name="maxPerFrame">每帧最多创建的对象数量</param>
    public IEnumerator WarmupPoolAsync(GameObject prefab, int count, int maxPerFrame = 5)
    {
        if (prefab == null) yield break;

        string poolKey = prefab.name;
        
        if (!pools.ContainsKey(poolKey))
        {
            Debug.LogWarning($"ObjectPools: 对象池 '{poolKey}' 不存在，请先创建！");
            yield break;
        }

        Pool pool = pools[poolKey];

        for (int i = 0; i < count; i++)
        {
            GameObject obj = CreateNewObject(pool);
            obj.SetActive(false);
            pool.inactiveObjects.Enqueue(obj);

            if ((i + 1) % maxPerFrame == 0)
            {
                yield return null;
            }
        }

        Debug.Log($"ObjectPools: 预热对象池 '{poolKey}' 完成，添加了 {count} 个对象");
    }

    /// <summary>
    /// 从对象池获取对象
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转</param>
    /// <returns>对象实例</returns>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("ObjectPools: 预制体不能为空！");
            return null;
        }

        string poolKey = prefab.name;

        // 如果对象池不存在，自动创建
        if (!pools.ContainsKey(poolKey))
        {
            CreatePool(prefab);
        }

        Pool pool = pools[poolKey];
        GameObject obj;

        // 从池中获取对象
        if (pool.inactiveObjects.Count > 0)
        {
            obj = pool.inactiveObjects.Dequeue();
        }
        else
        {
            obj = CreateNewObject(pool);
        }

        // 设置对象状态
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // 调用 IPoolable 接口的 OnSpawn 方法
        IPoolable[] poolables = obj.GetComponents<IPoolable>();
        foreach (var poolable in poolables)
        {
            poolable.OnSpawn();
        }

        return obj;
    }

    /// <summary>
    /// 从对象池获取对象（默认位置和旋转）
    /// </summary>
    public GameObject Spawn(GameObject prefab)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// 从对象池获取对象（指定位置）
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        return Spawn(prefab, position, Quaternion.identity);
    }

    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="obj">要回收的对象</param>
    public void Despawn(GameObject obj)
    {
        Debug.Log("ObjectPools1: 回收对象" + obj.name);
        if (obj == null)
        {
            Debug.LogWarning("ObjectPools: 尝试回收空对象！");
            return;
        }

        // 调用 IPoolable 接口的 OnDespawn 方法
        IPoolable[] poolables = obj.GetComponents<IPoolable>();
        foreach (var poolable in poolables)
        {
            poolable.OnDespawn();
        }

        // 获取对象的原始预制体名称
        string poolKey = obj.name.Replace("(Clone)", "").Trim();

        if (!pools.ContainsKey(poolKey))
        {
            Debug.LogWarning($"ObjectPools: 找不到对象池 '{poolKey}'，直接销毁对象。");
            Destroy(obj);
            return;
        }

        Pool pool = pools[poolKey];

        // 检查是否超过最大容量
        if (pool.maxSize > 0 && pool.inactiveObjects.Count >= pool.maxSize)
        {
            Destroy(obj);
            pool.totalCreated--;
            return;
        }

        Debug.Log("ObjectPools: 回收对象" + obj.name);

        // 重置对象状态
        obj.SetActive(false);
        obj.transform.SetParent(pool.parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        pool.inactiveObjects.Enqueue(obj);
    }

    /// <summary>
    /// 延迟回收对象
    /// </summary>
    /// <param name="obj">要回收的对象</param>
    /// <param name="delay">延迟时间（秒）</param>
    public void Despawn(GameObject obj, float delay)
    {
        StartCoroutine(DespawnAfterDelay(obj, delay));
    }

    private IEnumerator DespawnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Despawn(obj);
    }

    /// <summary>
    /// 清空指定对象池（同步方式）
    /// </summary>
    /// <param name="prefab">预制体</param>
    public void ClearPool(GameObject prefab)
    {
        if (prefab == null) return;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) return;

        Pool pool = pools[poolKey];
        
        // 销毁所有未激活的对象
        while (pool.inactiveObjects.Count > 0)
        {
            GameObject obj = pool.inactiveObjects.Dequeue();
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        pool.totalCreated = 0;
        Debug.Log($"ObjectPools: 清空对象池 '{poolKey}'");
    }

    /// <summary>
    /// 异步清空指定对象池（分帧执行，避免卡顿）
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="maxPerFrame">每帧最多销毁的对象数量</param>
    public IEnumerator ClearPoolAsync(GameObject prefab, int maxPerFrame = 10)
    {
        if (prefab == null) yield break;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) yield break;

        Pool pool = pools[poolKey];
        int count = 0;

        while (pool.inactiveObjects.Count > 0)
        {
            GameObject obj = pool.inactiveObjects.Dequeue();
            if (obj != null)
            {
                Destroy(obj);
                count++;

                if (count >= maxPerFrame)
                {
                    count = 0;
                    yield return null;
                }
            }
        }

        pool.totalCreated = 0;
        Debug.Log($"ObjectPools: 异步清空对象池 '{poolKey}' 完成");
    }

    /// <summary>
    /// 清空所有对象池（同步方式）
    /// </summary>
    public void ClearAllPools()
    {
        foreach (var pool in pools.Values)
        {
            while (pool.inactiveObjects.Count > 0)
            {
                GameObject obj = pool.inactiveObjects.Dequeue();
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            
            if (pool.parent != null)
            {
                Destroy(pool.parent.gameObject);
            }
        }

        pools.Clear();
        Debug.Log("ObjectPools: 清空所有对象池");
    }

    /// <summary>
    /// 异步清空所有对象池（分帧执行，避免卡顿）
    /// </summary>
    /// <param name="maxPerFrame">每帧最多销毁的对象数量</param>
    public IEnumerator ClearAllPoolsAsync(int maxPerFrame = 10)
    {
        int count = 0;
        
        foreach (var pool in pools.Values)
        {
            while (pool.inactiveObjects.Count > 0)
            {
                GameObject obj = pool.inactiveObjects.Dequeue();
                if (obj != null)
                {
                    Destroy(obj);
                    count++;

                    if (count >= maxPerFrame)
                    {
                        count = 0;
                        yield return null;
                    }
                }
            }
            
            if (pool.parent != null)
            {
                Destroy(pool.parent.gameObject);
            }
        }

        pools.Clear();
        Debug.Log("ObjectPools: 异步清空所有对象池完成");
    }

    /// <summary>
    /// 收缩对象池（减少池中未使用的对象数量）
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="targetSize">目标大小</param>
    public void ShrinkPool(GameObject prefab, int targetSize)
    {
        if (prefab == null) return;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) return;

        Pool pool = pools[poolKey];

        while (pool.inactiveObjects.Count > targetSize)
        {
            GameObject obj = pool.inactiveObjects.Dequeue();
            if (obj != null)
            {
                Destroy(obj);
                pool.totalCreated--;
            }
        }

        Debug.Log($"ObjectPools: 收缩对象池 '{poolKey}' 到 {targetSize} 个对象");
    }

    /// <summary>
    /// 异步收缩对象池（分帧执行）
    /// </summary>
    public IEnumerator ShrinkPoolAsync(GameObject prefab, int targetSize, int maxPerFrame = 10)
    {
        if (prefab == null) yield break;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) yield break;

        Pool pool = pools[poolKey];
        int count = 0;

        while (pool.inactiveObjects.Count > targetSize)
        {
            GameObject obj = pool.inactiveObjects.Dequeue();
            if (obj != null)
            {
                Destroy(obj);
                pool.totalCreated--;
                count++;

                if (count >= maxPerFrame)
                {
                    count = 0;
                    yield return null;
                }
            }
        }

        Debug.Log($"ObjectPools: 异步收缩对象池 '{poolKey}' 到 {targetSize} 个对象");
    }

    /// <summary>
    /// 获取对象池中未激活对象的数量
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <returns>对象池中未激活对象的数量</returns>
    public int GetPoolSize(GameObject prefab)
    {
        if (prefab == null) return 0;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) return 0;

        return pools[poolKey].inactiveObjects.Count;
    }

    /// <summary>
    /// 获取对象池已创建的总对象数（包括激活和未激活的）
    /// </summary>
    public int GetTotalCreated(GameObject prefab)
    {
        if (prefab == null) return 0;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) return 0;

        return pools[poolKey].totalCreated;
    }

    /// <summary>
    /// 获取对象池中正在使用的对象数量
    /// </summary>
    public int GetActiveCount(GameObject prefab)
    {
        if (prefab == null) return 0;

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) return 0;

        Pool pool = pools[poolKey];
        return pool.totalCreated - pool.inactiveObjects.Count;
    }

    /// <summary>
    /// 获取对象池统计信息
    /// </summary>
    public string GetPoolStats(GameObject prefab)
    {
        if (prefab == null) return "预制体为空";

        string poolKey = prefab.name;
        if (!pools.ContainsKey(poolKey)) return $"对象池 '{poolKey}' 不存在";

        Pool pool = pools[poolKey];
        int inactive = pool.inactiveObjects.Count;
        int active = pool.totalCreated - inactive;
        
        return $"对象池 '{poolKey}': 总计={pool.totalCreated}, 激活={active}, 未激活={inactive}, 最大容量={pool.maxSize}";
    }

    /// <summary>
    /// 打印所有对象池的统计信息
    /// </summary>
    public void PrintAllPoolStats()
    {
        Debug.Log("========== 对象池统计信息 ==========");
        foreach (var kvp in pools)
        {
            Pool pool = kvp.Value;
            int inactive = pool.inactiveObjects.Count;
            int active = pool.totalCreated - inactive;
            Debug.Log($"[{kvp.Key}] 总计={pool.totalCreated}, 激活={active}, 未激活={inactive}, 最大={pool.maxSize}");
        }
        Debug.Log("===================================");
    }

    /// <summary>
    /// 创建新对象
    /// </summary>
    private GameObject CreateNewObject(Pool pool)
    {
        GameObject obj = Instantiate(pool.prefab, pool.parent);
        obj.name = pool.prefab.name;
        pool.totalCreated++;
        return obj;
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
