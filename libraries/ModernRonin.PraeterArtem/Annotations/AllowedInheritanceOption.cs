namespace ModernRonin.PraeterArtem.Annotations
{
	public enum AllowedInheritanceOption
	{
        /// <summary>
        /// The class is designed to be inherited
        /// </summary>
		BecauseItIsDesignedSo,
        /// <summary>Some classes may inherit this class in dynamic code 
        /// (like EntityFramework, Mocks, etc.)
        /// but in our code this is prohibited </summary>
        Because,
		/// <summary>
		/// Entity framework code-first POCO entity
		/// </summary>
		EfCodeFirstEntity
	}
}