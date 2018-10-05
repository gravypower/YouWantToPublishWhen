using System;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Publishing;
using Sitecore.Tasks;

namespace YouWantToPublishWhen.Commands.Publish
{
    public class PublishConfigurationFactory : CommandConfigurationFactory<PublishConfiguration>
    {
        public override PublishConfiguration DoGetCommandConfiguration(CommandItem commandItem)
        {
            var config = new PublishConfiguration();
            var fields = commandItem.InnerItem.Fields;

            var publishSubItems = (CheckboxField)fields["Publish Sub-items"];
            config.PublishSubitems = publishSubItems?.Checked;

            config.PublishMode = PublishMode(fields);

            config.Source = fields["Source"].Value;
            config.Target = fields["Target"].Value;

            var itemsToPublish = (MultilistField)fields["Items"];
            config.ItemsToPublish = itemsToPublish.TargetIDs;

            var languagesToPublish = (MultilistField)commandItem.InnerItem.Fields["Languages"];
            config.LanguagesToPublish = languagesToPublish.TargetIDs;

            return null;
        }

        private static PublishMode PublishMode(FieldCollection fields)
        {
            var publishModeItem = (ReferenceField) fields["Publish Mode"];
            var publishModeValue = publishModeItem.TargetItem["value"];

            Enum.TryParse(publishModeValue, out PublishMode publishMode);

            if (publishMode == Sitecore.Publishing.PublishMode.Unknown)
            {
                publishMode = Sitecore.Publishing.PublishMode.Smart;
            }

            return  publishMode;
        }
    }
}