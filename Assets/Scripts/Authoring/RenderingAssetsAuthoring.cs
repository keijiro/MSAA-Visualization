using UnityEngine;
using Unity.Entities;

public class RenderingAssetsAuthoring : MonoBehaviour
{
    public Mesh PointMesh;
    public Mesh LineMesh;
    public Mesh PixelMesh;
    public Mesh TriangleMesh;
    public Material PointMaterial;
    public Material LineMaterial;
    public Material PixelMaterial;
    public Material TriangleMaterial;

    class Baker : Baker<RenderingAssetsAuthoring>
    {
        public override void Bake(RenderingAssetsAuthoring src)
          => AddComponentObject
               (GetEntity(TransformUsageFlags.None), 
                new RenderingAssets()
                  { PointMesh        = src.PointMesh,
                    LineMesh         = src.LineMesh,
                    PixelMesh        = src.PixelMesh,
                    TriangleMesh     = src.TriangleMesh,
                    PointMaterial    = src.PointMaterial,
                    LineMaterial     = src.LineMaterial,
                    PixelMaterial    = src.PixelMaterial,
                    TriangleMaterial = src.TriangleMaterial });
    }
}
