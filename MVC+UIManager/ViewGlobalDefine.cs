using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewType
{
    Normal,
    Fixed,
    PopUp
}

public enum ViewShowMode
{
    DoNothing,
    HideOther,//只是隐藏和自己相同类型的视图
    HideAll//隐藏所有类型的视图
}

//只针对PopUp类型的视图
public enum ViewTransparencyMode
{
    Lucency,//完全透明
    Translucence,//半透明
    ImPrenetrable,//低透明
    Pentrate//可以穿透
}


//UI 的名字 路径
public class ViewConst  {
    public static readonly string View_MainMenu = "MainMenuView";
    public static readonly string View_Game = "GameView";
    public static readonly string View_Top = "TopView";
    public static readonly string View_Message = "MessageView";
    public static readonly string View_SureBox = "SureBoxView";

    public static readonly string UIRootPath = "Prefab/ViewPrefab/";
}

public class ModelConst
{
    public static readonly string Model_Energy = "Energy";
}