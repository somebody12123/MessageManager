using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    #region 单例
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    #region 结点
    private RectTransform uiRoot;//UI根结点 UIManager挂在这里
    private RectTransform uiNormalRoot;//Normal视图物体挂在这里
    private RectTransform uiFixedRoot;//Fixed视图物体挂在这里
    private RectTransform uiPopUpRoot;//PopUp视图物体挂在这里

    private RectTransform popupMesk;//PopUp遮罩
    #endregion

    #region 数据
    private Dictionary<string, View> views = MVC.GetViews();//保存所有视图
    private Dictionary<string, View> showViews = new Dictionary<string, View>();//保存显示的视图

    private Stack<View> normalBackStack = new Stack<View>();//专门为Normal视图用
    private Stack<View> popupBackStack = new Stack<View>();//专门为PopUp视图用

    private View currentView = null;//保存当前视图
    #endregion

    #region Unity回调方法
    private void Awake()
    {
        instance = this;

        uiRoot = GameObject.Find("UIRoot").GetComponent<RectTransform>();
        SetSubRoot();
        
        DontDestroyOnLoad(uiRoot.gameObject);
    }
    #endregion

    #region 设置结点

    /// <summary>
    /// 设置子根节点
    /// </summary>
    private void SetSubRoot()
    {
        //设置普通窗口根结点
        if (uiNormalRoot == null)
        {
            GameObject temp = new GameObject("UINormalRoot");
            temp.layer = LayerMask.NameToLayer("UI");
            uiNormalRoot = temp.AddComponent<RectTransform>();
        }
        uiNormalRoot.SetParent(uiRoot);
        RectTransformInit(uiNormalRoot);
        //设置固定窗口根结点
        if (uiFixedRoot == null)
        {
            GameObject temp = new GameObject("UIFixedRoot");
            temp.layer = LayerMask.NameToLayer("UI");
            uiFixedRoot = temp.AddComponent<RectTransform>();
        }
        uiFixedRoot.SetParent(uiRoot);
        RectTransformInit(uiFixedRoot);
        //设置弹出窗口根结点
        if (uiPopUpRoot == null)
        {
            GameObject temp = new GameObject("UIPopUpRoot");
            temp.layer = LayerMask.NameToLayer("UI");
            uiPopUpRoot = temp.AddComponent<RectTransform>();
        }
        uiPopUpRoot.SetParent(uiRoot);
        RectTransformInit(uiPopUpRoot);
        //设置弹出窗口的遮罩
        popupMesk= CreatePopUpMesk().GetComponent<RectTransform>();
        popupMesk.SetParent(uiPopUpRoot);
        RectTransformInit(popupMesk);

    }

    /// <summary>
    /// 设置RectTransform的值
    /// 锚点设置为(stretch,stretch)
    /// Scale设置为（1,1,1）
    /// </summary>
    /// <param name="rect"></param>
    private void RectTransformInit(RectTransform rect)
    {
        rect.localScale = Vector3.one;
        rect.pivot = Vector2.one / 2;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }

    /// <summary>
    /// 创建弹出窗口遮罩
    /// </summary>
    /// <returns></returns>
    private GameObject CreatePopUpMesk()
    {
        GameObject meskGO = new GameObject("PopUpMesk");
        meskGO.layer= LayerMask.NameToLayer("UI");
        meskGO.AddComponent<RectTransform>();
        meskGO.AddComponent<Image>();
        meskGO.SetActive(false);
        return meskGO;
    }

    #endregion
    
    #region 初始化
    //初始化视图
    public void InitView(string viewName)
    {
        GameObject viewPrefab = Resources.Load<GameObject>(ViewConst.UIRootPath + viewName);
        
        GameObject viewGO= GameObject.Instantiate(viewPrefab);
        View view = viewGO.GetComponent<View>();
        //将视图挂在对应的根结点下
        switch (view.Type)
        {
            case ViewType.Normal:
                viewGO.GetComponent<RectTransform>().SetParent(uiNormalRoot);
                break;
            case ViewType.Fixed:
                viewGO.GetComponent<RectTransform>().SetParent(uiFixedRoot);
                break;
            case ViewType.PopUp:
                viewGO.GetComponent<RectTransform>().SetParent(uiPopUpRoot);
                break;
            default:
                Debug.LogError("非法视图类型");
                break;
        }
        RectTransformInit(viewGO.GetComponent<RectTransform>());

        view.Init();
        views.Add(view.Name, view);
    }
    #endregion

    #region 显示

    //显示视图
    public void ShowView(string viewName)
    {
        //视图不存在
        if(!views.ContainsKey(viewName))
        {
            //加载并初始化
            InitView(viewName);
        }
        View view = views[viewName];
        
        if (view.Type==ViewType.Normal)
        {
            //隐藏所有弹出视图
            HideAllPopUpView();
            //清空弹出视图返回栈
            ClearPopUpBackStack();
            //TODO
            while (normalBackStack.Contains(view))
            {
                HideView(normalBackStack.Pop().Name);
            }
            normalBackStack.Push(view);
        }
        else if(view.Type==ViewType.Fixed)
        {
            //TODO
        }else if(view.Type==ViewType.PopUp)
        {
            while(popupBackStack.Contains(view))
            {
                HideView(popupBackStack.Pop().Name);
            }
            popupBackStack.Push(view);

            Image image = popupMesk.GetComponent<Image>();
            switch (view.TransparencyMode)
            {
                case ViewTransparencyMode.Lucency://完全透明
                    image.color = new Color(100f, 100f, 100f,0f);
                    popupMesk.gameObject.SetActive(true);
                    break;
                case ViewTransparencyMode.Translucence://半透明
                    image.color = new Color(100f, 100f, 100f, 255*0.5f);
                    popupMesk.gameObject.SetActive(true);
                    break;
                case ViewTransparencyMode.ImPrenetrable://低透明
                    image.color = new Color(100f, 100f, 100f, 255*0.75f);
                    popupMesk.gameObject.SetActive(true);
                    break;
                case ViewTransparencyMode.Pentrate://可以穿透
                    popupMesk.gameObject.SetActive(false);
                    break;
            }
        }

        if (view.ShowMode == ViewShowMode.HideOther)
        {
            List<string> hiding = new List<string>();
            foreach (KeyValuePair<string, View> pair in showViews)
            {
                if (pair.Value.Type == view.Type)
                {
                    hiding.Add(pair.Key);
                }
            }
            foreach (string name in hiding)
            {
                HideView(name);
            }
        }
        else if (view.ShowMode == ViewShowMode.HideAll)
        {
            List<string> hiding = new List<string>();
            foreach (KeyValuePair<string, View> pair in showViews)
            {
               hiding.Add(pair.Key);
            }
            foreach (string name in hiding)
            {
                HideView(name);
            }
        }

        view.ShowWithAnimation();//显示视图
        currentView = view;
        showViews.Add(viewName, view);
    }


    #endregion

    #region 返回
    //返回视图
    public void ReturnView()
    {
        View backView = null;
        string backViewName = null;
        if (currentView != null)
        {
            //所有弹出窗口隐藏
            //清空弹出窗口栈
            //处理返回视图逻辑
            if (currentView.Type == ViewType.Normal)//普通视图
            {
                //所有弹出窗口隐藏
                HideAllPopUpView();
                //清空弹出窗口栈
                ClearPopUpBackStack();
                //处理返回视图逻辑
                if (normalBackStack.Count >= 2)
                {
                    if (normalBackStack.Pop() == currentView)
                    {
                        backView = normalBackStack.Pop();
                        HideView(currentView.Name);
                        ShowView(backView.Name);
                        return;
                    }
                }
                ClearNormalBackStack();
                backViewName = currentView.BackViewName;
                if (views.TryGetValue(backViewName, out backView))
                {
                    HideView(currentView.Name);
                    ShowView(backView.Name);
                }
                return;
            }
            
            else if (currentView.Type == ViewType.PopUp)//弹出视图
            {
                //处理返回视图逻辑
                if (popupBackStack.Count >= 2)
                {
                    if (popupBackStack.Pop() == currentView)
                    {
                        backView = popupBackStack.Pop();
                        HideView(currentView.Name);
                        ShowView(backView.Name);
                        return;
                    }
                }
                ClearPopUpBackStack();
                backViewName = currentView.BackViewName;
                if (views.TryGetValue(backViewName, out backView))
                {
                    HideView(currentView.Name);
                    ShowView(backView.Name);
                }
                return;
            }
        }
        else
        {
            Debug.LogError("currentView参数为null");
        }
    }
    #endregion

    #region 销毁
    //销毁所有视图
    public void DestroyAllView()
    {
        //销毁所有视图
        foreach (KeyValuePair<string, View> pair in views)
        {
            pair.Value.Destroy();
        }

        //字典 栈 清空
        ClearContainers();
    }
    #endregion

    #region 隐藏

    //隐藏视图 只用于其他函数内部
    private void HideView(string viewName)
    {
        if (showViews.ContainsKey(viewName))
        {
            View view = showViews[viewName];
            view.HideWithAnimation();
            showViews.Remove(viewName);

            //隐藏弹出视图的遮罩
            if (view.Type == ViewType.PopUp)
            {
                popupMesk.gameObject.SetActive(false);
            }
        }
        else
            Debug.LogError("显示视图字典中没有该视图："+viewName);
        
    }

    //private void HideView(View view)
    //{
    //    if (showViews.ContainsValue(view))
    //    {
    //        view.HideWithAnimation();
    //        showViews.Remove(view.Name);

    //        //隐藏弹出视图的遮罩
    //        if (view.Type == ViewType.PopUp)
    //        {
    //            popupMesk.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //        Debug.LogError("显示视图字典中没有该视图：" + view.Name);
    //}

    //隐藏所有普通视图
    private void HideAllNormalView()
    {
        List<string> hiding = new List<string>();
        foreach (KeyValuePair<string, View> pair in showViews)
        {
            if (pair.Value.Type == ViewType.Normal)
            {
                hiding.Add(pair.Key);
            }
        }
        foreach (string name in hiding)
        {
            HideView(name);
        }
    }
    //隐藏所有固定视图
    private void HideAllFixedView()
    {
        List<string> hiding = new List<string>();
        foreach (KeyValuePair<string, View> pair in showViews)
        {
            if (pair.Value.Type == ViewType.Fixed)
            {
                hiding.Add(pair.Key);
            }
        }
        foreach (string name in hiding)
        {
            HideView(name);
        }
    }
    //隐藏所有弹出视图
    private void HideAllPopUpView()
    {
        List<string> hiding = new List<string>();
        foreach (KeyValuePair<string, View> pair in showViews)
        {
            if (pair.Value.Type == ViewType.PopUp)
            {
                hiding.Add(pair.Key);
            }
        }
        foreach (string name in hiding)
        {
            HideView(name);
        }
    }

    #endregion

    #region 其他

    /// <summary>
    /// 清空字典和栈
    /// </summary>
    private void ClearContainers()
    {
        views.Clear();
        showViews.Clear();
        normalBackStack.Clear();
        popupBackStack.Clear();
    }

    //清空普通视图返回栈
    private void ClearNormalBackStack()
    {
        normalBackStack.Clear();
    }
    //清空弹出视图返回栈
    private void ClearPopUpBackStack()
    {
        popupBackStack.Clear();
    }
    #endregion
}
