using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // 오브젝트 이름
        public string objectName;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject prefab;
        // 몇개를 미리 생성 해놓을건지
        public int count;
    }

    public static ObjectPoolManager instance;

    [SerializeField]
    List<ObjectInfo> objectInfos = new List<ObjectInfo>();

    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();
    public bool IsReady { get; private set; }

    // Singleton 인스턴스 관리
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPoolManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ObjectPoolManager");
                    instance = obj.AddComponent<ObjectPoolManager>();
                }
                DontDestroyOnLoad(instance.gameObject); // 씬 전환 시 유지
            }
            return instance;
        }
    }

    private void Awake()
    {
        // IsReady = false;

        // for (int idx = 0; idx < objectInfos.Count; idx++)
        // {
        //     IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        //     OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

        //     if (goDic.ContainsKey(objectInfos[idx].objectName))
        //     {
        //         Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", objectInfos[idx].objectName);
        //         return;
        //     }

        //     goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
        //     ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

        //     // 미리 오브젝트 생성 해놓기
        //     for (int i = 0; i < objectInfos[idx].count; i++)
        //     {
        //         objectName = objectInfos[idx].objectName;
        //         PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
        //         poolAbleGo.Pool.Release(poolAbleGo.gameObject);
        //     }
        // }

        // Debug.Log("오브젝트풀링 준비 완료");
        // IsReady = true;
    }

    public GameObject GetObject(GameObject go)
    {
        return null;
    }

    public void RemoveObject(GameObject go)
    {

    }
}
