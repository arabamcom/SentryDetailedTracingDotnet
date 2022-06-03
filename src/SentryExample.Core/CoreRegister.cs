using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SentryExample.Core.Kafka;
using SentryExample.Core.Repositories.Product;
using SentryExample.Core.Services;
using SentryExample.Core.Tracing;

namespace SentryExample.Core
{
    public class CoreRegister : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<SentryInterceptor>().LifeStyle.Transient);

            container.Register(Component.For<IKafkaProducerService>()
                .ImplementedBy<KafkaProducerService>()
                .LifestyleSingleton()
                .Interceptors<SentryInterceptor>());

            container.Register(Component.For<IProductService>()
                .ImplementedBy<ProductService>()
                .LifestyleSingleton()
                .Interceptors<SentryInterceptor>());

            container.Register(Component.For<IProductRepository>()
                .ImplementedBy<ProductRepository>()
                .LifestyleTransient()
                .Interceptors<SentryInterceptor>());
        }
    }
}
