using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * 요구되는 클래스를 정의
 * ARRaycastManager 가 반드시 필요
 */
[RequireComponent(typeof(ARRaycastManager))]

/*
 * @class
 * @classdesc 얼굴 매트리얼 교체에 사용되는 클래스
 */
public class ARObjectController : MonoBehaviour
{
    /*
     * ARRaycastManager 를 연결하고 사용하기 위함
     */
    private ARRaycastManager raycastManager;

    /*
     * ARRaycastHit 로 결과를 받을 변수 리스트
     */
    private List<ARRaycastHit> arHits = new List<ARRaycastHit>();

    /*
     * Unity 전용 RaycastHit 결과를 받을 변수
     */
    private RaycastHit physicsHit;

    /*
     * 스폰된 prefab 을 저장할 gameObject 리스트
     */
    private List<GameObject> spawnedObjects = new List<GameObject>();
    // private Dictionary<string, GameObject> spawnedObjectsDict = new Dictionary<string,GameObject>();

    /*
     * UI 버턴에서 선택된 버턴 인덱스
     */
    int currentID = 0;

    /*
     * Prefab 을 터치해서 선택하면 ARObject Component 를 추가
     */
    ARObject selectedObject;

    /*
     * 디버깅용 텍스트 UI
     */
    [SerializeField]
    Text txtStatus;

    /*
     * UI 버턴에 대응되는 가구 prefab 리스트
     */
    [SerializeField]
    private List<GameObject> furniturePfbList = new List<GameObject>();

    /*
     * AR Camera를 연결해 둬야 한다.
     */
    [SerializeField]
    private Camera arCamera;

    /*
     * Unity 전용 함수 : 가장 먼저 실행
     */
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        //txtStatus.text = "Awake";
    }

    /*
     * Unity 전용 함수 : 매 프레임 호출
     */
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        //txtStatus.text = "TOUCHED";

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        if (IsPointOverUIObject(touchPosition))
            return;

        if (touch.phase == TouchPhase.Began)
        {
            CheckHitObject(touchPosition);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            if (selectedObject)
            {
                selectedObject.Selected = false;
            }
        }

        CheckHitPlane(touchPosition);
    }

    /*
     * 이미 설치된 Gameobject 가 터치되었는지 확인
     */
    bool CheckHitObject(Vector2 touchPos)
    {
        //txtStatus.text = "CheckHitObject";
        Ray ray = arCamera.ScreenPointToRay(touchPos);
        if (Physics.Raycast(ray, out physicsHit))
        {
            selectedObject = physicsHit.transform.GetComponent<ARObject>();
            if (selectedObject)
            {
                selectedObject.Selected = true;
                //txtStatus.text = "CheckHitObject >> TRUE";
                return true;
            }
        }

        return false;
    }

    /*
     * 바닥 혹은 이미 설치된 Gameobject를 터치했는지 확인
     */
    void CheckHitPlane(Vector2 touchPos)
    {
        //txtStatus.text = "CheckHitPlane >>" + touchPos;
        if (raycastManager.Raycast(touchPos, arHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = arHits[0].pose;

            if (selectedObject && selectedObject.Selected)
            {
                selectedObject.transform.position = hitPose.position;
                selectedObject.transform.rotation = hitPose.rotation;
                //txtStatus.text = "CheckHitPlane >> MOVE";
            }
            else
            {
                GameObject go = Instantiate(furniturePfbList[currentID], hitPose.position, hitPose.rotation);
                go.AddComponent<ARObject>();
                spawnedObjects.Add(go);
                //txtStatus.text = "CheckSelectPlane >> ADD";
            }
        }
    }

    /*
     * UI 버턴을 눌럿을 경우는 Raycast가 실행되면 안됨.
     * Update 함수에서 에외 처리
     */
    bool IsPointOverUIObject(Vector2 pos)
    {
        PointerEventData eventDataCurPosition = new PointerEventData(EventSystem.current);
        eventDataCurPosition.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurPosition, results);
        return results.Count > 0;
    }

    /*
     * UI 버턴을 눌렀을 때 인덱스를 받아 현재 인덱스를 변경
     */
    public void changeFurnitureID(int id)
    {
        currentID = id;
    }
}