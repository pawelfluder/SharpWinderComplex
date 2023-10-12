namespace TinderImport
{
    using YamlDotNetSerializer = YamlDotNet.Serialization.Serializer;
    using SharpYamlSerializer = SharpYaml.Serialization.Serializer;

    public class YamlWorker
    {
        private YamlDotNetSerializer yamlSerializerDotNet;
        private SharpYamlSerializer yamlSerializerSharp;

        public YamlWorker()
        {
            yamlSerializerDotNet = new YamlDotNetSerializer();
            yamlSerializerSharp = new SharpYamlSerializer();
        }

        public string Serialize(object input)
        {
            var result = yamlSerializerDotNet.Serialize(input);
            return result;
        }

        public object Deserialize(string path)
        {
            var yamlText = File.ReadAllText(path);
            var result = yamlSerializerSharp.Deserialize<object>(yamlText);
            return result;
        }
    }
}
