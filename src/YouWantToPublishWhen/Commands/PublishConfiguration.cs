using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Publishing;

namespace YouWantToPublishWhen.Commands.Publish
{
    public class PublishConfiguration : CommandConfiguration
    {
        public bool? PublishSubitems { get; set; }
        public PublishMode PublishMode { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public IEnumerable<ID> ItemsToPublish { get; set; }
        public IEnumerable<ID> LanguagesToPublish { get; set; }
    }
}