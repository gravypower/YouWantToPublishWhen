using Chips.DependencyInjection;
using SimpleInjector;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;

namespace YouWantToPublishWhen
{
    public class BootstrapContainer : IBootstrap<Container>
    {
        public void Bootstrap(Container container)
        {
            container.Register(() => ServiceLocator.GetRequiredResetableService<BaseLog>().Value);
        }
    }
}
