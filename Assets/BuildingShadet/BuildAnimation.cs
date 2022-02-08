using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BuildAnimation : MonoBehaviour
{
    private static readonly int _buildProgressProperty = Shader.PropertyToID("_BuildBrogress");
    [SerializeField] private float _speed = 1;

    private float _delta;
    private Material _material;
    private bool _canGenerate;

    public void StartGeneratingBuilding()
    {
        _canGenerate = true;
    }

    public void DestroyBuilding()
    {
        _canGenerate = false;
        _delta = 0;
        _material.SetFloat(_buildProgressProperty, _delta);
    }


    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().sharedMaterial;
        if (_material.shader.name != "Shader Graphs/BuildingShader")
            Debug.LogError("Please select for building this shader: Graphs/BuildingShader");
    }

    private void Update()
    {
        if (_canGenerate == false)
            return;
        _delta += _speed * Time.smoothDeltaTime;
        _material.SetFloat(_buildProgressProperty, _delta);
    }
}
