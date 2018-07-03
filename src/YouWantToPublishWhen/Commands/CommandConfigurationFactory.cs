using System;
using Sitecore.Tasks;

namespace YouWantToPublishWhen.Commands
{
    public abstract class CommandConfigurationFactory<TCommandConfiguration> where TCommandConfiguration : CommandConfiguration
    {
        public Type FatoryFor => typeof(TCommandConfiguration);

        public TCommandConfiguration GetCommandConfiguration(CommandItem commandItem)
        {
            return DoGetCommandConfiguration(commandItem);
        }

        public abstract TCommandConfiguration DoGetCommandConfiguration(CommandItem commandItem);
    }
}