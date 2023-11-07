using SharpCryptoCalcProg.Register;

namespace SharpCryptoCalcProg
{
    class Program
    {
        static void Main(string[] args)
        {
            var tmp = MyBorder.Container;
            var CryptoCalcService = new CryptoCalcService();
            CryptoCalcService.Sync();
        }
    }
}