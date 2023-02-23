using System.Threading;
using Unity.Collections;
using UnityEngine;

namespace Threads.ThreadTypes
{
    public class ThreadEx : IThread
    {
        public void Start()
        {
            Thread t = new Thread(ThreadProc);
            t.Start();
        }

        private void ThreadProc()
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
            Debug.LogError("ThreadEx end");
        }
    }
}