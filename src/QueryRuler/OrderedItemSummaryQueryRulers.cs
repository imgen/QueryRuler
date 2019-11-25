using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QueryRuler
{
    public class OrderedItemSummaryQueryRulers: QueryRulers
    {
        public OrderedItemSummaryQueryRulers(ITestOutputHelper testOutputHelper): base(testOutputHelper) 
        {
            TableName = "vw_OrderedItemSummary";
        }

        
        [Fact]
        public async Task GetCountOfOrderedItemSummaries()
        {
            await MeasureWithQuery(query => query.AsCount());
        }

        [Fact]
        public async Task GetDistinctedCountOfOrderedItemSummaries()
        {
            await MeasureWithQuery(query => query.Distinct().AsCount());
        }

        [Fact]
        public async Task GetRoughCountOfOrderedItemSummaries()
        {
            await MeasureWithQuery(query => query.WhereRaw("PurchaseOrderItemId % 10 = 1").AsCount());
        }
    }
}
