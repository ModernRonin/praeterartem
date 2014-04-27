using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ModernRonin.PraeterArtem.Xml
{
    /// <summary>
    ///     An implementation of <see cref="IEqualityComparer{T}" />
    ///     which deals correctly with changed order of attributes or child
    ///     nodes, in contrast to <see cref="XNode.DeepEquals" />.
    /// </summary>
    public class XElementEqualityComparer : IEqualityComparer<XElement>
    {
        public bool Equals(XElement x, XElement y)
        {
            return XNode.DeepEquals(Normalize(x), Normalize(y));
        }
        public int GetHashCode(XElement obj)
        {
            return Normalize(obj).GetHashCode();
        }
        static XElement Normalize(XElement element)
        {
            if (element.HasElements)
            {
                return new XElement(element.Name,
                    element.Attributes()
                           .Where(a => a.Name.Namespace == XNamespace.Xmlns)
                           .OrderBy(a => a.Name.ToString()),
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