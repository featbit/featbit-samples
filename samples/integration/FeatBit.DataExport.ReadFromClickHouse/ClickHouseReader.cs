using ClickHouse.Client.ADO;
using Dapper;

namespace FeatBit.DataExport.ClickHouse
{
    public class ClickHouseReader
    {
        public static async Task<List<FlagValueEvent>> RetrieveFlagValueEventsAsync(ParamOptions options)
        {
            using (var connection = new ClickHouseConnection(options.ConnectionString))
            {
                string sql = "SELECT " +
                             "  uuid as Id, " +
                             "  env_id as EnvId, " +
                             "  distinct_id as DistinctId, " +
                             "  event as EventName, " +
                             "  timestamp as Timestamp, " +
                             "  _timestamp as CHTimestamp, " +
                             //"  properties as Properties, " +
                             "  JSONExtract(properties, 'route', 'String') as Route, " +
                             "  JSONExtract(properties, 'flagId', 'String') as FeatureFlagId, " +
                             "  JSONExtract(properties, 'accountId', 'String') as AccountId, " +
                             "  JSONExtract(properties, 'projectId', 'String') as ProjectId, " +
                             "  JSONExtract(properties, 'featureFlagKey', 'String') as FeatureFlagKey, " +
                             "  JSONExtract(properties, 'sendToExperiment', 'Bool') as SendToExperiment, " +
                             "  JSONExtract(properties, 'userKeyId', 'String') as UserId, " +
                             "  JSONExtract(properties, 'userName', 'String') as Username, " +
                             "  JSONExtract(properties, 'variationId', 'String') as VariationId, " +
                             "  JSONExtract(properties, 'tag_0', 'String') as Tag0, " +
                             "  JSONExtract(properties, 'tag_1', 'String') as Tag1, " +
                             "  JSONExtract(properties, 'tag_2', 'String') as Tag2, " +
                             "  JSONExtract(properties, 'tag_3', 'String') as Tag3 " +
                             "FROM events " +
                             "WHERE " +
                            $"  timestamp > '{options.TimeStamp}' AND " +
                            $"  event = 'FlagValue' AND " +
                            $"  env_id = '{options.EnvId}' " +
                             "ORDER BY timestamp " +
                            $"LIMIT {options.PageSize} ";
                return (await connection.QueryAsync<FlagValueEvent>(sql)).ToList();
            }
        }
    }
}
