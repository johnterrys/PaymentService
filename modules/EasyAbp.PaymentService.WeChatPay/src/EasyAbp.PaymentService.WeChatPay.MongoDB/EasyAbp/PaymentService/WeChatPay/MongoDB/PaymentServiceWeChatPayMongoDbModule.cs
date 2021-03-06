using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace EasyAbp.PaymentService.WeChatPay.MongoDB
{
    [DependsOn(
        typeof(PaymentServiceWeChatPayDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class PaymentServiceWeChatPayMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<WeChatPayMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });
        }
    }
}
