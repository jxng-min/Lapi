using System;
using System.Collections.Generic;

public static class DIContainer
{
    private static Dictionary<Type, object> m_instances = new();

    public static void Register<T>(object instance)
    {
        m_instances[typeof(T)] = instance;
    }

    public static T Get<T>()
    {
        if (!IsRegistered<T>())
        {
            throw new Exception($"{typeof(T)}가 DI 컨테이너에 등록되어 있지 않습니다.");
        }
        return (T)m_instances[typeof(T)];
    }

    public static bool IsRegistered<T>()
    {
        return m_instances.ContainsKey(typeof(T));
    }

    public static void Clear()
    {
        m_instances.Clear();
    }
}