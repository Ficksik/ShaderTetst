using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Threads.ThreadTypes
{
    public class JobSystemEx : IThread
    {
        private struct ParallelForJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<float> input;
            public NativeArray<float> output;

            public void Execute(int index)
            {
                output[index] = input[index] * input[index];
            }
        }
        public void Start()
        {
            // Create the input and output arrays
            int arraySize = 1000000;
            NativeArray<float> inputArray = new NativeArray<float>(arraySize, Allocator.TempJob);
            NativeArray<float> outputArray = new NativeArray<float>(arraySize, Allocator.TempJob);

            // Fill the input array with values
            for (int i = 0; i < arraySize; i++)
            {
                inputArray[i] = i;
            }

            // Create the job
            ParallelForJob job = new ParallelForJob
            {
                input = inputArray,
                output = outputArray
            };

            // Schedule the job
            JobHandle handle = job.Schedule(arraySize, 100);

            // Wait for the job to complete
            handle.Complete();

            // Use the output array
            // for (int i = 0; i < arraySize; i++)
            // {
            //     Debug.Log(outputArray[i]);
            // }

            // Dispose of the arrays to free memory
            inputArray.Dispose();
            outputArray.Dispose();
            
            Debug.LogError("JobSystemEx end");
        }
    }
}