using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NavigationLineController : MonoBehaviour
{
    [Header("Nav")]
    public Transform player;
    public LineRenderer line;
    public float arriveDistance = 1.5f;

    [Header("Destinations")]
    public Transform[] destinations;
    public string[] destinationNames;

    [Header("UI")]
    public TMP_Text statusText;

    private NavMeshPath path;
    private int currentIndex = 0;
    private bool navigationActive = false;
    private Transform currentDestination;

    void Start()
    {
        path = new NavMeshPath();
        currentDestination = destinations[currentIndex];
        line.positionCount = 0;
        UpdateStatusText();
    }

    void Update()
    {
        HandleInput();

        if (!navigationActive)
            return;

        UpdatePath();

        float distance = Vector3.Distance(player.position, currentDestination.position);
        if (distance < arriveDistance)
        {
            StopNavigation();
        }
    }

    // =========================
    // 入力処理
    void HandleInput()
    {
        // ナビ ON / OFF
        if (Input.GetKeyDown(KeyCode.N))
        {
            navigationActive = !navigationActive;

            if (!navigationActive)
            {
                line.positionCount = 0;
            }

            UpdateStatusText();
        }

        // 目的地切替（ナビOFF中のみ）
        if (!navigationActive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex = (currentIndex + 1) % destinations.Length;
                currentDestination = destinations[currentIndex];
                UpdateStatusText();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex--;
                if (currentIndex < 0)
                    currentIndex = destinations.Length - 1;

                currentDestination = destinations[currentIndex];
                UpdateStatusText();
            }
        }
    }

    // =========================
    // 経路更新 & 表示
    void UpdatePath()
    {
        if (!NavMesh.CalculatePath(player.position, currentDestination.position, NavMesh.AllAreas, path))
            return;

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            line.positionCount = 0;
            return;
        }

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 pos = path.corners[i];

            // 坂でも埋もれないように Raycast
            if (Physics.Raycast(pos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
            {
                pos.y = hit.point.y + 0.15f;
            }

            line.SetPosition(i, pos);
        }
    }

    // =========================
    // ナビ停止
    void StopNavigation()
    {
        navigationActive = false;
        line.positionCount = 0;
        UpdateStatusText();
    }

    // =========================
    // UI更新
    void UpdateStatusText()
    {
        if (navigationActive)
        {
            statusText.text = $"Navi ON → {destinationNames[currentIndex]}";
        }
        else
        {
            statusText.text = $"Select：{destinationNames[currentIndex]}（Push to N）";
        }
    }
}
