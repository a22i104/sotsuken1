using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{
    public Transform player;           // プレイヤー
    public Transform destination;      // 目的地
    public LineRenderer line;          // 経路表示用ライン
    public float stopDistance = 1.5f;  // 到達判定距離

    private NavMeshPath path;          // 経路情報
    private bool isActive = true;      // 案内機能のON/OFF状態

    void Start()
    {
        path = new NavMeshPath();
        line.positionCount = 0;  // 初期は非表示
    }

    void Update()
    {
        if (!isActive) return;

        // 経路を更新
        if (NavMesh.CalculatePath(player.position, destination.position, NavMesh.AllAreas, path))
        {
            DrawPath(path);
        }

        // 到達判定
        float distance = Vector3.Distance(player.position, destination.position);
        if (distance < stopDistance)
        {
            DisableNavigation();
        }
    }

    // 経路をラインで表示
    void DrawPath(NavMeshPath path)
    {
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 pos = path.corners[i];
            pos.y += 0.1f; // 少し浮かせて地面に埋もれないように
            line.SetPosition(i, pos);
        }
    }

    // 案内停止
    void DisableNavigation()
    {
        isActive = false;
        line.positionCount = 0;  // ラインを消す
        Debug.Log("目的地に到達しました。ナビゲーションを終了します。");
    }
}
