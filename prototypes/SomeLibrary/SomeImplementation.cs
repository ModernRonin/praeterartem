using System;
using SharedInterfaces;

namespace SomeLibrary
{
    public class SomeImplementation : MarshalByRefObject, IRemoteType
    {
        public int AppDomainIdentifier
        {
            get
            {
                return AppDomain.CurrentDomain.Id; 
            }
        }
        public void Execute(Action action)
        {
            action();
        }
    }
}