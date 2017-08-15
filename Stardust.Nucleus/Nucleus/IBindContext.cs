using System;
using Stardust.Nucleus.TypeResolver;

namespace Stardust.Nucleus
{
    public interface IBindContext
    {
        /// <summary>
        /// Binds an implementation to its base class or interface
        /// </summary>
        IScopeContext To(Type type, string identifier = "default");

        IScopeContext To(Type type, Enum identifier);
    }
    public interface IBindContext<T> : IBindContext
    {
        /// <summary>
        /// Binds an implementation to its base class or interface
        /// </summary>
        IScopeContext To<TImplementation>() where TImplementation : T;

        /// <summary>
        /// Binds an implementation to its base class or interface
        /// </summary>
        IScopeContext To<TImplementation>(string identifier) where TImplementation : T;

        /// <summary>
        /// Binds an instace of an implementation to its base class or interface
        /// </summary>
        IScopeContext ToInstance(T instance, string identifier = TypeLocatorNames.DefaultName);

        /// <summary>
        /// Binds an implementation to its base class or interface
        /// </summary>
        IScopeContext To<TImplementation>(Enum identifier) where TImplementation : T;

        /// <summary>
        /// Binds the service to it self
        /// </summary>
        IScopeContext ToSelf();

        /// <summary>
        /// Binds the service to it self
        /// </summary>
        IScopeContext ToSelf(string identifier);

        /// <summary>
        /// Binds the service to it self
        /// </summary>
        IScopeContext ToSelf(Enum identifier);
        /// <summary>
        /// bind the service to the constructor function.
        /// </summary>
        /// <param name="creator">The creator function</param>
        /// <returns></returns>
        IScopeContext ToConstructor(Func<object> creator);

        /// <summary>
        /// bind the service to the constructor function.
        /// </summary>
        /// <param name="creator">The creator function</param>
        /// <param name="identifier">Implementation key</param>
        /// <returns></returns>
        IScopeContext ToConstructor(Func<object> creator, string identifier);

        /// <summary>
        /// bind the service to the constructor function.
        /// </summary>
        /// <param name="type">The type of object created through the function</param>
        /// <param name="creator">The creator function</param>
        /// <returns></returns>
        IScopeContext ToConstructor(Type type,Func<object> creator);
        /// <summary>
        /// bind the service to the constructor function.
        /// </summary>
        /// <param name="type">The type of object created through the function</param>
        /// <param name="creator">The creator function</param>
        /// <param name="identifier">Implementation key</param>
        /// <returns></returns>
        IScopeContext ToConstructor(Type type, Func<object> creator, string identifier);
    }
}