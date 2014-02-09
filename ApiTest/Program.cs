using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BtcE;

namespace ApiTest
{
	class Program
	{
        public static string apiKey;
        public static string apiSecret;
        public static int apiTick = BtceApi.apiTick;
        public static Stack refreshStack = new Stack();

		static void Main(string[] args) {
            var depth3 = BtceApiV3.GetDepth(new BtcePair[] { BtcePair.btc_usd });
            var ticker3 = BtceApiV3.GetTicker(new BtcePair[] { BtcePair.btc_usd });
            var trades3 = BtceApiV3.GetTrades(new BtcePair[] { BtcePair.btc_usd });
			var ticker = BtceApi.GetTicker(BtcePair.btc_usd);
			var trades = BtceApi.GetTrades(BtcePair.btc_usd);
			var btcusdDepth = BtceApi.GetDepth(BtcePair.usd_rur);
			var fee = BtceApi.GetFee(BtcePair.usd_rur);

            if (apiTick == 1)
            {
                apiKey = "3CDPFMIW-XQM8SQ9S-DKT32FZ1-0VOGI1YW-YQELC1Z7";
                apiSecret = "598c09236ca49e737b68c252e413d035d3a1c64a39f0c19078ede95ccf218a8b";
            }
            else if (apiTick == 2)
            {
                apiKey = "EJXDS1KQ-KIHF2XBQ-Z1B7U8P6-JXL7LTQP-P4ATQ3FC";
                apiSecret = "35ae42ffa6942a06724d03b4324a6ce628e8958f4e4bd173803863cc72718bed";
            }
            else if (apiTick == 3)
            {
                apiTick = 1;
            }

			var btceApi = new BtceApi(apiKey, apiSecret);
			var info = btceApi.GetInfo();
			var transHistory = btceApi.GetTransHistory();
			var tradeHistory = btceApi.GetTradeHistory(count: 20);
			var orderList = btceApi.GetOrderList();
			var tradeAnswer = btceApi.Trade(BtcePair.btc_usd, TradeType.Sell, 20, 0.1m);
			var cancelAnswer = btceApi.CancelOrder(tradeAnswer.OrderId);

            refreshStack.Push(refresh(btceApi, 0));
		}

        static object refresh(BtceApi btceApi, int counter)
        {


            //Multi threading fix for stack overflow errors/api key timeout.
            counter += 1;
            if (counter > 10)
            {
                refreshStack.Clear();
                counter = 0;
            }
            apiTick = BtceApi.apiTick;
            refreshStack.Push(refresh(btceApi, counter));
            return counter;
        }
	}
}
