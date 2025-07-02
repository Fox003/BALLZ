using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(BeforePhysicsSystemGroup))]
partial struct PlayerMovementSystem : ISystem
{
    private EntityQuery _playerQuery;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _playerQuery = SystemAPI.QueryBuilder()
            .WithAspect<PlayerAspect>()
            .Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new PlayerMovementJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel(_playerQuery);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    public partial struct PlayerMovementJob : IJobEntity
    {
        public float DeltaTime;
        void Execute(PlayerAspect playerAspect)
        {
            var physicsVelocity = playerAspect.PhysicsVelocity.ValueRW;
            var playerInputData = playerAspect.PlayerInputData.ValueRO;
            var playerMovementData = playerAspect.PlayerMovementData.ValueRO;
            var playerVelocityData = playerAspect.PlayerVelocityData.ValueRW;
            var physicsMass = playerAspect.PhysicsMass.ValueRO;

            float2 currentVelocity = physicsVelocity.Linear.xz;
            
            // Get the desired velocity from player input
            float2 desiredVelocity = playerInputData.Move * playerMovementData.MaxSpeed;

            float accel = playerMovementData.Acceleration;
            
            // Move the goal velocity towards the desired velocity
            playerVelocityData.GoalVelocity = MoveTowards(playerVelocityData.GoalVelocity, desiredVelocity, accel * DeltaTime);
            
            // Calculate the needed acceleration to reach the goal velocity from the current velocity in 1 frame (dv/dt)
            float2 neededAccel = (playerVelocityData.GoalVelocity - currentVelocity) / DeltaTime;

            float maxAccel = playerMovementData.MaxAccelerationForce;
            
            neededAccel = ClampMagnitude(neededAccel, maxAccel);
            
            // Add force to entity (F = ma)
            physicsVelocity.Linear.xz += (neededAccel * (1 / physicsMass.InverseMass)) * DeltaTime;
            
            playerAspect.PlayerVelocityData.ValueRW = playerVelocityData;
            playerAspect.PhysicsVelocity.ValueRW.Linear = physicsVelocity.Linear;
        }
        
        public static float2 ClampMagnitude(float2 vector, float maxLength)
        {
            float lengthSq = math.lengthsq(vector);
            if (lengthSq > maxLength * maxLength)
            {
                float length = math.sqrt(lengthSq);
                return vector * (maxLength / length);
            }
            return vector;
        }
        
        public static float2 MoveTowards(float2 current, float2 target, float maxDelta)
        {
            float2 delta = target - current;
            float magnitude = math.length(delta);

            if (magnitude <= maxDelta || magnitude == 0f)
                return target;

            return current + delta / magnitude * maxDelta;
        }
    }
}
