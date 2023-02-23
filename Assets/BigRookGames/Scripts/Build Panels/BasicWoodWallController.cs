using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Scripts.Build_Panels
{
    public class BasicWoodWallController : MonoBehaviour
    {
        [SerializeField] private float _shakeAmount = 0.2f;
        [SerializeField] private float _speedShakeFade = 1;
        [SerializeField] private float _speedShakeGrow = 5;
        
        private static readonly int ShakePoint = Shader.PropertyToID("_ShakePoint");
        private static readonly int ShakeAmount = Shader.PropertyToID("_ShakeAmount");

        private List<MaterialPropertyBlock> _blocks = new List<MaterialPropertyBlock>();
        private List<MeshRenderer> _renders = new List<MeshRenderer>();
        private float _shakeAmountCurrent;
        private bool _isGrow;

        private void Start()
        {
            UpdatePosShader();
        }

        [ContextMenu("UpdatePosShader")]
        private void UpdatePosShader()
        {
            var filters = GetComponentsInChildren<MeshRenderer>();
            foreach (var filter in filters)
            {
                if (filter.material.shader.name == "Unlit/LogShader")
                {
                    var block = new MaterialPropertyBlock();
                    filter.GetPropertyBlock(block);
                    block.SetVector(ShakePoint,transform.position);
                    block.SetFloat(ShakeAmount,_shakeAmountCurrent);
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
                _blocks[i].SetFloat(ShakeAmount,_shakeAmountCurrent);
                _renders[i].SetPropertyBlock(_blocks[i]);
            }
        }

        public void Hit(Vector3 pos)
        {
            for (int i = 0; i < _renders.Count; i++)
            {
                _blocks[i].SetVector(ShakePoint,pos);
                _renders[i].SetPropertyBlock(_blocks[i]);
            }

            _isGrow = true;
        }
    }
}