using SqlKata;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QueryRuler
{
    public class TransitTimeQueryRulers: BaseQueryRulers
    {
        public TransitTimeQueryRulers(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        protected override string TableName => "vw_TransitTime";

        private Query OrderByContainerNumber(Query query) => query.OrderBy("ContainerNumber");

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
        public async Task GetTop500TransitTimes()
        {
            await MeasureWithTopQuery(500, OrderByContainerNumber);
        }

        [Fact]
        public async Task GetTop500TransitTimesWithoutOrdering()
        {
            await MeasureWithTopQuery(500);
        }

        [Fact]
        public async Task GetTop500TransitTimesEfficiently()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(500, OrderByContainerNumber);
        }

        [Fact]
        public async Task GetTop500TransitTimesEfficientlyWithoutOrdering()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(500);
        }

        [Fact]
        public async Task GetAllTransitTimesWithoutOrdering()
        {
            await MeasureWithQuery();
        }
    }
}
