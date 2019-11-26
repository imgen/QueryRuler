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
            await MeasureWithTopQuery(5000, query => query.OrderBy("PurchaseOrderNumber"));
        }

        [Fact]
        public async Task GetTop5000VendorDocumentsEfficiently()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(5000, query => query.OrderBy("PurchaseOrderNumber"));
        }

        [Fact]
        public async Task GetTop500VendorDocuments()
        {
            await MeasureWithTopQuery(500, query => query.OrderBy("PurchaseOrderNumber"));
        }

        [Fact]
        public async Task GetTop500VendorDocumentsEfficiently()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(500, query => query.OrderBy("PurchaseOrderNumber"));
        }
    }
}
