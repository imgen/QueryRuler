using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QueryRuler
{
    public class TransitTimeQueryRulers: BaseQueryRulers
    {
        public TransitTimeQueryRulers(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        protected override string TableName => "vw_TransitTime";

        [Fact]
        public async Task GetCountOfTransitTimes()
        {
            await MeasureWithCountQuery();
        }

        [Fact]
        public async Task GetRoughCountOfTransitTimes()
        {
            await MeasureWithCountQuery(query => query.WhereRaw("ShipmentItemId % 10 = 1"));
        }

        [Fact]
        public async Task GetDistinctedCountOfTransitTimes()
        {
            await MeasureWithDistinctCountQuery();
        }

        [Fact]
        public async Task GetTop500VendorTransitTimes()
        {
            await MeasureWithTopQuery(500, OrderByPurchaseOrderNumber);
        }

        [Fact]
        public async Task GetTop500TransitTimesEfficiently()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(500, OrderByPurchaseOrderNumber);
        }
    }
}
