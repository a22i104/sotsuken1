using UnityEngine;
using UnityEngine.AI;

public class ElevatorTeleport : MonoBehaviour
{
    private NavMeshAgent agent;
    private CharacterController controller;
    private bool isTeleporting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("NavMeshが近くにありません！");
        }
    }


    void LateUpdate()
    {
        if (agent.isOnNavMesh)
        {
            agent.nextPosition = transform.position;
        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }


    void Update()
    {
        Debug.Log("isOnOffMeshLink: " + agent.isOnOffMeshLink);

        if (agent.isOnOffMeshLink && !isTeleporting)
        {
            Teleport();
        }
    }

    void Teleport()
    {
        isTeleporting = true;

        OffMeshLinkData data = agent.currentOffMeshLinkData;

        // CharacterControllerを一時停止
        controller.enabled = false;

        // プレイヤー本体を出口へ瞬間移動
        transform.position = data.endPos;

        // Agentも同期
        agent.Warp(data.endPos);
        agent.CompleteOffMeshLink();

        controller.enabled = true;
        isTeleporting = false;
    }
}
