using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sample.Commerce.Plugin.Index.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Search;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Sample.Commerce.Plugin.Index
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IIncrementalIndexMinionPipeline>(c =>
                    c.Add<InitializeGiftcardsIndexingViewBlock>().After<InitializeIndexingViewBlock>())
                .ConfigurePipeline<IFullIndexMinionPipeline>(c =>
                    c.Add<InitializeGiftcardsIndexingViewBlock>().After<InitializeIndexingViewBlock>())
            );

            services.RegisterAllCommands(assembly);
        }
    }
}