using System;
using System.Data;
using System.Data.SqlClient;
using Akka.Actor;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class StoreMetricsActor : ReceiveActor
    {
        public StoreMetricsActor()
        {
            Receive<StoreMetricsRequestMessage>(async r =>
            {
                try
                {
                    using (var connection = new SqlConnection(@"Data Source=(localdb)\v11.0;Initial Catalog=FoodClustering;Integrated Security=SSPI"))
                    using (var cmd = connection.CreateCommand())
                    {
                        await connection.OpenAsync();
                        var parameters = cmd.Parameters;
                        var foodTermsParam = parameters.Add("@FoodName", SqlDbType.NVarChar, 1000);
                        var documentUriParam = parameters.Add("@DocumentUri", SqlDbType.NVarChar, 1000);
                        var comparisonNameParam = parameters.Add(@"ComparisonFoodName", SqlDbType.NVarChar, 1000);
                        var normParam = parameters.Add("@Norm", SqlDbType.Float);
                        foodTermsParam.Value = r.Comparison.SourceScore.FoodNameTerms.FoodName;
                        documentUriParam.Value = r.Comparison.SourceScore.Document.DocumentUri.ToString();
                        comparisonNameParam.Value = r.Comparison.TargetScore.FoodNameTerms.FoodName;
                        normParam.Value = r.Comparison.DifferenceNorm;
                        cmd.CommandText =
                            "insert metrics (foodname, documenturi, comparisonfoodname, norm) values (@FoodName, @DocumentUri, @ComparisonFoodName, @Norm)";
                        await cmd.ExecuteNonQueryAsync();
                    }
                    Sender.Tell(new StoreMetricsResultMessage(r));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new StoreMetricsFailedMessage(r, exp));
                }
            });
        }
    }
}