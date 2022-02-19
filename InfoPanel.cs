using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InfoPanel : MonoBehaviour
{

    [SerializeField] Color32 selectedColor, unselectedColor;

    [SerializeField] private GameObject removeButton;

    [SerializeField] private TextMeshProUGUI txtPlayerName, txtPlayerActions;

    [SerializeField] private TextMeshProUGUI heliNum, bagNum, chipNum, antennaNum, displayNum, pcNum;
    [SerializeField] private ButtonScript[] buttons;

    private GameObject activeButton;

    //EventS myEventSystem;
    EventSystem eventSystem;
    private string playerName;
    private Player player;
    private Inventory inventory;
    public void Initialize(string playerName, Player player)
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        removeButton.SetActive(false);
        this.playerName = playerName;
        this.player = player;
        inventory = player.GetInventory(); //Funker rekkefølge? 
        UpdateInfo();
        Player.onPlayerAction += Player_onPlayerAction;
        ButtonScript.onButtonPressed += ButtonScript_onButtonPressed;
    }

    private void ButtonScript_onButtonPressed(ButtonScript obj)
    {
        foreach (var item in buttons)
        {
            item.SetColor(unselectedColor);
        }

        removeButton.SetActive(activeButton != obj.gameObject);
        if (activeButton == obj.gameObject)
        {
            activeButton = null;
        }
        else
        {
            obj.SetColor(selectedColor);
            activeButton = obj.gameObject;
            
        }       
    }

    private void Player_onPlayerAction(Player obj)
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        txtPlayerName.text = "" + playerName;
        txtPlayerActions.text = "Actions: " + player.GetActions();

        int[] indexes = inventory.GetInventoryIndexes();
        heliNum.text = "" + indexes[0];
        bagNum.text = "" + indexes[1];
        chipNum.text = "" + indexes[2];
        antennaNum.text = "" + indexes[3];
        displayNum.text = "" + indexes[4];
        pcNum.text = "" + indexes[5];

    }
    private void OnDestroy()
    {
        ButtonScript.onButtonPressed -= ButtonScript_onButtonPressed;
        Player.onPlayerAction -= Player_onPlayerAction;
    }
    #region GetSet
    public Color32 GetunselectedColor()
    {
        return unselectedColor;
    }
    public void SetunselectedColor(Color32 value)
    {
        unselectedColor = value;
    }
    public Color32 GetselectedColor()
    {
        return selectedColor;
    }
    public void SetselectedColor(Color32 value)
    {
        selectedColor = value;
    }
    #endregion

}
