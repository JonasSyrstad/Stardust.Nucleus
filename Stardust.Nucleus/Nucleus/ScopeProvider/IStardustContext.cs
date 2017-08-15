using System;

namespace Stardust.Nucleus.ScopeProvider
{
    public interface IStardustContext : IDisposable
    {
        Guid ContextId { get; }
        void SetDisconnectorAction(Action<object> action);
        void ClearDisposeActoion();
    }
}