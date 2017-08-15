using Stardust.Clusters.Clusters;
using Stardust.Clusters.Converters;
using Stardust.Clusters.EncodingCheckers;
using Stardust.Clusters.FileTransfer;
using Stardust.Clusters.TableParser;
using Stardust.Clusters.TabularMapper;
using Stardust.Nucleus;
using Stardust.Particles;

namespace Stardust.Clusters
{
    public static class ConfigurationExtensions
    {
        private static IConfigurator BindBinaryConverterFactory(this IConfigurator configuration)
        {
            configuration.Bind<IBinaryConverter>().To<HexConverter>(ConverterTypes.Hex).SetSingletonScope().DisableOverride();
            configuration.Bind<IBinaryConverter>().To<BinaryUtf8Converter>(ConverterTypes.BinaryUtf8).SetSingletonScope().DisableOverride();
            configuration.Bind<IBinaryConverter>().To<BinaryUnicodeConverter>(ConverterTypes.BinaryUnicode).SetSingletonScope().DisableOverride();
            
            return configuration;
        }

        public static IConfigurator BindFactories(this IConfigurator configuration)
        {
            BindEncodingFactory(configuration);
            BindParserFactory(configuration);
            BindTransferFactory(configuration);
            BindMiscStuff(configuration);
            BindBinaryConverterFactory(configuration);
            return configuration;

        }



        private static void BindMiscStuff(IConfigurator resolver)
        {
            resolver.Bind<ITabularMapper>().To<TabularMapper.TabularMapper>().SetSingletonScope().AllowOverride = false;
        }

        private static void BindTransferFactory(IConfigurator resolver)
        {
            resolver.Bind<IFileTransfer>().To<FileTransfer.FileTransfer>(TransferMethods.File).SetTransientScope().DisableOverride();
            resolver.Bind<IFileTransfer>().To<HttpFileTrasfer>(TransferMethods.Http).SetTransientScope().DisableOverride();
            resolver.Bind<IFileTransfer>().To<FtpTrasfer>(TransferMethods.Ftp).SetTransientScope().DisableOverride();
            resolver.Bind<IFileTransfer>().To<FileTransfer.FileTransfer>().SetTransientScope().DisableOverride();
        }

        private static void BindParserFactory(IConfigurator resolver)
        {
            resolver.Bind<ITableParser>().To<XmlTableParser>(Parsers.SimpleXmlParser).SetTransientScope().DisableOverride();
            resolver.Bind<ITableParser>().To<CsvTableParser>(Parsers.Delimitered).SetTransientScope().DisableOverride(); ;
            resolver.Bind<ITableParser>().To<XlsTableParser>(Parsers.OldExcel).SetTransientScope().DisableOverride(); ;
            resolver.Bind<ITableParser>().To<XlsxTableParser>(Parsers.Excel).SetTransientScope().DisableOverride(); ;
        }

        private static void BindEncodingFactory(IConfigurator resolver)
        {
            resolver.Bind<IEncodingChecker>().To<UnicodeBigendianChecker>("1").SetSingletonScope().DisableOverride();
            resolver.Bind<IEncodingChecker>().To<UnicodeChecker>("2").SetSingletonScope().DisableOverride();
            resolver.Bind<IEncodingChecker>().To<Utf32Checker>("3").SetSingletonScope().DisableOverride();
            resolver.Bind<IEncodingChecker>().To<Utf8Checker>("4").SetSingletonScope().DisableOverride();
            resolver.Bind<IEncodingChecker>().To<Utf7Checker>("5").SetSingletonScope().DisableOverride();
        }
    }
}
