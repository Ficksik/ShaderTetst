using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Scripts.Build_Panels
{
    public class BasicWoodWallController : MonoBehaviour
    {
        [SerializeField] private float _destroyTime =1f;
        [SerializeField] private float _shakeAmount = 0.2f;
        [SerializeField] private float _speedShakeFade = 1;
        [SerializeField] private float _speedShakeGrow = 5;
        [SerializeField] private BuildStage[] _stages;
        
        private static readonly int ShakePoint = Shader.PropertyToID("_ShakePoint");
        private static readonly int ShakeAmount = Shader.PropertyToID("_ShakeAmount");

        private List<MaterialPropertyBlock> _blocks = new List<MaterialPropertyBlock>();
        private List<MeshRenderer> _renders = new List<MeshRenderer>();
        private float _shakeAmountCurrent;
        private bool _isGrow;
        private Vector3 _formard;
        private static readonly int ForwardNormal = Shader.PropertyToID("_ForwardNormal");
        private Transform _transform;
        private int _stageNum;
        private static readonly int DropAnimation = Shader.PropertyToID("_DropAnimation");

        private void Start()
        {
            _stageNum = 0;
            _transform = transform;
            _formard = _transform.forward;
            
            UpdatePosShader();
        }

        [ContextMenu("UpdatePosShader")]
        private void UpdatePosShader()
        {
            _transform ??= transform;
            var filters = GetComponentsInChildren<MeshRenderer>();
            foreach (var filter in filters)
            {
                if (filter.material.shader.name == "Unlit/LogShader")
                {
                    var block = new MaterialPropertyBlock();
                    filter.GetPropertyBlock(block);
                    block.SetVector(ShakePoint,_transform.position);
                    block.SetFloat(ShakeAmount,_shakeAmountCurrent);
                    block.SetVector(ForwardNormal,_formard);
                    filter.SetPropertyBlock(block);
                        
                    _renders.Add(filter);
                    _blocks.Add(block);
                };
            }
        }

        private void Update()
        {
            if(!_isGrow && _shakeAmountCurrent<=0) return;
            if (_shakeAmountCurrent> _shakeAmount)
            {
                _shakeAmountCurrent = _shakeAmount;
            }
            if (_isGrow && _shakeAmountCurrent < _shakeAmount)
            {
                _shakeAmountCurrent += Time.deltaTime * _speedShakeGrow;
            }
            else
            {
                _isGrow = false;
                _shakeAmountCurrent -= Time.deltaTime * _speedShakeFade;
            }
            
            for (int i = 0; i < _renders.Count; i++)
            {
                if(_renders[i] == null) continue;
                _renders[i].GetPropertyBlock(_blocks[i]);
                _blocks[i].SetFloat(ShakeAmount,_shakeAmountCurrent);
                _renders[i].SetPropertyBlock(_blocks[i]);
            }
        }

        public void Hit(Vector3 pos)
        {
            for (int i = 0; i < _renders.Count; i++)
            {
                if(_renders[i] == null) continue;
                _renders[i].GetPropertyBlock(_blocks[i]);
                _blocks[i].SetVector(ShakePoint,pos);
#if UNITY_EDITOR
                _blocks[i].SetVector(ForwardNormal,_transform.forward);
#endif
                _renders[i].SetPropertyBlock(_blocks[i]);
            }

            _isGrow = true;
        }

        public void NextStage()
        {
            _stageNum++;
            if(_stages.Length <= _stageNum) return;
            var lastStage = _stages.Length - 1 == _stageNum;
            if (lastStage)
            {
                Destroy(gameObject);
                return;
            }
            StartCoroutine(DestroyAnimate());
        }

        private IEnumerator DestroyAnimate()
        {
            var time = _destroyTime;
            var stage = _stages[_stageNum];
            var block = new MaterialPropertyBlock();
            
            while (time > 0)
            {
                time -= Time.deltaTime;
                var amount = 1-time / _destroyTime;
                for (int i = 0; i < stage.ObjectsToDestroy.Count; i++)
                {
                    var render = stage.ObjectsToDestroy[i];
                    render.GetPropertyBlock(block);
                    block.SetFloat(DropAnimation,amount);
                    render.SetPropertyBlock(block);
                }
                yield return null;
            }

            stage.DestroyObjects();
        }
    }
}