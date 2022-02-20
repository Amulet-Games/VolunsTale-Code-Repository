/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/IJobParallelFor_UpdateAngle_Dis_Dir")]
    public class IJobParallelFor_UpdateAngle_Dis_Dir : StateActions
    {
        public AIStateArrayVariable total_ai_state_array;

        public State TargetState;

        int ai_state_array_index;

        public override void Execute(StateManager statesVariable, AIStateManager aIState)
        {
            int total_ai_state_amount = total_ai_state_array.value.Count;
            ai_state_array_index = 0;

            if (total_ai_state_amount == 0)
                return;

            List<AIStateManager> ai_state_array = new List<AIStateManager>();
            for (int i = 0; i < total_ai_state_amount; i++)
            {
                if(total_ai_state_array.value[i].currentState == TargetState)
                {
                    ai_state_array_index++;
                    ai_state_array.Add(total_ai_state_array.value[i]);
                }
            }

            // Create Native Arrays
            NativeArray<float> native_dis_array = new NativeArray<float>(ai_state_array.Count, Allocator.TempJob);
            NativeArray<float> native_angle_array = new NativeArray<float>(ai_state_array.Count, Allocator.TempJob);
            NativeArray<float> native_exit_aggro_thers_array = new NativeArray<float>(ai_state_array.Count, Allocator.TempJob);
            NativeArray<float3> native_estate_pos_array = new NativeArray<float3>(ai_state_array.Count, Allocator.TempJob);
            NativeArray<float3> native_estate_forward_array = new NativeArray<float3>(ai_state_array.Count, Allocator.TempJob);
            NativeArray<float3> native_dir_array = new NativeArray<float3>(ai_state_array.Count, Allocator.TempJob);
            NativeArray<float3> native_state_pos_array = new NativeArray<float3>(ai_state_array.Count, Allocator.TempJob);

            // Update the aiManager[i] value to Native Array
            for (int i = 0; i < ai_state_array_index; i++)
            {
                native_dis_array[i] = ai_state_array[i].aiManager.distanceToTarget.value;
                native_angle_array[i] = ai_state_array[i].aiManager.angleToTarget;
                native_exit_aggro_thers_array[i] = ai_state_array[i].aiManager.aggro_Thershold;
                native_estate_pos_array[i] = ai_state_array[i].mTransform.savablePosition;
                native_estate_forward_array[i] = ai_state_array[i].mTransform.forward;
                native_dir_array[i] = ai_state_array[i].aiManager.dirToTarget;
                native_state_pos_array[i] = ai_state_array[i].aiManager.currentPlayer.value.mTransform.savablePosition;
            }

            // Create a Job Instance
            UpdateAngleDisDirJob updateAngleDisDirJob = new UpdateAngleDisDirJob
            {
                dis_job_array = native_dis_array,
                angle_job_array = native_angle_array,
                dir_job_array = native_dir_array,
                estate_pos_job_array = native_estate_pos_array,
                estate_foward_job_array = native_estate_forward_array,
                exit_aggro_thers_job_array = native_exit_aggro_thers_array,
                statePos = native_state_pos_array
            };

            // Schedule the Job, Complete the Job
            JobHandle updateAngleDisDirJobHandler = updateAngleDisDirJob.Schedule(ai_state_array_index, 1);
            updateAngleDisDirJobHandler.Complete();

            // Receive New Native Arrays Value to Update aiManager[i]
            for (int i = 0; i < ai_state_array_index; i++)
            {
                ai_state_array[i].aiManager.distanceToTarget.value = native_dis_array[i];
                ai_state_array[i].aiManager.angleToTarget = native_angle_array[i];
                ai_state_array[i].aiManager.aggro_Thershold = native_exit_aggro_thers_array[i];
                ai_state_array[i].mTransform.savablePosition = native_estate_pos_array[i];
                ai_state_array[i].mTransform.forward = native_estate_forward_array[i];
                ai_state_array[i].aiManager.dirToTarget = native_dir_array[i];
            }

            // Dispose Native Arrays
            native_dis_array.Dispose();
            native_angle_array.Dispose();
            native_exit_aggro_thers_array.Dispose();
            native_estate_pos_array.Dispose();
            native_estate_forward_array.Dispose();
            native_dir_array.Dispose();
            native_state_pos_array.Dispose();
        }
    }
    
    [BurstCompile]
    public struct UpdateAngleDisDirJob : IJobParallelFor
    {
        public NativeArray<float> dis_job_array;
        public NativeArray<float> angle_job_array;
        public NativeArray<float3> dir_job_array;
        public NativeArray<float3> estate_pos_job_array;
        public NativeArray<float3> estate_foward_job_array;
        public NativeArray<float3> statePos;
        public NativeArray<float> exit_aggro_thers_job_array;

        public void Execute(int index)
        {
            // DISTANCE
            dis_job_array[index] = Vector3.Distance(statePos[index], estate_pos_job_array[index]);
            if (dis_job_array[index] > exit_aggro_thers_job_array[index])
                return;

            // ANGLE
            angle_job_array[index] = Vector3.Angle(estate_foward_job_array[index], dir_job_array[index]);

            // DIRECTION
            dir_job_array[index] = statePos[index] - estate_pos_job_array[index];
            dir_job_array[index] = new float3(dir_job_array[index].x, 0, dir_job_array[index].z);
            float3 vector3Zero = new float3(0, 0, 0);

            if (math.all(dir_job_array[index] == vector3Zero))
                dir_job_array[index] = estate_foward_job_array[index];
        }
    }
    
}
*/