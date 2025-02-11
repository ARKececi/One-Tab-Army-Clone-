using Controllers.EnemyController;
using UnityEngine;
using UnityEngine.AI;

public class BotFlock : MonoBehaviour
{
    public NavMeshAgent _agent;

    public float cohesionWeight = 1f;
    public float separationWeight = 1f;
    public float alignmentWeight = 1f;

    private Vector3 cohesion;
    private Vector3 separation;
    private Vector3 alignment;

    // Bu metot botların flocking değerlerini alıp işleme koyuyor
    public void SetFlockingValues(Vector3 cohesion, Vector3 separation, Vector3 alignment)
    {
        this.cohesion = cohesion;
        this.separation = separation;
        this.alignment = alignment;
    }

    // Flocking davranışını uygula
    public void Flocking()
    {
        // Gelen değerler ile botun hareketini hesapla
        Vector3 flockingMove = cohesion * cohesionWeight + separation * separationWeight + alignment * alignmentWeight;

        // Botun yeni hız ve yönünü ayarla
        _agent.velocity = flockingMove;
    }
}