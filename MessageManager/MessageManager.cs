using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack();
public delegate void CallBack<T>(T arg0);
public delegate void CallBack<T,U>(T arg0,U arg1);
public delegate void CallBack<T,U,V>(T arg0,U arg1,V arg2);
public class MessageManager :Singleton<MessageManager> {
    private Dictionary<string, Delegate> messageDic = new Dictionary<string, Delegate>();

    #region 添加监听者

    //1.判断事件字典中有该事件
    //2.没有 创建该消息的委托
    //3.将该方法添加上去

    //监听者被加上
    private bool OnListenerAdding(string messageName, Delegate listener)
    {
        if (!messageDic.ContainsKey(messageName))
        {
            messageDic.Add(messageName, null);
        }
        Delegate d = messageDic[messageName];
        if(d!=null&&d.GetType()!=listener.GetType())
        {
            Debug.LogError("监听方法的类型与委托类型不一样，事件："+messageName);
            return false;
        }
        return true;
    }

    public void AddListener(string messageName,CallBack handler)
    {
        if(OnListenerAdding(messageName, handler))
        {
            messageDic[messageName] = (CallBack)messageDic[messageName] + handler;
        }
    }

    public void AddListener<T>(string messageName, CallBack<T> handler)
    {
        if (OnListenerAdding(messageName, handler))
        {
            messageDic[messageName] = (CallBack<T>)messageDic[messageName] + handler;
        }
    }

    public void AddListener<T,U>(string messageName, CallBack<T,U> handler)
    {
        if (OnListenerAdding(messageName, handler))
        {
            messageDic[messageName] = (CallBack<T, U>)messageDic[messageName] + handler;
        }
    }

    public void AddListener<T,U,V>(string messageName, CallBack<T,U,V> handler)
    {
        if (OnListenerAdding(messageName, handler))
        {
            messageDic[messageName] = (CallBack<T, U, V>)messageDic[messageName] + handler;
        }
    }

    #endregion

    #region 移除监听者
    //1.判断事件字典中有该事件
    //2.有 判断要去除的方法的类型和委托的类型是否一样 没有 报错
    //3.去除方法

    //正在移除监听者
    private bool OnListenerRemoving(string messageName, Delegate listener)
    {
        if (!messageDic.ContainsKey(messageName))
        {
            Debug.LogError("没有该消息："+messageName);
            return false;
        }
        else
        {
            if(messageDic[messageName]==null)
            {
                Debug.LogError("该消息未初始化："+messageName);
                return false;
            }
            else 
            {
                if (messageDic[messageName].GetType() == listener.GetType())
                {
                    return true;
                }else
                {
                    Debug.LogError("监听方法的类型与委托类型不一样，事件：" + messageName);
                    return false;
                }
            }
        }
    }

    //移除监听者之后
    private void OnListenerRemoved(string messageName)
    {
        if(messageDic[messageName]==null)
        {
            messageDic.Remove(messageName);
        }
    }

    //移除监听者
    public void RemoveListener(string messageName,CallBack handler)
    {
        if(OnListenerRemoving(messageName,handler))
        {
            messageDic[messageName] = (CallBack)messageDic[messageName] - handler;
            OnListenerRemoved(messageName);
        }
    }

    //移除监听者
    public void RemoveListener<T>(string messageName, CallBack<T> handler)
    {
        if (OnListenerRemoving(messageName, handler))
        {
            messageDic[messageName] = (CallBack<T>)messageDic[messageName] - handler;
            OnListenerRemoved(messageName);
        }
    }

    //移除监听者
    public void RemoveListener<T,U>(string messageName, CallBack<T,U> handler)
    {
        if (OnListenerRemoving(messageName, handler))
        {
            messageDic[messageName] = (CallBack<T,U>)messageDic[messageName] - handler;
            OnListenerRemoved(messageName);
        }
    }

    //移除监听者
    public void RemoveListener<T,U,V>(string messageName, CallBack<T, U, V> handler)
    {
        if (OnListenerRemoving(messageName, handler))
        {
            messageDic[messageName] = (CallBack<T, U, V>)messageDic[messageName] - handler;
            OnListenerRemoved(messageName);
        }
    }

    //清除消息时执行
    public void OnListenersClearing(string messageName)
    {
        if(!messageDic.ContainsKey(messageName))
        {
            Debug.LogWarning("没有该消息 ："+messageName);
        }
    }

    //清除一个消息
    public void ClearListeners(string messageName)
    {
        OnListenersClearing(messageName);
        messageDic.Remove(messageName);
    }

    //清除所有消息
    public void ClearAllMessage()
    {
        messageDic.Clear();
    }

    #endregion

    #region 广播消息
    //1.判断该消息是否存在
    //2.存在 判断该消息是否有方法监听
    //3.触发委托

    //正在消息广播
    private bool OnMessageBroading(string messageName)
    {
        if(!messageDic.ContainsKey(messageName))
        {
            Debug.LogError("不存在该消息 ："+messageName);
            return false;
        }
        else
        {
            if(messageDic[messageName]==null)
            {
                Debug.LogError("该消息未初始化 ：" + messageName);
                return false;
            }
        }
        return true;
    }
    
    //广播消息
    public void BroadMessage(string messageName)
    {
        if(OnMessageBroading(messageName))
        {
            CallBack callback = messageDic[messageName] as CallBack;
            if(callback!=null)
            {
                callback();
            }
        }
    }

    //广播消息
    public void BroadMessage<T>(string messageName,T arg0)
    {
        if (OnMessageBroading(messageName))
        {
            CallBack<T> callback = messageDic[messageName] as CallBack<T>;
            if (callback != null)
            {
                callback(arg0);
            }
        }
    }

    //广播消息
    public void BroadMessage<T,U>(string messageName ,T arg0,U arg1)
    {
        if (OnMessageBroading(messageName))
        {
            CallBack<T, U> callback = messageDic[messageName] as CallBack<T, U>;
            if (callback != null)
            {
                callback(arg0,arg1);
            }
        }
    }

    //广播消息
    public void BroadMessage<T,U,V>(string messageName,T arg0, U arg1, V arg2)
    {
        if (OnMessageBroading(messageName))
        {
            CallBack<T, U, V> callback = messageDic[messageName] as CallBack<T, U, V>;
            if (callback != null)
            {
                callback(arg0,arg1,arg2);
            }
        }
    }

    #endregion
}
