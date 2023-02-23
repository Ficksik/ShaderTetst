using UnityEngine;

namespace BigRookGames.Scripts.Build_Panels
{
    public abstract class HitObject: MonoBehaviour
    {
        public abstract void Hit(Vector3 pos);
        public abstract void NextStage();
    }
}