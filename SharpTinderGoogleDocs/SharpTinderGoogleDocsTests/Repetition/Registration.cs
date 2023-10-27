using SharpConfigProg.ConfigPreparer;
using SharpContainerProg.Public;
using OutBorder1 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder2 = SharpConfigProg.Repetition.OutBorder;
using OutBorder3 = SharpRepoServiceProg.Repetition.OutBorder;

namespace SharpTinderGoogleDocsTests.Repetition
{
    internal class Registration : RegistrationBase
    {
        protected override void Registrations()
        {
            var fileService = OutBorder1.FileService();
            RegisterByFunc(() => fileService);

            var configService = OutBorder2.ConfigService(fileService);

            var repoService = OutBorder3.RepoService(fileService);
            RegisterByFunc(() => repoService);

            var preparer = container.Resolve<IPreparer>();
            configService.Prepare(preparer);
            repoService.Initialize(configService.GetRepoSearchPaths());
        }
    }
}
