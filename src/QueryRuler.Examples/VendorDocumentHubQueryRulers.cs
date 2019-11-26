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
            await MeasureWithCountQuery();
        }

        [Fact]
        public async Task GetDistinctdCountOfVendorDocuments()
        {
            await MeasureWithDistinctCountQuery();
        }

        [Fact]
        public async Task GetRoughCountOfVendorDocuments()
        {
            await MeasureWithCountQuery(query => query.WhereRaw("PurchaseOrderItemId % 10 = 1"));
        }

        [Fact]
        public async Task GetAllVendorDocuments()
        {
            await MeasureWithQuery();
        }

        [Fact]
        public async Task GetAllDistinctedDocuments()
        {
            await MeasureWithDistinctQuery();
        }

        

        [Fact]
        public async Task GetTop5000VendorDocuments()
        {
            await MeasureWithTopQuery(5000, OrderByPurchaseOrderNumber);
        }

        [Fact]
        public async Task GetTop5000VendorDocumentsEfficiently()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(5000, OrderByPurchaseOrderNumber);
        }

        [Fact]
        public async Task GetTop500VendorDocuments()
        {
            await MeasureWithTopQuery(500, OrderByPurchaseOrderNumber);
        }

        [Fact]
        public async Task GetTop500VendorDocumentsEfficiently()
        {
            await MeasureWithTopQueryInSelectTopMaxSubQuery(500, OrderByPurchaseOrderNumber);
        }
    }
}
