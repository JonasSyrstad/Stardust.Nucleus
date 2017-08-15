# Stardust.Nucleus
![alt text][logo]

[logo]: http://stardustframework.wdfiles.com/local--files/blog%3A_start/logo_smal.png "Stardust"
Stardust.Nucleus is an fast and lightweight IoC container for .net.  

## nuget

asp.net
```nuget
PM> Install-Package Stardust.Nucleus.Web
```

console & class libraries
```nuget
PM> Install-Package Stardust.Nucleus
```

TODO: Add code samples.

### Web application configuration

the nuget package Stardust.Nucleus.Web creates a class located in App_Start called AppBlueprint, this is where you configure the IoC container bindings and sets the object life cycle scope. 
```CSharp
   [IocConfiguration]//Remove this and call Resolver.LoadModuleConfiguration<AppBlueprint>(); from Global.asax Application_Start if you experience problems with early loading of the ioc bindings
    public partial class AppBlueprint:BlueprintBase
    {
        partial void LoadConfiguration(IConfigurator configuration)
        {
            //Configures the IOC Container to use SomeServiceImplementation for ISomeService
            configuration.Bind<ISomeService>().To<SomeServiceImplementation>().SetSingletonScope();
            //binds ISomeService to SomeNamedServiceImplementation with the implementation key implementationName
            configuration.Bind<ISomeService>().To<SomeNamedServiceImplementation>("implementationName").SetSingletonScope();    	    
        }
    }
```

## Configuration api:

Bind\<T>()

```CSharp
//Gets an instance of the default implementation
Resolver.Activate<IService>().DoStuff();
//Gets an instance of a named implementation
Resolver.Activate<IService>("implementationName").DoStuff();


```

# Stardust.Particles

This package contains a suite of helper classes and extension methods to improve readability and provide implementations for common problems and tasks 
