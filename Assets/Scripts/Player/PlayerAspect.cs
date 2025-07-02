using Inputs;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

readonly partial struct PlayerAspect : IAspect
{
    public readonly Entity Self;
    
    public readonly RefRW<LocalTransform> Transform;
    public readonly RefRO<PlayerInputData> PlayerInputData;
    
    public readonly RefRO<PlayerMovementData> PlayerMovementData;
    public readonly RefRW<PlayerVelocityData> PlayerVelocityData;
    
    public readonly RefRW<PhysicsVelocity> PhysicsVelocity;
    public readonly RefRO<PhysicsMass> PhysicsMass;
}
