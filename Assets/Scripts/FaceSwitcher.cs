using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/*
 * 요구되는 클래스를 정의
 * ARFaceManager가 반드시 필요
 */
[RequireComponent(typeof(ARFaceManager))]

/*
 * @class
 * @classdesc 얼굴 매트리얼 교체에 사용되는 클래스
 */
public class FaceSwitcher : MonoBehaviour
{
    /*
     * ARFaceManager를 연결하고 사용하기 위함
     */
    private ARFaceManager faceManager;

    /*
     * 여러 매트리얼중 현재 인덱스를 저장
     */
    private int currentId = 0;

    /*
     * 사용할 얼굴 매트리얼 배열
     * SerializeField, 에디터에서 설정
     */
    [SerializeField]
    private Material[] materials;

    /*
     * 디버그용 텍스쳐 출력
     * SerializeField, 에디터에서 설정
     */
    [SerializeField]
    private Text txtStatus;

    /*
     * nity 전용 함수 : 가장 먼저 실행
     */
    void Awake()
    {
        faceManager = GetComponent<ARFaceManager>();
        faceManager.facePrefab.GetComponent<MeshRenderer>().material = materials[0];
        txtStatus.text = "Awake";
    }

    /*
     * 입력 인덱스에 해당하는 매트리얼로 교체
     * UI Event와 연동하기 위함
     */
    public void OnChangeMaterial(int id)
    {
        currentId = id;

        foreach (var face in faceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material = materials[currentId];
        }

        faceManager.facePrefab.GetComponent<MeshRenderer>().material = materials[currentId];
        // txtStatus.text = "BUTTON CLICKED >> " + id + ", " + faceManager.trackables.count;
    }
}