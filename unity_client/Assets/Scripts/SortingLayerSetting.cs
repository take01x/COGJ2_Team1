using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class SortingLayerSetting : MonoBehaviour {

    [SerializeField]
    private string layerName = "UI";

//    [SerializeField]
//    private int orderInLayer = 0;


    void OnValidate()
    {
        SetupLayerName();
    }

    void Awake()
    {
        SetupLayerName();
    }


    void SetupLayerName()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = layerName;
        }

    }
}
