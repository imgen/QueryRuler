using Dakata;
using Dakata.SqlServer;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryRuler
{
    public abstract class QueryRulers
    {
        private readonly Action<string> _messageProcessor;

        protected abstract string TableName { get; } 

        protected abstract string ConnectionString { get; }

        protected QueryRulers(Action<string> messageProcessor) => _messageProcessor = messageProcessor;

        protected const int ExecutionTimes = 3, WarmUpTimes = 1;
        protected static readonly TimeSpan ExecutionInterval = TimeSpan.FromSeconds(2);

        protected async Task Measure((BaseDal dal, Query query) dalAndQuery)
        {
            await Measure(dalAndQuery.dal, dalAndQuery.query);
        }
        protected async Task Measure(BaseDal dal, Query query, int? commandTimeout = null)
        {
            var elapsedTimes = new List<TimeSpan>();
            for(int i = 0; i < WarmUpTimes; i++)
            {
                await TinyProfiler.ProfileAsync($"executing warm-up query the {i + 1}th time",
                    () => dal.QueryDynamicAsync(query),
                    _messageProcessor);
                await Task.Delay(ExecutionInterval);
            }

            for(int i = 0; i < ExecutionTimes; i++)
            {
                await TinyProfiler.ProfileAsync($"executing query the {i + 1}th time", 
                    () => dal.QueryDynamicAsync(query), 
                    _messageProcessor, 
                    ProcessElapsedTime);
                if (i < ExecutionTimes - 1)
                { 
                    await Task.Delay(ExecutionInterval);
                }
            }

            var totalDuration = elapsedTimes.Aggregate(TimeSpan.Zero, (current, next) => current + next);
            var averageDurationInTicks = totalDuration.Ticks / ExecutionTimes;
            var averageDuration = new TimeSpan(averageDurationInTicks);
            var formattedAverageDuration = TinyProfiler.FormatTimeSpan(averageDuration);
            _messageProcessor($"The query on average takes {formattedAverageDuration} to run");

            void ProcessElapsedTime(TimeSpan time) => elapsedTimes.Add(time);
        }

        protected void LogSqlInfo(SqlInfo sqlInfo)
        {
            _messageProcessor($"The sql is {sqlInfo.Sql}");
            foreach (var (key, value) in sqlInfo.Parameters)
            {
                _messageProcessor($"The parameter name is {key}, value is {value}");
            }
        }

        protected DapperConnection CreateDapperConnection() => new DapperConnection(ConnectionString, new SqlServerDbProvider());

        protected (BaseDal dal, Query query) GetDalAndQuery(string tableName = null)
        {
            var dal = new BaseDal(tableName?? TableName, CreateDapperConnection(), LogSqlInfo);
            return (dal, dal.NewQuery());
        }

        protected async Task MeasureWithDalAndQuery(Func<BaseDal, Query, Query> queryBuilder = null, int? commandTimeout = null, string tableName = null)
        {
            tableName ??= TableName;
            var (dal, query) = GetDalAndQuery(tableName);
            var newQuery = queryBuilder?.Invoke(dal, query)?? query;
            await Measure(dal, newQuery, commandTimeout);
        }

        protected async Task MeasureWithQuery(Func<Query, Query> queryBuilder = null, int? commandTimeout = null, string tableName = null)
        {
            await MeasureWithDalAndQuery((dal, query) => queryBuilder?.Invoke(query), commandTimeout, tableName);
        }
    }
}
