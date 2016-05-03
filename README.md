# Stardust.Nucleus
![alt text][logo]

[logo]: http://stardustframework.wdfiles.com/local--files/blog%3A_start/logo_smal.png "Stardust"
Contains the stardust IOC and basic helpers.

TODO: Add code samples.
```CSharp
public class AppBlueprint: Blueprint<LoggingDefaultImplementation>
{
        protected sealed override void DoCustomBindings()
        {
            Configurator.Bind<IService>().To<ServiceImplementation>().SetTransientScope();
        }
}


```

```CSharp
Resolver.Activate<IService>().DoStuff();


```
