using System;
using Stardust.Core.Pool;
using Stardust.Nucleus;
using Stardust.Particles;

namespace Stardust.Core
{
    /// <summary>
    /// Loads the framework bindings using the default logger.
    /// </summary>
    internal class CoreFrameworkBlueprint : CoreFrameworkBlueprint<LoggingDefaultImplementation>
    {
        
    }

    /// <summary>
    /// Loads the framework bindings using a provided logging implementation
    /// logger.
    /// </summary>
    /// <typeparam name="T">
    /// An implementation of the ILogging <see langword="interface"/>
    /// </typeparam>
    public class CoreFrameworkBlueprint<T> : IBlueprint
    {

        public void Bind(IConfigurator resolver)
        {
           
            resolver.Bind<IStardustPerformanceMetrics>().To<ConnectionStringPoolContainerMetrics>("ConnectionStringPool").SetSingletonScope();
            FrameworkBindings();
            
        }

        /// <summary>
        /// The <see cref="Type" /> of the <see cref="ILogging"/> implementation.
        /// </summary>
        public virtual Type LoggingType
        {
            get { return typeof (T); }
        }

        /// <summary>
        /// This should only be called by Stardust. This is where we apply all
        /// bindings that are required for the framework to work and have a
        /// consistent behavior.
        /// </summary>
        protected internal virtual void FrameworkBindings()
        {}

        
    }
}