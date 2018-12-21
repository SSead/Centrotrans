using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Client;
using GraphQL.Common;

namespace Centrotrans.StationAPI
{
    class GetLinesDrivingThroughStations
    {
        public async static System.Threading.Tasks.Task<dynamic> post(int stationBegin, int stationEnd, DateTime departureDate, DateTime returnDate)
        {
            var request = new GraphQL.Common.Request.GraphQLRequest
            {
                Query = @"
                    query GetLinesDrivingThroughStationsOperation(
                        $filter: String,
                        $page: Int,
                        $itemsPerPage: Int,
                        $orderBy: [Input_com_busfive_citypass_api_graphql_OrderBy!],
                        $fromStationId: Int!,
                        $toStationId: Int!,
                        $cardTypeId: Int!,
                        $departureDate: String,
                        $returnDate: String,
                        $currencyId: Int!
                ) {
                        GetLinesDrivingThroughStations(
                            filter: $filter,
                            orderBy: $orderBy,
                            departureDate: $departureDate,
                            returnDate: $returnDate,
                            currencyId: $currencyId,
                            pagination: {
                                itemsPerPage: $itemsPerPage
                                page: $page
                            }, fromStationId: $fromStationId,
                                toStationId: $toStationId,
                                cardTypeId: $cardTypeId) {
                                        lineId
                                        currency
                                        linePrice
                                        lineCarrierKind {
                                            kindId
                                            name
                                            shortName
                                        }
                                        subLinePrice
                                        subLineStationFrom
                                        subLineStationTo
                                        partner {
                                            id
                                            name
                                        }
                                        timeTables {
                                            departTime
                                            directionId
                                            entries {
                                                borderCrossing
                                                distanceToFirstStationKilometers
                                                station {
                                                    name
                                                    stationId
                                                }
                                                timeSeconds
                                            }
                                            lineId
                                            timeTableId
                                            maxPassengerCount
                                        }
                                }
                    }
                ",
                Variables = new
                {
                    cardTypeId = (returnDate == DateTime.Today.AddDays(-1) ? 1 : 2),
                    fromStationId = stationBegin,
                    toStationId = stationEnd,
                    departureDate = departureDate.ToString("yyyy-MM-ddTHH:mm:ss.000Z"),
                    returnDate = returnDate.ToString("yyyy-MM-ddTHH:mm:ss.0000Z"),
                    currencyId = 1,
                    page = 1,
                    itemsPerPage = 10000
                }
            };

            var client = new GraphQLClient("http://149.202.105.188:8160/api/transport_data");
            client.DefaultRequestHeaders.Add("BusfiveAuthorization", "VROsNbCJOiZ7dKFbVCJdzonxPg6v52gwWhwvEiQpigQigAYi.fq0Kt4CHdoXJe4t");

            var val = await client.PostAsync(request);

            return val.Data.GetLinesDrivingThroughStations;
        }
    }
}