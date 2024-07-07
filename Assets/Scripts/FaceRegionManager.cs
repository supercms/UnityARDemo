using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARFaceManager))]
[RequireComponent(typeof(XROrigin))]
public class FaceRegionManager : MonoBehaviour
{
    ARFaceManager faceManager;
    XROrigin xrOrigin;
    NativeArray<ARCoreFaceRegionData> faceRegions;

    public GameObject leftHeadPfb;
    public GameObject rightHeadPfb;
    public GameObject nosePfb;

    GameObject noseObj;
    GameObject leftHeadObj;
    GameObject rightHeadObj;

    // Start is called before the first frame update
    void Start()
    {
        faceManager = GetComponent<ARFaceManager>();
        xrOrigin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        ARCoreFaceSubsystem subSystem = (ARCoreFaceSubsystem)faceManager.subsystem;

        foreach (ARFace face in faceManager.trackables)
        {
            subSystem.GetRegionPoses(face.trackableId, Unity.Collections.Allocator.Persistent, ref faceRegions);

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