using UnityEngine;
using Unity.Entities;

public class RenderingAssetsAuthoring : MonoBehaviour
{
    public Mesh PointMesh;
    public Mesh QuadMesh;
    public Mesh TriangleMesh;
    public Material PointMaterial;
    public Material QuadMaterial;
    public Material TriangleMaterial;

    class Baker : Baker<RenderingAssetsAuthoring>
    {
        public override void Bake(RenderingAssetsAuthoring src)
          => AddComponentObject
               (GetEntity(TransformUsageFlags.None), 
                new RenderingAssets
                  { PointMesh        = src.PointMesh,
                    QuadMesh         = src.QuadMesh,
                    TriangleMesh     = src.TriangleMesh,
                    PointMaterial    = src.PointMaterial,
                    QuadMaterial     = src.QuadMaterial,
                    TriangleMaterial = src.TriangleMaterial });
    }
}
