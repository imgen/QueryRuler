using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QueryRuler
{
    public class OrderedItemSummaryQueryRulers: BaseQueryRulers
    {
        public OrderedItemSummaryQueryRulers(ITestOutputHelper testOutputHelper): base(testOutputHelper) { }

        protected override string TableName => "vw_OrderedItemSummary";

        [Fact]
        public async Task GetCountOfOrderedItemSummaries()
        {
            await MeasureWithQuery(query => query.AsCount());
        }

        [Fact]
        public async Task GetDistinctedCountOfOrderedItemSummaries()
        {
            await MeasureWithQuery(query => query.Distinct().AsCount(), commandTimeout: 300);
        }

        [Fact]
        public async Task GetRoughCountOfOrderedItemSummaries()
        {
            await MeasureWithQuery(query => query.WhereRaw("PurchaseOrderItemId % 10 = 1").AsCount());
        }
    }
}
