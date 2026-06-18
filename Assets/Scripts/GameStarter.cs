using UnityEngine;
using Tanks.Complete;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        var gm = GetComponent<GameManager>();
        if (gm == null) return;

        var p1 = new GameManager.PlayerData { ControlIndex = 1, IsComputer = false, TankColor = new Color(0.165f, 0.392f, 0.698f), UsedPrefab = gm.m_Tank1Prefab };
        var p2 = new GameManager.PlayerData { ControlIndex = 2, IsComputer = false, TankColor = new Color(0.898f, 0.180f, 0.157f), UsedPrefab = gm.m_Tank2Prefab };

        gm.StartGame(new GameManager.PlayerData[] { p1, p2 });
    }
}
