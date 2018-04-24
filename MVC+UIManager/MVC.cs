using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MVC
{
    //存储MVC
    private static List<Model> Models = new List<Model>(); //模型
    private static Dictionary<string, View> Views = new Dictionary<string, View>();//名字---视图
    private static List<Controller> Controllers = new List<Controller>();//名字---控制器类型

    //注册
    public static void RegisterModel(Model model)
    {
        if(!Models.Contains(model))
        {
            Models.Add(model);
        }
    }

    public static void RegisterView(View view)
    {
        //防止重复注册
        if (!Views.ContainsKey(view.Name))
        {
            //缓存
            Views[view.Name] = view;
        }
    }

    public static void RegisterController(Controller controller)
    {
        if(!Controllers.Contains(controller))
        {
            Controllers.Add(controller);
        }
    }

    //获取
    public static T GetModel<T>()
        where T : Model
    {
        foreach(Model m in Models)
        {
            if (m is T)
                return m as T;
        }
        return null;
    }

    public static T GetView<T>()
        where T : View
    {
        foreach (View v in Views.Values)
        {
            if (v is T)
                return v as T;
        }
        return null;
    }
    
    public static Dictionary<string,View> GetViews()
    {
        return Views;
    }
}
