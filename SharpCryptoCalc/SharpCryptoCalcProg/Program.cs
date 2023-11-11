using SharpCryptoCalcProg.Register;
using SharpCryptoCalcProg.Service;

namespace SharpCryptoCalcProg
{
    class Program
    {
        static void Main(string[] args)
        {
            var tmp = MyBorder.Container;
            var cryptoCalcService = new CryptoCalcService();
            cryptoCalcService.Sync();
        }
    }
}