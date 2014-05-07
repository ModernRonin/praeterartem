using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.Xml
{
    /// <summary>
    ///     An implementation of <see cref="IEqualityComparer{T}" />
    ///     which deals correctly with changed order of attributes or child
    ///     nodes, in contrast to <see cref="XNode.DeepEquals" />.
    /// </summary>
    public sealed class XElementEqualityComparer :
        IEqualityComparer<XElement>
    {
        public bool Equals([NotNull] XElement x, [NotNull] XElement y)
        {
            var lhs = Normalize(x);
            var rhs = Normalize(y);
            return XNode.DeepEquals(lhs, rhs);
        }
        public int GetHashCode([NotNull] XElement obj)
        {
            return Normalize(obj).GetHashCode();
        }
        [NotNull]
        static XElement Normalize([NotNull] XElement element)
        {
            if (element.HasElements)
            {
                return new XElement(element.Name,
                    element.Attributes().OrderBy(a => a.Name.ToString()),
                    element.Elements()
                           .OrderBy(a => a.Name.ToString())
                           .Select(Normalize));
            }

            if (element.IsEmpty || string.IsNullOrEmpty(element.Value))
            {
                return new XElement(element.Name,
                    element.Attributes().OrderBy(a => a.Name.ToString()));
            }

            return new XElement(element.Name,
                element.Attributes().OrderBy(a => a.Name.ToString()),
                element.Value);
        }
    }
}