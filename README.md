# Stardust.Nucleus
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
