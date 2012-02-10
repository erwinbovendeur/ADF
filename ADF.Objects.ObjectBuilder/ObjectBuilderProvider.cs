using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Adf.Core.Logging;
using Adf.Core.Objects;
using Adf.Objects.ObjectBuilder.BuilderPolicies;
using Adf.Objects.ObjectBuilder.Configuration;
using Adf.ObjectBuilder.Properties;
using Microsoft.Practices.ObjectBuilder;

namespace Adf.Objects.ObjectBuilder
{
    public class ObjectBuilderProvider : IObjectProvider
    {
        private readonly List<DependencyResolutionLocatorKey> threadStaticTypes =
            new List<DependencyResolutionLocatorKey>();

        private readonly SystemFactoryConfigurationSection systemFactoryConfiguration;
        private readonly IBuilder<BuilderStage> builder;

        public ObjectBuilderProvider()
        {
            builder = new BuilderBase<BuilderStage>();
            builder.Strategies.AddNew<TypeMappingStrategy>(BuilderStage.PreCreation);
            builder.Strategies.AddNew<SingletonStrategy>(BuilderStage.PreCreation);
            builder.Strategies.AddNew<CreationStrategy>(BuilderStage.Creation);
            builder.Policies.SetDefault<ICreationPolicy>(new CreationPolicy());

            systemFactoryConfiguration =
                ConfigurationManager.GetSection(SystemFactoryConfigurationSection.SystemFactoryConfigurationSectionName)
                as SystemFactoryConfigurationSection ??
                new SystemFactoryConfigurationSection();

            //PolicyList policyList = new PolicyList();
            foreach (SystemFactoryConfigurationElement serviceElement in systemFactoryConfiguration.Services)
            {
                if (serviceElement.LifeCycle != LifeCycle.InstancePerBuildUp)
                {
                    if (serviceElement.LifeCycle == LifeCycle.InstancePerThread)
                    {
                        threadStaticTypes.Add(new DependencyResolutionLocatorKey(serviceElement.ServiceRealizationType,
                                                                                 serviceElement.InstanceName));
                    }
                    builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true),
                                                           serviceElement.ServiceRealizationType,
                                                           serviceElement.InstanceName);
                }
                builder.Policies.Set<ITypeMappingPolicy>(
                    new TypeMappingPolicy(serviceElement.ServiceRealizationType, serviceElement.InstanceName),
                    serviceElement.ServiceInterfaceType, serviceElement.InstanceName);
            }
        }

        /// <overloads>
        /// Returns an instance of type <typeparamref name="TServiceType"/>.
        /// </overloads>
        /// <summary>
        /// Returns a new default instance of type <typeparamref name="TServiceType"/> based on configuration information 
        /// from the default configuration source.
        /// </summary>
        /// <typeparam name="TServiceType">The type to build.</typeparam>
        /// <returns>A new instance of <typeparamref name="TServiceType"/> or any of it subtypes.</returns>
        public TServiceType BuildUp<TServiceType>()
        {
            return BuildUp<TServiceType>(null);
        }

        /// <overloads>
        /// Returns an instance of type <typeparamref name="TServiceType"/>.
        /// </overloads>
        /// <summary>
        /// Returns a new default instance of type <typeparamref name="TServiceType"/> based on configuration information 
        /// from the default configuration source.
        /// </summary>
        /// <typeparam name="TServiceType">The type to build.</typeparam>
        /// <returns>A new instance of <typeparamref name="TServiceType"/> or any of it subtypes.</returns>
        public TServiceType BuildUp<TServiceType>(string instanceName)
        {
            return (TServiceType) BuildUp(typeof (TServiceType), instanceName);
        }

        /// <overloads>
        /// Returns an instance of specified type />.
        /// </overloads>
        /// <summary>
        /// Returns a new default instance of specified type based on configuration information 
        /// from the default configuration source.
        /// </summary>
        /// <returns>A new instance of serviceType or any of it subtypes.</returns>
        public object BuildUp(Type serviceType)
        {
            return BuildUp(serviceType, null);
        }

        /// <overloads>
        /// Returns an instance of type serviceType.
        /// </overloads>
        /// <summary>
        /// Returns a new default instance of type serviceType based on configuration information 
        /// from the default configuration source.
        /// </summary>
        /// <returns>A new instance of serviceType or any of it subtypes.</returns>
        public object BuildUp(Type serviceType, string instanceName)
        {
            try
            {
                return builder.BuildUp(new SystemFactoryLocator(threadStaticTypes), serviceType,
                                       string.IsNullOrEmpty(instanceName) ? string.Empty : instanceName, null);
            }
            catch (ArgumentException ae)
            {
                var exception =
                    new SystemFactoryException(
                        string.Format(Resources.CannotBuildUpInstanceOfType, serviceType, instanceName), ae);

                LogManager.Log(exception);

                throw exception;
            }
        }

        public IEnumerable<TServiceType> BuildAll<TServiceType>()
        {
            return BuildAll<TServiceType>(false);
        }

        public IEnumerable<object> BuildAll(Type serviceType)
        {
            return BuildAll(serviceType, false);
        }

        public IEnumerable<TServiceType> BuildAll<TServiceType>(bool inherited)
        {
            return BuildAll(typeof (TServiceType), inherited).Cast<TServiceType>();
        }

        public IEnumerable<object> BuildAll(Type serviceType, bool inherited)
        {
            return from SystemFactoryConfigurationElement serviceElement in systemFactoryConfiguration.Services
                   where
                       serviceElement.ServiceInterfaceType == serviceType ||
                       inherited && serviceType.IsAssignableFrom(serviceElement.ServiceInterfaceType)
                   select BuildUp(serviceElement.ServiceInterfaceType, serviceElement.InstanceName);
        }

        private class SystemFactoryLocator : Locator
        {
            private static readonly ILifetimeContainer container = new LifetimeContainer();

            private static Dictionary<object, object> _staticInstances = new Dictionary<object, object>();

            [ThreadStatic] private static Dictionary<object, object> _threadStaticInstances;

            private readonly List<DependencyResolutionLocatorKey> threadStaticTypes;

            private static Dictionary<object, object> ThreadStaticInstances
            {
                get { return _threadStaticInstances ?? (_threadStaticInstances = new Dictionary<object, object>()); }
            }

            public SystemFactoryLocator(List<DependencyResolutionLocatorKey> threadStaticTypes)
            {
                this.threadStaticTypes = threadStaticTypes;

                if (_staticInstances == null) _staticInstances = new Dictionary<object, object>();
            }

            public override object Get(object key, SearchMode options)
            {
                if (BelongsToThreadStaticScope(key))
                {
                    if (ThreadStaticInstances.ContainsKey(key))
                    {
                        return ThreadStaticInstances[key];
                    }
                }
                else
                {
                    if (_staticInstances.ContainsKey(key))
                    {
                        return _staticInstances[key];
                    }
                }
                if (key is Type && typeof (LifetimeContainer) == key)
                {
                    return container;
                }
                return null;
            }

            public override bool Contains(object key, SearchMode options)
            {
                return BelongsToThreadStaticScope(key)
                           ? ThreadStaticInstances.ContainsKey(key)
                           : _staticInstances.ContainsKey(key);
            }

            public override void Add(object key, object value)
            {
                if (BelongsToThreadStaticScope(key))
                {
                    ThreadStaticInstances.Add(key, value);
                }
                else if (!_staticInstances.ContainsKey(key))
                {
                    _staticInstances.Add(key, value);
                }
            }

            public override bool Remove(object key)
            {
                return BelongsToThreadStaticScope(key)
                           ? ThreadStaticInstances.Remove(key)
                           : _staticInstances.Remove(key);
            }

            public override int Count
            {
                get { return _staticInstances.Count + ThreadStaticInstances.Count; }
            }

            private bool BelongsToThreadStaticScope(object o)
            {
                var dependencyResolutionLocatorKey = o as DependencyResolutionLocatorKey;

                return o != null && threadStaticTypes.Contains(dependencyResolutionLocatorKey);
            }
        }
    }
}
