using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SentryExample.Core;
using SentryExample.Core.IoC;
using SentryExample.Core.Kafka;
using SentryExample.Core.Tracing;

namespace SentryExample.Api1
{
    public static class BootStrapper
    {
        public static IWindsorContainer InitializeContainer()
        {
            var container = ContainerManager.WindsorContainer;
            container.Install(new CoreRegister());
            return container;
        }
    }
}
