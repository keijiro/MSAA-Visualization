using UnityEngine;
using Unity.Entities;

public class RenderingAssetsAuthoring : MonoBehaviour
{
    public Mesh PointMesh;
    public Mesh LineMesh;
    public Mesh TriangleMesh;
    public Material PointMaterial;
    public Material LineMaterial;
    public Material TriangleMaterial;

    class Baker : Baker<RenderingAssetsAuthoring>
    {
        public override void Bake(RenderingAssetsAuthoring src)
          => AddComponentObject
               (GetEntity(TransformUsageFlags.None), 
                new RenderingAssets()
                  { PointMesh        = src.PointMesh,
                    LineMesh         = src.LineMesh,
                    TriangleMesh     = src.TriangleMesh,
                    PointMaterial    = src.PointMaterial,
                    LineMaterial     = src.LineMaterial,
                    TriangleMaterial = src.TriangleMaterial });
    }
}
