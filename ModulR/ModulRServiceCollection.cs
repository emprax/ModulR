using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ModulR
{
    internal class ModulRServiceCollection : IServiceCollection
    {
        private readonly ConcurrentDictionary<int, ServiceDescriptor> services;

        internal ModulRServiceCollection(ConcurrentDictionary<int, ServiceDescriptor> services)
            => this.services = services ?? throw new ArgumentNullException(nameof(services));

        public ServiceDescriptor this[int index]
        { 
            get => services[index];
            set => services[index] = value;
        }

        public int Count => this.services.Count;

        public bool IsReadOnly => false;

        public void Add(ServiceDescriptor item) => this.services.AddOrUpdate(item.ServiceType.GetHashCode(), item, (i, s) => item);

        public void Clear() => this.services.Clear();

        public bool Contains(ServiceDescriptor item) => this.services.ContainsKey(item.ServiceType.GetHashCode());

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            if (arrayIndex >= this.Count || arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array is null || array?.Length == 0 || array.Length < arrayIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            var amount = Math.Min(this.Count, array.Length - arrayIndex);
            var results = this.services
                .ToList()
                .GetRange(0, amount)
                .Select(x => x.Value)
                .ToArray();

            for (var index = 0; index < amount; index++)
            {
                array[index + arrayIndex] = results[index];
            }
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator() => this.services
            .Select(x => x.Value)
            .ToList()
            .GetEnumerator();

        public int IndexOf(ServiceDescriptor item) => this.services
            .Select(x => x.Value)
            .ToList()
            .IndexOf(item);

        public void Insert(int index, ServiceDescriptor item) => this.services
            .ToList()
            .Insert(index, new KeyValuePair<int, ServiceDescriptor>(index, item));

        public bool Remove(ServiceDescriptor item) => this.services.TryRemove(item.ServiceType.GetHashCode(), out _);

        public void RemoveAt(int index) => this.services.TryRemove(index, out _);

        IEnumerator IEnumerable.GetEnumerator() => this.services
            .Select(x => x.Value)
            .ToList()
            .GetEnumerator();
    }
}
