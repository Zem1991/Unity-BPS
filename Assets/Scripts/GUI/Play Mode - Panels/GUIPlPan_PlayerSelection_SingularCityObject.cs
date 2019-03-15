using BPS.Population;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlPan_PlayerSelection_SingularCityObject : GUIComponent_Panel
{
    public Button btn_profile;
    public Text txt_name;
    public Text txt_votes, txt_income;
    public Text txt_religionLevel, txt_educationLevel, txt_politicalPosition, txt_soccerFanaticism;

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

    public void setData(CityObject co)
    {
        int income = 0, votes = 0;
        string religion = "", education = "", polPos = "";
        switch (co.economicalZoning)
        {
            case EconomicalZoning.POOR:
                income = 4;
                votes = 4;
                break;
            case EconomicalZoning.MEDIUM:
                income = 16;
                votes = 4;
                break;
            case EconomicalZoning.RICH:
                income = 64;
                votes = 4;
                break;
        }

        CO_Building res = co as CO_Building;
        if (res)
        {
            switch (res.religionLevel)
            {
                case ReligionLevel.No_Religion:
                    religion = "no religion";
                    break;
                case ReligionLevel.Low:
                    religion = "low religion";
                    break;
                case ReligionLevel.Medium:
                    religion = "medium religion";
                    break;
                case ReligionLevel.High:
                    religion = "high religion";
                    break;
            }
            switch (res.educationLevel)
            {
                case EducationLevel.No_Education:
                    education = "no education";
                    break;
                case EducationLevel.Low:
                    education = "low education";
                    break;
                case EducationLevel.Medium:
                    education = "medium education";
                    break;
                case EducationLevel.High:
                    education = "high education";
                    break;
            }
            switch (res.politicalPosition)
            {
                case PoliticalPosition.None:
                    polPos = "None";
                    break;
                case PoliticalPosition.AuthoriatianLeft:
                    polPos = "AuthoriatianLeft";
                    break;
                case PoliticalPosition.AuthoriatianCenter:
                    polPos = "AuthoriatianCenter";
                    break;
                case PoliticalPosition.AuthoriatianRight:
                    polPos = "AuthoriatianRight";
                    break;
                case PoliticalPosition.EconomicalLeft:
                    polPos = "EconomicalLeft";
                    break;
                case PoliticalPosition.EconomicalCenter:
                    polPos = "EconomicalCenter";
                    break;
                case PoliticalPosition.EconomicalRight:
                    polPos = "EconomicalRight";
                    break;
                case PoliticalPosition.LibertarianLeft:
                    polPos = "LibertarianLeft";
                    break;
                case PoliticalPosition.LibertarianCenter:
                    polPos = "LibertarianCenter";
                    break;
                case PoliticalPosition.LibertarianRight:
                    polPos = "LibertarianRight";
                    break;
            }
        }
        else
        {
            religion = "--";
            education = "--";
            polPos = "--";
        }

        btn_profile.image.sprite = co.img_profile;
        txt_name.text = co.levelObjectName;
        txt_income.text = "Income: " + income;
        txt_votes.text = "Votes: " + votes;
        txt_religionLevel.text = "Religion: " + religion;
        txt_educationLevel.text = "Education: " + education;
        txt_politicalPosition.text = "Political position: " + polPos;
        txt_soccerFanaticism.text = "Soccer Fanaticism: " + polPos;
    }

    public void clearData()
    {
        btn_profile.image.sprite = null;
        txt_name.text = null;
        txt_income.text = null;
        txt_votes.text = null;
        txt_religionLevel.text = null;
        txt_educationLevel.text = null;
        txt_politicalPosition.text = null;
        txt_soccerFanaticism.text = null;
    }
}
