using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Reflection
{
    /// <summary>
    /// Extensions method on <see cref="MemberInfo"/>, stuff frequently used and missing in the BCL.
    /// </summary>
    public static class MemberInfoExtensions
    {
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
    }
}