using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class AccelerationCurveAuthoring : MonoBehaviour
{
    public AnimationCurve _accelerationCurve;
    public int _numberOfSamples;
}

class AccelerationCurveAuthoringBaker : Baker<AccelerationCurveAuthoring>
{
    public override void Bake(AccelerationCurveAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        var blobBuilder = new BlobBuilder(Allocator.Temp);
        ref var sampledCurve = ref blobBuilder.ConstructRoot<SampledCurve>();
        var sampledCurveArray = blobBuilder.Allocate(ref sampledCurve.SampledPoints, authoring._numberOfSamples);
        sampledCurve.NumberOfSamples = authoring._numberOfSamples;

        for (int i = 0; i < authoring._numberOfSamples; i++)
        {
            var samplePoint = (float) i/(authoring._numberOfSamples - 1);
            var sampleValue = authoring._accelerationCurve.Evaluate(samplePoint);
            sampledCurveArray[i] = sampleValue;
        }
        
        var blobAssetReference = blobBuilder.CreateBlobAssetReference<SampledCurve>(Allocator.Persistent);
        var accelerationCurveReference = new AccelerationCurveReference { Value = blobAssetReference };
        
        AddComponent(entity, accelerationCurveReference);
    }
}
