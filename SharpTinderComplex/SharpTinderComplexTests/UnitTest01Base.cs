using FileServiceCoreApp;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using SharpTinderComplexTests.Repetition;
using Unity;
using OutBorder01 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder02 = SharpConfigProg.Repetition.OutBorder;

namespace SharpTinderComplexTests
{
    public class UnitTest01Base
    {
        protected readonly IFileService.IYamlOperations yamlWorker;
        protected readonly IRepoService repoService;
        private readonly IFileService fileService;
        protected readonly IConfigService configService;

        protected readonly string configFilePath;
        protected Dictionary<string, object> ConfigData { get; private set; }

        protected UnitTest01Base()
        {
            fileService = MyBorder.Container.Resolve<IFileService>();
            configService = MyBorder.Container.Resolve<IConfigService>();
            configFilePath = configService.ConfigFilePath;
            yamlWorker = fileService.Yaml.Custom03;
            repoService = MyBorder.Container.Resolve<IRepoService>();
        }

        protected void CreateWorkspace(string path)
        {
            var pathsList = new List<string>() { path };
            var xmlWorker = new XmlWorker(fileService);
            var path2 = path + "/" + "workspace.txt";
            xmlWorker.Load(path2);
            //xmlDocument.Save();
        }
    }
}