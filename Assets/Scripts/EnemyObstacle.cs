using UnityEngine;
using Tanks.Complete;

public class EnemyObstacle : MonoBehaviour
{
    public float m_Speed = 6f;
    public float m_TurnSpeed = 5f;
    public float m_ContactDamage = 8f;
    public float m_DamageCooldown = 1f;
    public float m_StopDistance = 1.5f;

    private Rigidbody m_Rigidbody;
    private float m_LastDamageTime;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Transform target = FindNearestTank();
        if (target == null) return;

        Vector3 dir = target.position - m_Rigidbody.position;
        dir.y = 0f;
        float dist = dir.magnitude;
        if (dist <= m_StopDistance) return;

        Vector3 step = dir.normalized * m_Speed * Time.fixedDeltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + step);

        Quaternion look = Quaternion.LookRotation(dir.normalized);
        m_Rigidbody.MoveRotation(Quaternion.Slerp(m_Rigidbody.rotation, look, m_TurnSpeed * Time.fixedDeltaTime));
    }

    private Transform FindNearestTank()
    {
        var tanks = Object.FindObjectsByType<TankMovement>(FindObjectsSortMode.None);
        Transform nearest = null;
        float best = float.MaxValue;
        foreach (var t in tanks)
        {
            if (!t.gameObject.activeInHierarchy) continue;
            float d = (t.transform.position - transform.position).sqrMagnitude;
            if (d < best) { best = d; nearest = t.transform; }
        }
        return nearest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryDamage(collision.collider);
    }

    private void OnCollisionStay(Collision collision)
    {
        TryDamage(collision.collider);
    }

    private void TryDamage(Collider other)
    {
        if (Time.time - m_LastDamageTime < m_DamageCooldown) return;
        var health = other.GetComponentInParent<TankHealth>();
        if (health != null)
        {
            health.TakeDamage(m_ContactDamage);
            m_LastDamageTime = Time.time;
        }
    }
}
