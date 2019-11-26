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
            await MeasureWithQuery(query => query.AsCount());
        }

        [Fact]
        public async Task GetRoughCountOfTransitTimes()
        {
            await MeasureWithQuery(query => query.WhereRaw("ShipmentItemId % 10 = 1").AsCount());
        }
    }
}
