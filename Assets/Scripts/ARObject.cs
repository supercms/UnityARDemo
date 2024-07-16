using UnityEngine;

/*
 * @class
 * @classdesc GameObject가 터치되었을 때 반응에 대한 클래스
 */
public class ARObject : MonoBehaviour
{
    /*
     * 객체 색을 변경하기 위함
     */
    private MeshRenderer meshRenderer;

    /*
     * 객체 터치에 의한 선택 여부 확인하기 위함
     */
    private bool bSelected;

    /*
     * 객체 원래 색을 저장
     */
    private Color originColor;

    /*
     * bSelected의 get, set
     */
    public bool Selected
    {
        get { return bSelected; }
        set
        {
            bSelected = value;
            // 선택되면 매트리얼 색을 변경
            UpdateMaterialColor();
        }
    }

    /*
     * Unity 전용 함수 : 가장 먼저 실행
     */
    void Awake()
    {
        bSelected = false;

        meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer)
        {
            meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        originColor = meshRenderer.material.color;
    }

    /*
     * 매트리얼 색 변경
     * 선택시 : 약간 어두운 색
     * 해재시 : 원래 색으로 복구
     */
    void UpdateMaterialColor()
    {
        if (bSelected)
        {
            meshRenderer.material.color = Color.gray;
        }
        else
        {
            meshRenderer.material.color = originColor;
        }
    }
}