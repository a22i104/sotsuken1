using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class NavigationLineController : MonoBehaviour
{
    public Transform player;          // 現在地
    public Transform destination;     // 目的地
    public LayerMask groundLayer;     // 地面レイヤー
    public float lineHeight = 0.15f;  // 地面から浮かせる高さ

    private LineRenderer line;
    private NavMeshPath path;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        line.positionCount = 0;
    }

    void Update()
    {
        if (player == null || destination == null) return;

        if (NavMesh.CalculatePath(
            player.position,
            destination.position,
            NavMesh.AllAreas,
            path))
        {
            DrawGroundedPath(path);
        }
    }

    void DrawGroundedPath(NavMeshPath path)
    {
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 corner = path.corners[i];
            Vector3 groundedPos = GetGroundPosition(corner);
            line.SetPosition(i, groundedPos);
        }
    }

    Vector3 GetGroundPosition(Vector3 source)
    {
        Ray ray = new Ray(source + Vector3.up * 5f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20f, groundLayer))
        {
            return hit.point + Vector3.up * lineHeight;
        }

        // 地面が見つからなかった場合の保険
        return source + Vector3.up * lineHeight;
    }
}
