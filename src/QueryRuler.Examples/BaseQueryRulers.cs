using SqlKata;
using Xunit.Abstractions;

namespace QueryRuler
{
    public abstract class BaseQueryRulers: QueryRulers
    {
        protected override string ConnectionString => "Data Source=(local);Initial Catalog=ProductionGravity;Integrated Security=True;MultipleActiveResultSets=True";

        protected BaseQueryRulers(ITestOutputHelper testOutputHelper): base(testOutputHelper.WriteLine) { }

        protected Query OrderByPurchaseOrderNumber(Query query) => query.OrderBy("PurchaseOrderNumber");
    }
}
