using System;

namespace SharedInterfaces
{
    public interface IRemoteType : IAppDomainIdentifiable
    {
        void Execute(Action action);
        void WriteError();
        string Error { get; }
    }
}