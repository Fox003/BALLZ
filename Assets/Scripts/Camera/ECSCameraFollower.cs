using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ECSCameraFollower : MonoBehaviour
{
    public Entity CameraEntity; // Assign this at runtime or via a Baker
    private EntityManager entityManager;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void LateUpdate()
    {
        if (!entityManager.Exists(CameraEntity)) return;

        var transform = entityManager.GetComponentData<LocalToWorld>(CameraEntity);
        this.transform.position = transform.Position;
        this.transform.rotation = transform.Rotation;
    }
}
