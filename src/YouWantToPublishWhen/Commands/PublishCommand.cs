using System;
using System.Linq;
using Chips.Sitecore.Commands;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Publishing;
using Sitecore.Tasks;

namespace YouWantToPublishWhen.Commands.Publish
{
    public class PublishCommand : ICommand
    {
        private readonly CommandConfigurationFactory<PublishConfiguration> _coinfigConfigurationFactory;

        public PublishCommand(CommandConfigurationFactory<PublishConfiguration> coinfigConfigurationFactory)
        {
            _coinfigConfigurationFactory = coinfigConfigurationFactory;
        }

        public PublishCommand()
        {
        }

        public void Execute(Item[] items, CommandItem commandItem, ScheduleItem schedule)
        {
            var config = _coinfigConfigurationFactory.DoGetCommandConfiguration(commandItem);
            var dbSource = Sitecore.Configuration.Factory.GetDatabase(config.Source);
            var dbTarget = Sitecore.Configuration.Factory.GetDatabase(config.Target);

            var languages = config.LanguagesToPublish.Select(id => dbSource.GetItem(id)).ToArray();
            
            foreach (var id in config.ItemsToPublish)
            {
                foreach (var language in languages)
                {
                    var publishLanguage = dbSource.Languages.SingleOrDefault(l => l.Origin.ItemId == language.ID);
                    if(publishLanguage == null)
                    {
                        continue;
                    }

                    var item = dbSource.GetItem(id);
                    if (item == null)
                    {
                        continue;
                    }

                    var publishSubitems = config.PublishSubitems ?? false;

                    RunPublishJob(item, dbSource, dbTarget, config.PublishMode, publishLanguage, publishSubitems);
                }
            }
        }

        private void RunPublishJob(Item item, Database sourceDatabase,  Database targetDatabase, PublishMode publishMode, Language language, bool deep)
        {
            var options = new PublishOptions(sourceDatabase, targetDatabase, publishMode, language, DateTime.Now)
            {
                Deep = deep,
                PublishRelatedItems = false,
                RootItem = item
            };

            try
            {
                var publisher = new Publisher(options);
                publisher.Publish();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error publishing.", ex, this);
            }
        }
    }
}