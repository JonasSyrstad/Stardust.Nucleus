using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stardust.Clusters.Clusters;
using Stardust.Clusters.TableParser;
using Stardust.Clusters.TabularMapper;
using Stardust.Core.CrossCuttingTest.LegacyTests.Mock;
using Stardust.Nucleus;
using Stardust.Nucleus.TypeResolver;

namespace Stardust.Core.CrossCuttingTest.LegacyTests
{
    [TestClass]
    public class TabularMapperTests
    {
        private const string TestFilePath = "TabularMappingTestFile.txt";
        private IKernelContext KernelScope;

        [TestInitialize]
        public void Initialize()
        {
            KernelScope = Resolver.BeginKernelScope();
            Resolver.LoadModuleConfiguration<Blueprint>();

        }

        [TestCleanup]
        public void Cleanup()
        {
            Resolver.EndKernelScope(KernelScope, true);
        }

        
        [TestMethod()]
        [TestCategory("Tabular Mapper")]
        [DeploymentItem("TestFiles/TabularMappingTestFile.txt")]
        public void ConvertSimpleStringType()
        {
            var data = Parsers.Delimitered.GetParser().SetHeaderRow(false)
                .Parse(TestFilePath)
                .ConvertTo<StringClass>(new MappingDefinition
                {
                    Fields =
                        new[]
                        {
                            new FieldMapping
                            {
                                SourceColumnNumber = 0,
                                MemberType = MemberTypes.Property,
                                TargetMemberName = "header1"
                            },
                            new FieldMapping
                            {
                                SourceColumnNumber = 1,
                                MemberType = MemberTypes.Property,
                                TargetMemberName = "header2"
                            },
                            new FieldMapping
                            {
                                SourceColumnNumber = 2,
                                MemberType = MemberTypes.Method,
                                TargetMemberName = "header3"
                            },
                            new FieldMapping
                            {
                                SourceColumnNumber = 3,
                                MemberType = MemberTypes.Field,
                                TargetMemberName = "header4"
                            },
                        }
                });
            Assert.IsTrue(data.Any());
        }

        [TestMethod()]
        [TestCategory("Tabular Mapper")]
        [DeploymentItem("TestFiles/TabularMappingTestFile.txt")]
        public void ConvertSimpleTypedType()
        {
            var source = Parsers.Delimitered.GetParser().SetHeaderRow(true).Parse(TestFilePath);
            var data = source.ConvertTo<TypedClass>(new MappingDefinition
                {
                    Fields =
                        new[]
                        {
                            new FieldMapping
                            {
                                SourceColumnName = "header1",
                                MemberType = MemberTypes.Property,
                                TargetMemberName = "header1"
                            },
                            new FieldMapping
                            {
                                SourceColumnName = "header2",
                                MemberType = MemberTypes.Property,
                                TargetMemberName = "header2"
                            },
                            new FieldMapping
                            {
                                SourceColumnName = "header3",
                                MemberType = MemberTypes.Property,
                                TargetMemberName = "header3"
                            },
                            new FieldMapping
                            {
                                SourceColumnName = "header4",
                                MemberType = MemberTypes.Property,
                                TargetMemberName = "header4"
                            },
                        }
                });
            Assert.AreEqual(data.First().header1.ToString(), ((dynamic)source.First()).header1);
            Assert.IsTrue(data.Any());
        }

        [TestMethod()]
        [TestCategory("Tabular Mapper")]
        [DeploymentItem("TestFiles/TabularMappingTestFile.txt")]
        public void ConvertSimpleTypedTypeWithAutoMap()
        {
            var doc = Parsers.Delimitered.GetParser().SetHeaderRow(true)
                .Parse(TestFilePath);
            var data = doc.ConvertTo<TypedClass>(MappingDefinition.CreateMappingDefinition().AddMappingFromDocumentWithHeaders(doc));
            Assert.IsTrue(data.Any());
        }
    }

    
}