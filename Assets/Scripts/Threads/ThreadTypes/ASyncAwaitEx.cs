using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace Threads.ThreadTypes
{
    public class ASyncAwaitEx : IThread
    {
        public void Start()
        {
            StartAsync();
        }

        private async void StartAsync()
        {
            await TaskEx();
        }

        private Task TaskEx()
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
            Debug.LogError("ASyncAwaitEx end");
            return Task.CompletedTask;
        }
    }
}