using System.Collections.Generic;
using Threads.ThreadTypes;
using UnityEngine;

namespace Threads
{
    public class ThreadsExample : MonoBehaviour
    {
        public void Start()
        {
            var list = new List<IThread>
            {
                new CoroutineEx(this),
                new ASyncAwaitEx(),
                new ThreadEx(),
                new JobSystemEx()
            };

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Start();
            }
        }
    }
}