using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace EasyAbp.PaymentService.WeChatPay.EntityFrameworkCore
{
    public class WeChatPayModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public WeChatPayModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}