using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

/*
 * 요구되는 클래스를 정의
 * ARFaceManager, XROrigin가 반드시 필요
 */
[RequireComponent(typeof(ARFaceManager))]
[RequireComponent(typeof(XROrigin))]

/*
 * @class
 * @classdesc Unity엔진에서 지원하는 코, 왼쪽및 오른쪽 머리 부분을 원하는 매시로 교체하기 위함
 */
public class FaceRegionManager : MonoBehaviour
{
    /*
     * ARFaceManager를 연결하고 사용하기 위함
     */
    ARFaceManager faceManager;

    /*
     * XROrigin을 연결하고 사용하기 위함
     */
    XROrigin xrOrigin;

    /*
     * Unity에서 제공하는 얼굴 지역 정보를 얻기 위함
     */
    NativeArray<ARCoreFaceRegionData> faceRegions;

    /*
     * (원본) 코, 왼, 오른 머리 prefab
     */
    public GameObject leftHeadPfb;
    public GameObject rightHeadPfb;
    public GameObject nosePfb;

    /*
     * 원본으로 부터 로드된 코, 왼, 오른 헤드 prefab 정보 저장
     */
    GameObject noseObj;
    GameObject leftHeadObj;
    GameObject rightHeadObj;

    /*
     * Unity 전용 함수 : 가장 먼저 실행
     */
    void Awake()
    {
        faceManager = GetComponent<ARFaceManager>();
        xrOrigin = GetComponent<XROrigin>();
    }

    /*
     * Unity 전용 함수 : 매 프레임마다 호출됨
     */
    void Update()
    {
        ARCoreFaceSubsystem subSystem = (ARCoreFaceSubsystem)faceManager.subsystem;

        foreach (ARFace face in faceManager.trackables)
        {
            subSystem.GetRegionPoses(face.trackableId, Unity.Collections.Allocator.Persistent, ref faceRegions);

            // Unity에서는 3군데만 지원
            foreach (ARCoreFaceRegionData faceRegion in faceRegions)
            {
                ARCoreFaceRegion regionType = faceRegion.region;

                switch (regionType)
                {
                    case ARCoreFaceRegion.NoseTip:
                        if (!noseObj)
                        {
                            noseObj = Instantiate(nosePfb, xrOrigin.TrackablesParent);
                        }
                        noseObj.transform.localPosition = faceRegion.pose.position;
                        noseObj.transform.localRotation = faceRegion.pose.rotation;
                        break;
                    case ARCoreFaceRegion.ForeheadLeft:
                        if (!leftHeadObj)
                        {
                            leftHeadObj = Instantiate(leftHeadPfb, xrOrigin.TrackablesParent);
                        }
                        leftHeadObj.transform.localPosition = faceRegion.pose.position;
                        leftHeadObj.transform.localRotation = faceRegion.pose.rotation;
                        break;
                    case ARCoreFaceRegion.ForeheadRight:
                        if (!rightHeadObj)
                        {
                            rightHeadObj = Instantiate(rightHeadPfb, xrOrigin.TrackablesParent);
                        }
                        rightHeadObj.transform.localPosition = faceRegion.pose.position;
                        rightHeadObj.transform.localRotation = faceRegion.pose.rotation;
                        break;

                    default:
                        Debug.LogWarning("Not found region type : " + regionType);
                        break;
                }
            }
        }
    }
}