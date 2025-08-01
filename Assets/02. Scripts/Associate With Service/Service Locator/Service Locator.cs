using System;
using System.Collections.Generic;
using EquipmentService;
using EXPService;
using InventoryService;
using ItemDataService;
using SkillService;
using UserService;

public static class ServiceLocator
{
    private static Dictionary<Type, object> m_services = new();

    public static IDictionary<Type, object> Services => m_services;

    public static void Initialize()
    {
        Register<IEXPService>(new LocalEXPService());
        Register<IUserService>(new LocalUserService());
        Register<IInventoryService>(new LocalInventoryService());
        Register<IItemDataService>(new LocalItemDataService());
        Register<IEquipmentService>(new LocalEquipmentService());
        Register<ISkillService>(new LocalSkillService());
    }

    public static void Register<T>(T service)
    {
        if (!Services.ContainsKey(typeof(T)))
        {
            Services.Add(typeof(T), service);
        }
    }

    public static T Get<T>()
    {
        if (!Services.TryGetValue(typeof(T), out var service))
        {
            throw new Exception($"{typeof(T)} 서비스가 존재하지 않습니다.");
        }
        else
        {
            return (T)service;
        }
    }
}