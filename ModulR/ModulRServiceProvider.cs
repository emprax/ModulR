using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    internal class ModulRServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<int, ServiceDescriptor> serviceDescriptors;

        public ModulRServiceProvider(IServiceProvider serviceProvider, ConcurrentDictionary<int, ServiceDescriptor> serviceDescriptors)
        {
            this.serviceProvider = serviceProvider;
            this.serviceDescriptors = serviceDescriptors;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType is null)
            {
                return null;
            }

            if (this.serviceDescriptors.TryGetValue(serviceType.GetHashCode(), out var value))
            {
                if (value?.ImplementationInstance != null)
                {
                    return value.ImplementationInstance;
                }

                if (value?.ImplementationFactory != null)
                {
                    return value.ImplementationFactory.Invoke(this);
                }

                var constructor = value?.ImplementationType?
                    .GetConstructors()?
                    .FirstOrDefault();

                var parameters = constructor?
                    .GetParameters()
                    .Select(p => this.GetService(p?.ParameterType))?
                    .ToArray();

                return constructor?.Invoke(parameters);
            }

            return this.serviceProvider.GetService(serviceType);
        }
    }
}
