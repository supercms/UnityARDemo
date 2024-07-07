using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]
public class FaceSwitcher : MonoBehaviour
{
    private ARFaceManager faceManager;
    private int currentId = 0;

    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private Text txtStatus;

    // Start is called before the first frame update
    void Awake()
    {
        faceManager = GetComponent<ARFaceManager>();
        faceManager.facePrefab.GetComponent<MeshRenderer>().material = materials[0];
        txtStatus.text = "Awake";
    }

    void SwitchFaces()
    {
        foreach (var face in faceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material = materials[currentId];
        }

        currentId = (currentId + 1) % materials.Length;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         txtStatus.text = "TOUCHED";
    //     }

    //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //     {
    //         SwitchFaces();
    //         txtStatus.text = "TOUCHED + Began";

    //     }
    // }

    public void OnChangeMaterial(int id)
    {
        currentId = id;

        foreach (var face in faceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material = materials[currentId];
        }

        faceManager.facePrefab.GetComponent<MeshRenderer>().material = materials[currentId];
        txtStatus.text = "BUTTON CLICKED >> " + id + ", " + faceManager.trackables.count;
    }
}