using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DG.Tweening;

public abstract class View : MonoBehaviour
{
    #region 视图基本信息
    //视图标识
    public abstract string Name { get; }

    //返回视图的标识
    public abstract string BackViewName { get; }

    //视图类型
    public abstract ViewType Type { get; }

    //视图显示类型
    public abstract ViewShowMode ShowMode { get; }

    //视图透明度类型（只给弹出窗口使用）
    public abstract ViewTransparencyMode TransparencyMode{get;}

    #endregion

    #region UI
    //显示
    protected void ShowSelf()
    {
        this.gameObject.SetActive(true);
    }

    //隐藏
    protected void HideSelf()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 本视图初始化
    /// </summary>
    public virtual void Init()
    {
        //注册操作该视图的控制器
    }

    /// <summary>
    /// 本视图显示
    /// </summary>
    public virtual void Show()
    {
        ShowSelf();
    }

    /// <summary>
    /// 本视图隐藏
    /// </summary>
    public virtual void Hide()
    {
        HideSelf();
    }

    /// <summary>
    /// 本视图销毁
    /// </summary>
    public virtual void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    #endregion

    #region 动画
    /// <summary>
    /// 带显示动画显示视图
    /// </summary>
    public virtual void ShowWithAnimation()
    {
        transform.DOMove(transform.position, 0f).onComplete = Show;
    }

    /// <summary>
    /// 带隐藏动画隐藏视图
    /// </summary>
    public virtual void HideWithAnimation()
    {
        transform.DOMove(transform.position, 0f).onComplete = Hide;
    }
    #endregion
}