using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Unity;

public class UnityDependencyResolver : IDependencyResolver
{
    private readonly IUnityContainer _container;

    public UnityDependencyResolver(IUnityContainer container)
    {
        _container = container;
    }

    public object GetService(Type serviceType)
    
    {
        try
        {
            return _container.Resolve(serviceType);
        }
        catch (ResolutionFailedException)
        {
            return null;
        }
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
        try
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }
        catch (ResolutionFailedException)
        {
            return new List<object>();
        }
    }
}
