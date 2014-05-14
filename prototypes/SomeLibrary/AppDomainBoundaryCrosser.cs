using System;
using JetBrains.Annotations;
using SharedInterfaces;

namespace SomeLibrary
{
    [UsedImplicitly]
    public class AppDomainBoundaryCrosser : AnAppDomainIdentifiable,
                                            IRemoteType
    {
        public void Execute(Action action)
        {
            action();
        }
    }
}