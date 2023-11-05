using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpTinderComplexTests;
using Unity;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder2 = SharpConfigProg.AAPublic.OutBorder;
using OutBorder3 = SharpRepoServiceProg.AAPublic.OutBorder;

namespace SharpNotesMigrationTests.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            RegisterByFunc(OutBorder1.FileService);

            RegisterByFunc(
                OutBorder2.ConfigService,
                container.Resolve<IFileService>());

            RegisterByFunc<IRepoService, IFileService>(OutBorder3.RepoService,
                container.Resolve<IFileService>());

            var configService = container.Resolve<IConfigService>();
            var preparer = new WinderPreparer();
            configService.Prepare(preparer);
            container.Resolve<IRepoService>()
                .Initialize(configService.GetRepoSearchPaths());
        }
    }
}
