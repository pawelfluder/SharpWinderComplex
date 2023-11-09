namespace SharpCryptoCalcProg.ASheetObjects
{
    internal class BinanceConvert
    {
        public BinanceConvert()
        {
        }
        // Id Date Wallet Pair Type Sell Buy Price InversePrice DateUpdated Status
        public BinanceConvert(
            string id,
            string date,
            string wallet,
            string pair,
            string type2,
            string sell,
            string buy,
            string price,
            string inversePrice,
            string dateUpdated,
            string status)
        {
            Id = id;
            Date = date;
            Wallet = wallet;
            Pair = pair;
            Type2 = type2;
            Sell = sell;
            Buy = buy;
            Price = price;
            InversePrice = inversePrice;
            DateUpdated = dateUpdated;
            Status = status;
        }

        public string Id { get; set; }

        public string Date { get; set; }

        public string Wallet { get; set; }

        public string Pair { get; set; }

        public string Type2 { get; set; }

        public string Sell { get; set; }

        public string Buy { get; set; }

        public string Price { get; set; }

        public string InversePrice { get; set; }

        public string DateUpdated { get; set; }

        public string Status { get; set; }
    }
}
