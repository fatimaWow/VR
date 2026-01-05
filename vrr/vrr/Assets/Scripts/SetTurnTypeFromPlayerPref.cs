using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnTypeFromPlayerPref : MonoBehaviour
{
    [Header("Turn Providers")]
    public SnapTurnProviderBase snapTurn;
    public ContinuousTurnProviderBase continuousTurn;

    void Start()
    {
        ApplyPlayerPref();
    }

    public void ApplyPlayerPref()
    {
        if (!PlayerPrefs.HasKey("turn"))
            return;

        int value = PlayerPrefs.GetInt("turn");

        // 0 = Snap Turn, 1 = Continuous Turn
        snapTurn.enabled = (value == 0);
        continuousTurn.enabled = (value == 1);
    }
}
