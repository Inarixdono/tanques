using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject m_EnemyPrefab;
    public int m_Count = 3;
    public float m_SpawnRadius = 22f;
    public Vector3 m_Center = Vector3.zero;

    private void Start()
    {
        if (m_EnemyPrefab == null) return;

        for (int i = 0; i < m_Count; i++)
        {
            Vector2 c = Random.insideUnitCircle * m_SpawnRadius;
            Vector3 pos = m_Center + new Vector3(c.x, 0.5f, c.y);
            Instantiate(m_EnemyPrefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        }
    }
}
