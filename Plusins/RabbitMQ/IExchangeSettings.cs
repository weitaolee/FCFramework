using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FC.Framework.Utilities;
using RabbitMQ.Client;

namespace FC.Framework.RabbitMQ
{
    public interface IExchangeSettings
    {
        Dictionary<string, int> CoinPairExchanges { get; }
        string RabbitConnectionString { get; }
        Func<ICommand, Dictionary<string, int>, string> MatchExchange { get; }

        void Watch();
        void WatchStop();
    }
}
