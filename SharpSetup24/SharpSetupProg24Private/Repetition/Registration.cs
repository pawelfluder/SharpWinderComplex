using SharpConfigProg.ConfigPreparer;
using SharpContainerProg.Public;

namespace SharpSetupProg24Private.Repetition
{
    public class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            var preparer = new WinderPreparer();
            RegisterByFunc<IPreparer>(() => preparer);
        }
    }
}
