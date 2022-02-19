using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloodManager : MonoBehaviour
{
    #region Singleton
    public static FloodManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] private TextMeshProUGUI waterLevelText;
    private List<int> floodMeterValues = new List<int>() {2,2,3,3,3,4,4,5,5};
    private int floodIndex = 0;

    public int GetWaterLevel() => floodMeterValues[floodIndex];
    public void IncreaseWaterLevel()
    {
        floodIndex++;
        waterLevelText.text = GetWaterLevel() + "";
        if (floodIndex >= floodMeterValues.Count)
        {
            GameManager.instance.ChangeGameState(GameState.lost);
        }
    }
}
