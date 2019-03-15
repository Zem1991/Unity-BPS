using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_Minimap : GUIComponent_Panel
{
    public Image img_minimapBox;
    public RawImage rawimg_minimap;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        GenerateCompleteMiniMap();
    }

    public bool GenerateCompleteMiniMap()
    {
        Texture2D mapImg = screenManager.gameManager.MapManager().GenerateMiniMap();

        if (mapImg == null)
            return false;

        //img_minimapBox.material.mainTexture = mapImg;
        rawimg_minimap.material.mainTexture = mapImg;

        //TODO: minimap using camera
        /*
         * Aparentemente é mais fácil fazer o minimapa com uma Camera do que definindo texturas em imagens.
         */

        return true;
    }
}
