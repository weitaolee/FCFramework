using FC.Framework.Utilities;


namespace FC.Framework.Repository
{
    public class ConnectionString
    {
        private string _connectionString;
        private string _providerName;

        public ConnectionString(string connectionStr, string providerName)
        {
            Check.Argument.IsNotEmpty(connectionStr, "connectionStr");
            Check.Argument.IsNotEmpty(providerName, "providerName");

            this._connectionString = connectionStr;
            this._providerName = providerName;
        }

        public string Value
        {
            get { return this._connectionString; }
        }

        public string ProviderName
        {
            get { return this._providerName; }
        }
    }
}
