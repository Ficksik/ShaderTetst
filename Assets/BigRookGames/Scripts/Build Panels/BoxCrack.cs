using System;
using BigRookGames.Scripts.Build_Panels;
using UnityEngine;

public class BoxCrack : HitObject
{
    [SerializeField] private int _countStages;
    private int _stageNum;
    private SkinnedMeshRenderer[] _filters;
    private MaterialPropertyBlock _block;
    private static readonly int DropAnimation = Shader.PropertyToID("_DropAnimation");

    private void Start()
    {
        _block = new MaterialPropertyBlock();
        _filters = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public override void Hit(Vector3 pos)
    {
        
    }

    public override void NextStage()
    {
        _stageNum++;
        if(_countStages <= _stageNum)
        {
            Destroy(gameObject);
            return;
        }

        var amount = _stageNum / (float)_countStages;
        foreach (var f in _filters)
        {
            f.GetPropertyBlock(_block);
            _block.SetFloat(DropAnimation,amount);
            f.SetPropertyBlock(_block);
        }
    }
}
