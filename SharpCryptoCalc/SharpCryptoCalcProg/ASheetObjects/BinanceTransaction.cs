namespace SharpCryptoCalcProg.ASheetObjects
{
    internal class BinanceTransaction
    {

        public BinanceTransaction()
        {
        }

        // Id DateUTC Coin Network Amount TransactionFee Address Txid SourceAddress PaymentID Status
        public BinanceTransaction(
            string id,
            string date,
            string coin,
            string network,
            string amount,
            string transactionFee,
            string address,
            string txid,
            string sourceAddress,
            string paymentID,
            string status)
        {
            Id = id;
            Date = date;
            Coin = coin;
            Network = network;
            Amount = amount;
            TransactionFee = transactionFee;
            Address = address;
            Txid = txid;
            SourceAddress = sourceAddress;
            PaymentID = paymentID;
            Status = status;
        }

        public string Id { get; set; }

        public string Date { get; set; }

        public string Coin { get; set; }

        public string Network { get; set; }

        public string Amount { get; set; }

        public string TransactionFee { get; set; }

        public string Address { get; set; }

        public string Txid { get; set; }

        public string SourceAddress { get; set; }

        public string PaymentID { get; set; }

        public string Status { get; set; }
    }
}
