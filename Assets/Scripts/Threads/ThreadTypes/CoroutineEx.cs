using System.Collections;
using Unity.Collections;
using UnityEngine;

namespace Threads.ThreadTypes
{
    public class CoroutineEx : IThread
    {
        private readonly MonoBehaviour _mono;

        public CoroutineEx(MonoBehaviour mono)
        {
            _mono = mono;
        }
        public void Start()
        {
            _mono.StartCoroutine(Core());
        }

        private IEnumerator Core()
        {
            int arraySize = 1000000;
            NativeArray<float> inputArray = new NativeArray<float>(arraySize, Allocator.TempJob);
            NativeArray<float> outputArray = new NativeArray<float>(arraySize, Allocator.TempJob);

            // Fill the input array with values
            for (int i = 0; i < arraySize; i++)
            {
                inputArray[i] = i;
            }

            for (int i = 0; i < arraySize; i++)
            {
                outputArray[i] = inputArray[i] * inputArray[i];
            }
            Debug.LogError("CoroutineEx end");
            yield return null;
        }
    }
}