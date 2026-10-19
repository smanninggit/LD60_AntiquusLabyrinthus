using UnityEngine;

[ExecuteAlways]
public class PivotOffset : MonoBehaviour
{
    [Tooltip("How much to shift the visuals away from the pivot point.")]
    public Vector3 visualOffset = new Vector3(0, 0.5f, 0); 

    private void OnValidate()
    {
        // delayCall forces Unity to wait until OnValidate finishes before destroying the object
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += RefreshPivot;
#endif
    }

    private void RefreshPivot()
    {
        // Safely exit if the object was deleted while waiting for the delayCall
        if (this == null) return; 

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshFilter == null || meshRenderer == null) return;

        // 1. Clean up old clones safely outside of the OnValidate loop
        Transform oldClone = transform.Find("CubeMesh_VisualClone");
        if (oldClone != null)
        {
            DestroyImmediate(oldClone.gameObject); 
        }

        // 2. Hide the original parent mesh renderer
        meshRenderer.enabled = false;

        // 3. Create the new offset child visual object
        GameObject visualChild = new GameObject("CubeMesh_VisualClone");
        visualChild.transform.SetParent(this.transform);
        visualChild.transform.localPosition = visualOffset;
        visualChild.transform.localRotation = Quaternion.identity;
        visualChild.transform.localScale = Vector3.one;

        // 4. Copy the mesh and material data over
        MeshFilter childFilter = visualChild.AddComponent<MeshFilter>();
        MeshRenderer childRenderer = visualChild.AddComponent<MeshRenderer>();

        childFilter.sharedMesh = meshFilter.sharedMesh;
        childRenderer.sharedMaterials = meshRenderer.sharedMaterials;

        // 5. Shift the Box Collider if one exists
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.center = visualOffset;
        }
    }
}
