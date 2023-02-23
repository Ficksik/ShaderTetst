using System;
using UnityEngine;

namespace Game.Asteroids
{
    public class ParticleDestory : MonoBehaviour
    {
        public ParticleSystem _particle;

        public void Update() 
        {
            if(_particle)
            {
                if(!_particle.IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
