using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlayerSelection_PluralPlayerObject : GUIComponent_Panel
{
    [Header("All buttons are gathered automatically")]
    public List<Button> buttons = new List<Button>();

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        buttons.AddRange(GetComponentsInChildren<Button>());
        ClearData();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void SetData(List<PlayerObject> poList)
    {
        ResourceManager resourcesManager = screenManager.gameManager.ResourceManager();

        for (int i = 0; i < buttons.Count; i++)
        {
            if (!Equals(poList, null) && i < poList.Count)
            {
                buttons[i].image.sprite = poList[i].img_icon;
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].image.sprite = resourcesManager.icon_NoIcon;
                buttons[i].interactable = false;
            }
        }
    }

    public void ClearData()
    {
        ResourceManager resourcesManager = screenManager.gameManager.ResourceManager();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].image.sprite = resourcesManager.icon_NoIcon;
            buttons[i].interactable = false;
        }
    }
}
