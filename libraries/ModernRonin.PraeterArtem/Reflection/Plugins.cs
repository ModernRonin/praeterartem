using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModernRonin.PraeterArtem.Reflection
{
    /// <summary>
    /// Helps with enumerating concrete implementations of a specific type
    /// and with instantiating them.
    /// </summary>
    public static class Plugins
    {
        static IEnumerable<Type> AllAvailableTypes
            => AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
        static IEnumerable<Type> GetTypes<T>() => AllAvailableTypes.SelectConcreteImplementations<T>();
        static IEnumerable<Type> SelectConcreteImplementations<T>(this IEnumerable<Type> self)
        {
            return self.Where(t => typeof (T).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic);
        }
        public static IEnumerable<Type> GetConcreteImplementationsOf<T>(this Assembly self)
        {
            return self.GetTypes().SelectConcreteImplementations<T>();
        }
        public static IEnumerable<Type> GetConcreteImplementationsInSameAssembly<T>()
        {
            return typeof (T).Assembly.GetConcreteImplementationsOf<T>();
        }
        public static IDictionary<TKey, TPluginInterface> Instantiate<TKey, TPluginInterface>(
            Func<TPluginInterface, TKey> keyExtractor)
        {
            return Instantiate(keyExtractor, t => (TPluginInterface) Activator.CreateInstance(t));
        }
        /// <summary>
        /// Creates a dictionary of some selector criterion to instances of a certain type.
        /// Typically, the criterion will be an enum-like property that each type has. 
        /// For example:
        /// <example>
        /// enum SourceLanguage { German, French, Spanish }
        /// interface ITranslator
        /// { 
        ///    SourceLanguage HandledLanguage {get;}
        ///    string TranslateToEnglish(string input);
        /// }
        /// class GermanTranslator
        /// {
        ///    SourceLanguage HandledLanguage { get { return SourceLanguage.German; } }
        ///    ...
        /// }
        /// class SpanishTranslator ...
        /// class FrenchTranslator ...
        /// </example>
        /// If you called Plugins.Instantiate{SourceLanguage, ITranslator}(t => t.HandledLanguage),
        /// you'd get {{ SourceLanguage.German -> GermanTranslator}, {SourceLanguage.Spanish -> SpanishTranslator}, {SourceLanguage.French -> FrenchTranslator }}
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TPluginInterface"></typeparam>
        /// <param name="keyExtractor"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TPluginInterface> Instantiate<TKey, TPluginInterface>(
            Func<TPluginInterface, TKey> keyExtractor, Func<Type, TPluginInterface> creator)
        {
            var pluginTypes = GetTypes<TPluginInterface>();
            var pluginInstances = pluginTypes.Select(creator);
            return pluginInstances.ToDictionary(keyExtractor);
        }
    }
}