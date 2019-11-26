using SqlKata;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QueryRuler
{
    public class VendorDocumentHubQueryRulers: BaseQueryRulers
    {
        public VendorDocumentHubQueryRulers(ITestOutputHelper testOutputHelper): base(testOutputHelper) { }

        protected override string TableName => "vw_VendorDocumentHub";

        [Fact]
        public async Task GetCountOfVendorDocuments()
        {
            await MeasureWithQuery(query => query.AsCount());
        }

        [Fact]
        public async Task GetDistinctdCountOfVendorDocuments()
        {
            await MeasureWithQuery(query => query.Distinct().AsCount());
        }

        [Fact]
        public async Task GetRoughCountOfVendorDocuments()
        {
            await MeasureWithQuery(query => query.WhereRaw("PurchaseOrderItemId % 10 = 1").AsCount());
        }

        [Fact]
        public async Task GetAllVendorDocuments()
        {
            await MeasureWithQuery();
        }

        [Fact]
        public async Task GetAllDistinctedDocuments()
        {
            await MeasureWithQuery(query => query.Distinct());
        }

        [Fact]
        public async Task GetTop5000VendorDocuments()
        {
            await MeasureWithQuery(query => query.OrderBy("PurchaseOrderNumber").Take(5000));
        }

        [Fact]
        public async Task GetTop5000VendorDocumentsEfficiently()
        {
            await MeasureWithQuery(query => 
            {
                query.OrderBy("PurchaseOrderNumber").Take(int.MaxValue).As("q");
                var outerQuery = new Query().From(query);
                return outerQuery.Take(5000);
            });
        }
    }
}
