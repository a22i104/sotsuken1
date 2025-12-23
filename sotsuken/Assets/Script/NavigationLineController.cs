using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class NavigationLineController : MonoBehaviour
{
    public Transform player;
    public List<Transform> destinations;
    public LayerMask groundLayer;
    public float lineHeight = 0.15f;
    public float arriveDistance = 1.5f;

    private LineRenderer line;
    private NavMeshPath path;

    private int currentIndex = 0;
    private bool navigationEnabled = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        line.positionCount = 0;
    }

    void Update()
    {
        HandleToggle();
        HandleDestinationSelect();

        if (!navigationEnabled)
        {
            line.positionCount = 0;
            return;
        }

        UpdateNavigationLine();
        CheckArrive();
    }

    // ナビ ON / OFF
    void HandleToggle()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            navigationEnabled = !navigationEnabled;
            Debug.Log("ナビ状態: " + (navigationEnabled ? "ON" : "OFF"));

            if (!navigationEnabled)
                line.positionCount = 0;
        }
    }

    // 目的地切り替え
    void HandleDestinationSelect()
    {
        if (!navigationEnabled || destinations.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % destinations.Count;
            Debug.Log("目的地: " + destinations[currentIndex].name);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = destinations.Count - 1;
            Debug.Log("目的地: " + destinations[currentIndex].name);
        }
    }

    void UpdateNavigationLine()
    {
        if (player == null || destinations.Count == 0) return;

        if (NavMesh.CalculatePath(
            player.position,
            destinations[currentIndex].position,
            NavMesh.AllAreas,
            path))
        {
            DrawGroundedPath(path);
        }
    }

    // ★ 到達判定
    void CheckArrive()
    {
        float distance = Vector3.Distance(
            player.position,
            destinations[currentIndex].position);

        if (distance <= arriveDistance)
        {
            ForceNavigationOff();
        }
    }

    void ForceNavigationOff()
    {
        navigationEnabled = false;
        line.positionCount = 0;
        Debug.Log("目的地に到達 → ナビOFF");
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
            Vector3 pos = GetGroundPosition(path.corners[i]);
            line.SetPosition(i, pos);
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

        return source + Vector3.up * lineHeight;
    }
}
