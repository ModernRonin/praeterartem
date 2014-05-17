using JetBrains.Annotations;

namespace ModernRonin.PraeterArtem.UnitTests.Reflection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers /* By reflection*/
        )]
    sealed class WithoutPublicMethods : WithPublicMethods
    {
        void X() {}
        static void Y() {}
    }
}