using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Client;
using GraphQL.Common;


namespace Centrotrans.StationAPI
{
    class GetStations
    {
        public async static System.Threading.Tasks.Task<dynamic> post()
        {
            var request = new GraphQL.Common.Request.GraphQLRequest
            {
                Query = @"
                query GetStationsOperation($pagination: Input_com_busfive_citypass_api_graphql_Pagination, $orderBy: [Input_com_busfive_citypass_api_graphql_OrderBy], $filter: String){
                    GetStations(pagination: $pagination, orderBy: $orderBy, filter: $filter) {
                        stationId
                        name
                        borderCrossing
                    }        
                }
            ",
                Variables = new
                {
                    pagination = new
                    {
                        page = 1,
                        itemsPerPage = 10000
                    }
                }
            };

            var client = new GraphQLClient("http://149.202.105.188:8160/api/transport_data");
            client.DefaultRequestHeaders.Add("BusfiveAuthorization", "VROsNbCJOiZ7dKFbVCJdzonxPg6v52gwWhwvEiQpigQigAYi.fq0Kt4CHdoXJe4t");

            var val = await client.PostAsync(request);

            return val.Data.GetStations;
        }
    }
}
