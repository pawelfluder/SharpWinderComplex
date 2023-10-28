using SharpConfigProg.Service;
using SharpContainerProg.Public;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using Registration01 = SharpSetupProg21Private.APublic.Registration;

namespace CSharpSheetToObjProg.Repet
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            new Registration01().Start();
        }
    }
}
