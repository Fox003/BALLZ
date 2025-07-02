using Unity.Entities;

public struct AccelerationCurveReference : IComponentData
{
    public BlobAssetReference<SampledCurve> Value;
}

public struct SampledCurve
{
    public BlobArray<float> SampledPoints;
    public int NumberOfSamples;
}
