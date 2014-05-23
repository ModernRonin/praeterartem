using System;

namespace ModernRonin.PraeterArtem
{
    /// <summary>
    /// Abstracts what DateTime.Now does to allow mocking in tests
    /// and also to more OO distinguish between .Now and .UtcNow.
    /// </summary>
    public interface ITimeGiver
    {
        DateTime Now { get; }
    }
}