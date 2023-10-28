using SharpContainerProg.Public;
using Registration01 = SharpSetupProg21Private.APublic.Registration01;

namespace CSharpGameSynchProg.Register
{
    internal class Registration : RegistrationBase
    {
        private Registration01 r01;

        public Registration()
        {
            r01 = new Registration01();
        }

        public override void Registrations()
        {
            r01.Registrations();
        }
    }
}
