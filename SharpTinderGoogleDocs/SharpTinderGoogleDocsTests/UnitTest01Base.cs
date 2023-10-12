using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpTinderComplexTests.Repetition;
using Unity;

namespace SharpTinderComplexTests
{
    public class UnitTest01Base
    {
        protected readonly string configFilePath;
        private readonly IFileService fileService;
        protected readonly IFileService.IYamlOperations yamlWorker;
        protected readonly IConfigService configService;
        protected readonly IRepoService repoService;

        protected UnitTest01Base()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            yamlWorker = fileService.Yaml.Custom03;
            configService = MyBorder.Container.Resolve<IConfigService>();
            configFilePath = configService.ConfigFilePath;
        }
    }
}