using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Client;
using GraphQL.Common;

namespace Centrotrans.StationAPI
{
    class GetPossibleDestinations
    {
        public async static System.Threading.Tasks.Task<dynamic> post(int stationID)
        {
            var request = new GraphQL.Common.Request.GraphQLRequest
            {
                Query = @"
                    query GetPossibleDestinationsOperation(
                        $cardTypeId: Int!,
                        $startStationId: Int!,
                        $pagination: Input_com_busfive_citypass_api_graphql_Pagination, 
                        $orderBy: [Input_com_busfive_citypass_api_graphql_OrderBy], 
                        $filter: String
                    ){
                        GetPossibleDestinations(cardTypeId: $cardTypeId, startStationId: $startStationId, pagination: $pagination, orderBy: $orderBy, filter: $filter) {        
                            stationId
                            name
                            borderCrossing
                        }        
                    }
                ",
                Variables = new
                {
                    cardTypeId = 1,
                    startStationId = stationID
                }
            };

            var client = new GraphQLClient("http://149.202.105.188:8160/api/transport_data");
            client.DefaultRequestHeaders.Add("BusfiveAuthorization", "VROsNbCJOiZ7dKFbVCJdzonxPg6v52gwWhwvEiQpigQigAYi.fq0Kt4CHdoXJe4t");

            var val = await client.PostAsync(request);

            return val.Data.GetPossibleDestinations;
        }
    }
}