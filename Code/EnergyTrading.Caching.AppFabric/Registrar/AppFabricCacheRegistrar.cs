﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EnergyTrading.Caching.AppFabric.Cache;
using EnergyTrading.Configuration;
using EnergyTrading.Container.Unity;
using Microsoft.Practices.Unity;

namespace EnergyTrading.Caching.AppFabric.Registrar
{
   public class AppFabricCacheRegistrar 
    {
       public static void Register(IUnityContainer container,IConfigurationManager configuration)
       {
           var appFabricCacheName = configuration.AppSettings["AppFabricCacheName"];
           Validate("AppFabricCacheName", appFabricCacheName);
           var appFabricUri = configuration.AppSettings["AppFabricUri"];

           if (!string.IsNullOrEmpty(appFabricUri))
           {
               container.RegisterType<ICacheRepository, AppFabricCacheRepository>(
                   new ContainerControlledLifetimeManager(),
                   new InjectionConstructor(appFabricCacheName,
                       appFabricUri.Split(',').Select(a => new Uri(a)).ToArray(), configuration));
           }
           else
           {
               container.RegisterType<ICacheRepository, AppFabricCacheRepository>(
                   new ContainerControlledLifetimeManager(),
                   new InjectionConstructor(appFabricCacheName, configuration));
           }
       }

       private static void Validate(string name, string value)
       {
           if (string.IsNullOrEmpty(value))
           {
               throw new ConfigurationErrorsException(string.Format("{0} appSetting key is missing\\empty.", name));
           }
       }
    }
}
