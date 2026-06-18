using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tanks.Complete;

public class MatchController : MonoBehaviour
{
    public float m_MatchTimeLimit = 240f;
    public int m_WinsToEnd = 5;
    public TMP_FontAsset m_Font;
    public TextMeshProUGUI m_TimerText;

    private GameManager m_GameManager;
    private float m_StartTime;
    private bool m_MatchOver;
    private GameObject m_GameOverPanel;
    private TextMeshProUGUI m_GameOverText;

    private void Start()
    {
        m_GameManager = GetComponent<GameManager>();
        m_StartTime = Time.time;
        BuildGameOverUI();
    }

    private void Update()
    {
        if (m_GameManager == null || m_MatchOver) return;

        float elapsed = Time.time - m_StartTime;
        float remaining = Mathf.Max(0f, m_MatchTimeLimit - elapsed);
        UpdateTimer(remaining);

        if (m_GameManager.m_SpawnPoints == null) return;

        foreach (var t in m_GameManager.m_SpawnPoints)
        {
            if (t != null && t.m_Wins >= m_WinsToEnd)
            {
                EndMatch(t, elapsed);
                return;
            }
        }

        if (remaining <= 0f)
            EndMatch(null, m_MatchTimeLimit);
    }

    private void UpdateTimer(float seconds)
    {
        if (m_TimerText == null) return;
        int m = (int)(seconds / 60f);
        int s = (int)(seconds % 60f);
        m_TimerText.text = m.ToString("0") + ":" + s.ToString("00");
        m_TimerText.color = seconds <= 30f ? Color.red : Color.white;
    }

    private void EndMatch(TankManager winner, float elapsed)
    {
        m_MatchOver = true;
        m_GameManager.StopAllCoroutines();

        foreach (var t in m_GameManager.m_SpawnPoints)
            if (t != null) t.DisableControl();

        int m = (int)(elapsed / 60f);
        int s = (int)(elapsed % 60f);
        string timeStr = m.ToString("0") + ":" + s.ToString("00");

        string msg;
        if (winner != null)
            msg = winner.m_ColoredPlayerText + " GANA EL JUEGO\n\nPuntaje: " + winner.m_Wins + " victorias\nTiempo: " + timeStr;
        else
            msg = "TIEMPO AGOTADO\n\nAMBOS JUGADORES PIERDEN\n\nTiempo: " + timeStr;

        m_GameOverText.text = msg;
        m_GameOverPanel.SetActive(true);
    }

    private void BuildGameOverUI()
    {
        var canvasGO = new GameObject("MatchUICanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        m_GameOverPanel = new GameObject("GameOverPanel");
        m_GameOverPanel.transform.SetParent(canvasGO.transform, false);
        var panelImg = m_GameOverPanel.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.85f);
        var prt = m_GameOverPanel.GetComponent<RectTransform>();
        prt.anchorMin = Vector2.zero;
        prt.anchorMax = Vector2.one;
        prt.offsetMin = Vector2.zero;
        prt.offsetMax = Vector2.zero;

        var goTextGO = new GameObject("GameOverText");
        goTextGO.transform.SetParent(m_GameOverPanel.transform, false);
        m_GameOverText = goTextGO.AddComponent<TextMeshProUGUI>();
        m_GameOverText.fontSize = 46;
        m_GameOverText.alignment = TextAlignmentOptions.Center;
        m_GameOverText.color = Color.white;
        if (m_Font != null) m_GameOverText.font = m_Font;
        var gort = goTextGO.GetComponent<RectTransform>();
        gort.anchorMin = new Vector2(0.1f, 0.15f);
        gort.anchorMax = new Vector2(0.9f, 0.85f);
        gort.offsetMin = Vector2.zero;
        gort.offsetMax = Vector2.zero;

        m_GameOverPanel.SetActive(false);
    }
}
