namespace SentryExample.Core.IoC
{
    public interface IContainerManager
    {
        bool IsRegistered<TService>();
        TService Resolve<TService>();
    }
}
