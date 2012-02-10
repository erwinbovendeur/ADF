using System;
using System.Collections.Generic;
using Adf.Core.Logging;
using Adf.Core.Objects;
using Adf.ObjectBuilder.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Adf.Objects.Unity
{
    public class UnityProvider : IObjectProvider
    {
        private readonly IUnityContainer unityContainer;

        public UnityProvider()
        {
            unityContainer = new UnityContainer();
            unityContainer.LoadConfiguration();
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
                return unityContainer.Resolve(serviceType, instanceName);
            }
            catch (ArgumentException ae)
            {
                var exception = new SystemFactoryException(string.Format(Resources.CannotBuildUpInstanceOfType, serviceType, instanceName), ae);

                LogManager.Log(exception);

                throw exception;
            }
        }

        public IEnumerable<object> BuildAll(Type serviceType, bool inherited)
        {
            return unityContainer.ResolveAll(serviceType);
        }
    }

}
