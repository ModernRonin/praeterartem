using System;

namespace ModernRonin.PraeterArtem.Reflection
{
    /// <summary>
    ///     Extensions for <see cref="AppDomain" />.
    /// </summary>
    public static class AppDomainExtensions
    {
        /// <summary>
        ///     Creates a type in an <see cref="AppDomain" />. Loads the
        ///     assembly where the type should be located into that AppDomain.
        ///     This is useful if you want to dynamically load types and later unload
        ///     them again. .NET does not allow unloading of types or assemblies, but
        ///     it allows unloading of AppDomains. So you create an AppDomain to hold your
        ///     dynamically loaded types (with <see cref="AppDomain.CreateDomain(string)" />)
        ///     and then use this method.
        /// </summary>
        /// <remarks>
        ///     Types loaded MUST inherit from <see cref="MarshalByRefObject" />.
        ///     They also MUST implement an interface of your choice. This interface
        ///     MUST be located in an assembly other than the one containing the type
        ///     to be instantiated. You specify this interface with <typeparamref name="T" />.
        ///     So what you get from this method is an interface, not the concrete type.
        ///     Also, make sure that your code does never refer to anything of the
        ///     dynamically loaded assembly directly. In particular, do NOT use
        ///     any typeof of a type located in that assembly. If you do,
        ///     the assembly will be loaded into your primary AppDomain which cannot be
        ///     unloaded, thus defeating the whole point of the exercise.
        ///     The interface type denoted by <typeparamref name="T" />
        ///     MUST NOT be defined in your calling code's assembly, either.
        ///     Incidentally, this is also the reason why you must pass the type to be instantiated as a string
        ///     to this method.
        ///     Also, note that the type to be instantiated MUST be default constructable. While it would have been
        ///     entirely possible to allow parameterized construction, for many cases default construction and
        ///     object initializers are the better choice, anyway, and besides, non-default constructable types
        ///     create hard-to-understand phenomena when being serialized (which they must be when crossing domain boundaries).
        ///     Last not least, note that all arguments you pass to methods on a remote type need to be marked as
        ///     [Serialized]. That includes, not so obviously, that if you pass lambdas as parameters, the class containing
        ///     the method in which the lambda is defined, needs to be marked as [Serializable], too.
        /// </remarks>
        /// <example>
        ///     You have at least three different assemblies:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>SharedInterfaces</description>
        ///         </item>
        ///         <item>
        ///             <description>CallingCode</description>
        ///         </item>
        ///         <item>
        ///             <description>DynamicallyLoadedCode</description>
        ///         </item>
        ///     </list>
        ///     SharedInterfaces contains:
        ///     <code>
        /// public interface IRemoteType
        /// {
        ///       void DoSomething();
        /// }
        /// </code>
        ///     DynamicallyLoadedCode references SharedInterfaces and contains:
        ///     <code>
        /// public class SomeImplementation : MarshalByRefObject, IRemoteType
        /// {
        ///     public void DoSomething()
        ///     {
        ///         // implementation
        ///     }
        /// }        
        /// </code>
        ///     CallingCode references SharedInterfaces and contains:
        ///     <code>
        ///     const string fullPathToAssembly = "DynamicallyLoadedCode.dll";
        ///     const string namespaceQualifiedTypeName = "DynamicallyLoadedCode.SomeImplementation";
        ///     var remoteType = someDomain.CreateTypeInDomain{IRemoteType}(fullPathToAssembly, namespaceQualifiedTypeName);
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="domain"></param>
        /// <param name="assemblyFilePath"></param>
        /// <param name="concreteTypeName"></param>
        /// <returns></returns>
        public static T CreateTypeInDomain<T>(this AppDomain domain,
                                              string assemblyFilePath,
                                              string concreteTypeName)
        {
            return TypeLoader.CreateTypeInDomain<T>(domain, assemblyFilePath,
                concreteTypeName);
        }
        /// <summary>
        ///     Executes an action in another AppDomain.
        ///     Note that any lambdas passed must be serializable. That implies they should
        ///     not reference local variables of the method calling Execute. If the lambda
        ///     accesses member fields of the calling method's class, that class must be marked
        ///     as [Serializable].
        /// </summary>
        public static void Execute(this AppDomain domain, Action action)
        {
            RemoteDomainExecutor.ExecuteIn(domain, action);
        }
        /// <summary>
        ///     Executes an function with a single parameter in another AppDomain and returns
        ///     the result.
        ///     Note that any lambdas passed must be serializable. That implies they should
        ///     not reference local variables of the method calling Execute. If the lambda
        ///     accesses member fields of the calling method's class, that class must be marked
        ///     as [Serializable].
        /// </summary>
        public static TResult Execute<TArgument, TResult>(
            this AppDomain domain, TArgument argument,
            Func<TArgument, TResult> function)
        {
            var result = RemoteDomainExecutor.ExecuteIn(domain, argument,
                function);
            return result;
        }
        /// <summary>
        ///     Executes an action with a parameter in another AppDomain.
        ///     Note that any lambdas passed must be serializable. That implies they should
        ///     not reference local variables of the method calling Execute. If the lambda
        ///     accesses member fields of the calling method's class, that class must be marked
        ///     as [Serializable].
        /// </summary>
        public static void Execute<T>(this AppDomain domain, T argument,
                                      Action<T> action)
        {
            RemoteDomainExecutor.ExecuteIn(domain, argument, action);
        }
    }
}