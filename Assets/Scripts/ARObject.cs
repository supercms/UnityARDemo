using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private bool IsSelected;
    private Color originColor;

    public bool Selected
    {
        get { return IsSelected; }
        set
        {
            IsSelected = value;
            UpdateMaterialColor();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        IsSelected = false;

        meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer)
        {
            meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        originColor = meshRenderer.material.color;
    }

    void UpdateMaterialColor()
    {
        if (IsSelected)
        {
            meshRenderer.material.color = Color.gray;
        }
        else
        {
            meshRenderer.material.color = originColor;
        }
    }
}