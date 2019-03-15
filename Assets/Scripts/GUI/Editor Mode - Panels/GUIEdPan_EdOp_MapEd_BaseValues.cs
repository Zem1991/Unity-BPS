using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEdPan_EdOp_MapEd_BaseValues : GUIComponent_Panel
{
    public InputField inp_MapSizeX;
    public InputField inp_MapSizeZ;
    public InputField inp_ChunkSizeX;
    public InputField inp_ChunkSizeZ;
    public InputField inp_InitialElevation;
    public InputField inp_TileSize;
    public Button btn_Apply;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void ReadParameters()
    {
        if (!screenManager)
            screenManager = GetComponentInParent<ScreenManager>();

        MapManager mm = screenManager.gameManager.MapManager();
        inp_MapSizeX.text = mm.mapSizeX.ToString();
        inp_MapSizeZ.text = mm.mapSizeZ.ToString();
        inp_ChunkSizeX.text = mm.chunkSizeX.ToString();
        inp_ChunkSizeZ.text = mm.chunkSizeZ.ToString();
        inp_InitialElevation.text = mm.initialElevation.ToString();
        inp_TileSize.text = mm.tileSize.ToString();
    }

    public void WriteParameters()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.mapSizeX = int.Parse(inp_MapSizeX.text);
        mm.mapSizeZ = int.Parse(inp_MapSizeZ.text);
        mm.chunkSizeX = int.Parse(inp_ChunkSizeX.text);
        mm.chunkSizeZ = int.Parse(inp_ChunkSizeZ.text);
        mm.initialElevation = int.Parse(inp_InitialElevation.text);
        mm.tileSize = int.Parse(inp_TileSize.text);

        mm.CheckParameters();
        ReadParameters();
    }

    public void BTN_Apply()
    {
        MapManager mm = screenManager.gameManager.MapManager();
        mm.Step1_ApplyBaseValues();
    }
}
