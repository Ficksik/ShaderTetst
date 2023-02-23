using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BigRookGames.Scripts.Build_Panels
{
    [Serializable]
    public class BuildStage
    {
        [SerializeField] private List<MeshRenderer> _objectsToDestroy;
        public IReadOnlyList<MeshRenderer> ObjectsToDestroy => _objectsToDestroy;

        public void DestroyObjects()
        {
            for (int i = 0; i < _objectsToDestroy.Count; i++)
            {
                Object.Destroy(_objectsToDestroy[i]);
            }
        }
    }
}