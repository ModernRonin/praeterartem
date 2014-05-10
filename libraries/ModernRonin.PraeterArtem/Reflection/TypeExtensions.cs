using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    /// <summary>
    ///     Just a bunch of extensions for <see cref="Type" /> which MS has forgotten.
    /// </summary>
    public static class TypeExtensions
    {
        static readonly Dictionary<Type, string> sCSharpTypeAliases =
            new Dictionary<Type, string>
            {
                {typeof (string), "string"},
                {typeof (void), "void"},
                {typeof (object), "object"},
                {typeof (bool), "bool"},
                {typeof (char), "char"},
                {typeof (int), "int"},
                {typeof (uint), "uint"},
                {typeof (long), "long"},
                {typeof (ulong), "ulong"},
                {typeof (short), "short"},
                {typeof (ushort), "ushort"},
                {typeof (byte), "byte"},
                {typeof (sbyte), "sbyte"},
                {typeof (float), "float"},
                {typeof (double), "double"},
            };
        /// <summary>Check whether an attribute is set for this member.</summary>
        /// <remarks>Multiple attributes count as true, too. Also, attributes derived from <typeparamref name="T" /> count, too.s</remarks>
        /// <returns>whether a custom attribute of member <typeparamref name="T" /> is set on <paramref name="member" /></returns>
        public static bool HasAttribute<T>([NotNull] this MemberInfo member)
            where T : Attribute
        {
            if (member == null)
                throw new ArgumentNullException("member");
            return Attribute.IsDefined(member, typeof (T));
        }
        /// <summary>
        ///     Indicates whether there are any public methods declared for this type, excluding methods defined in base
        ///     classes.
        /// </summary>
        public static bool HasPublicMethods([NotNull] this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return
                type.GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                BindingFlags.Static)
                    .Where(m => !m.IsSpecialName)
                    .Any(m => m.DeclaringType == type);
        }
        /// <summary>
        ///     Gets a pretty name for a type even if it is generic. For example, for Dictionary{string,object} it will return
        ///     exactly that instead of
        ///     "Dictionary'1[System.String...] as type.Name would do. Also uses <see cref="CSharpName" />, so CLR types are
        ///     translated to their C# counterparts where possible.
        /// </summary>
        public static string PrettyName(this Type type)
        {
            return type.IsGenericType
                ? String.Format("{0}<{1}>", type.Name.Split('`')[0],
                    String.Join(", ",
                        type.GetGenericArguments()
                            .Select(PrettyName)
                            .ToArray()))
                : type.CSharpName();
        }
        /// <summary>
        ///     Returns the C# type name instead of the CLR type name for those types which have a special name in C#, for example
        ///     "int" instead of Int32 etc.
        /// </summary>
        public static string CSharpName(this Type type)
        {
            if (sCSharpTypeAliases.ContainsKey(type))
                return sCSharpTypeAliases[type];
            return type.Name;
        }
    }
}