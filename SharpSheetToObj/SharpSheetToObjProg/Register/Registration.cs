using SharpContainerProg.AAPublic;
using Registration01 = SharpSetupProg21Private.AAPublic.Registration;

namespace CSharpGameSynchProg.Register
{
    internal class Registration : RegistrationBase
    {
        public override void Registrations()
        {
            new Registration01().Registrations();
        }
    }
}
