using System;

namespace SharedInterfaces
{
    public abstract class AnAppDomainIdentifiable : MarshalByRefObject, IAppDomainIdentifiable
    {
        public int AppDomainIdentifier
        {
            get { return AppDomain.CurrentDomain.Id; }
        }
    }
}