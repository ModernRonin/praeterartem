using System;
using System.IO;
using SharedInterfaces;

namespace SomeLibrary
{
    public class SomeImplementation : MarshalByRefObject, IRemoteType
    {
        readonly StringWriter mError= new StringWriter();
        public SomeImplementation()
        {
            Console.SetError(mError);
        }
        public string Error
        {
            get { return mError.ToString(); }
        }
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
        public void WriteError()
        {
            Console.Error.Write("PAPA!!!");
        }
    }
}