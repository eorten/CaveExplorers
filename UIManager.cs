using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] private InfoPanel infoPanelPrefab;
    [SerializeField] private Transform[] infoPanelPositions;
    [SerializeField] private Canvas canvas;
    private List<InfoPanel> infoPanels = new List<InfoPanel>();
    private int infoPanelCount = 0;
    public void MakeInfoPanel(Player player)
    {
        InfoPanel ip = Instantiate(infoPanelPrefab);
        ip.transform.SetParent(infoPanelPositions[infoPanelCount], false);
        ip.Initialize("Player " + (infoPanelCount + 1), player);
        infoPanels.Add(ip);
        infoPanelCount++;
    }
    public void UpdateAllInfoPanels()
    {
        foreach (var item in infoPanels)
        {
            item.UpdateInfo();
        }
    }
}
