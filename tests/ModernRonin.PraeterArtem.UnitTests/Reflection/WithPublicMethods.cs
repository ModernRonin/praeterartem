using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.UnitTests.Reflection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers /* By reflection*/
        )]
    abstract class WithPublicMethods
    {
        public void A() {}
        public static void B() {}
    }
}