using System;
using ArchestrA.Configuration;
using ArchestrA.Configuration.Internal;
using ArchestrA.Core;
using ArchestrA.GRAccess;
using ArchestrA.IDE;
using ArchestrA.Security;
using FluentAssertions;
using GServer.Archestra.IntegrationTests;
using NUnit.Framework;
using FolderType = ArchestrA.Core.FolderType;

namespace GServer.Archestra.LibraryTests
{
    [TestFixture]
    public class PackageManagerTests
    {
        private static IGalaxy _galaxy;
        private GRAccessAppClass _grAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _grAccess = new GRAccessAppClass();
            _galaxy = _grAccess.QueryGalaxies()["Test"];
            _galaxy.Login(string.Empty, string.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
            _galaxy = null;
        }

        [Test]
        public void GetFolderTree_WhenCalled_ShouldNotBeNull()
        {
            var config = _galaxy.GetGalaxyConfiguration();

            if (config is not IGalaxyConfiguration galaxyConfiguration)
                throw new InvalidOperationException();

            var tree = galaxyConfiguration.GetFolderTree(FolderType.Visual, out var status, out var message);

            
            tree.Should().NotBeNull();
            status.Should().Be(ERRORCODE.eNoError);
            message.Should().Be("");
        }
        
        [Test]
        public void GetFolderTreeData()
        {
            var config = _galaxy.GetGalaxyConfiguration();

            if (config is not IGalaxyConfiguration galaxyConfiguration)
                throw new InvalidOperationException();

            if (config is not IPackageManager packageManager)
                throw new InvalidOperationException();
            
            
            using var operationStatusCallback = new GObjectOperationStatusCallback();
            using var packageManagerCallback = new PackageManagerCallback();
            
            /*galaxyConfiguration.CreateObjectInFolder(id, EACTION.eNewInstance, 0, "TestGraphic",
                        ArchestrA.Core.E_RESOLVE_NAME_CONFLICT_ACTION.eResolveNameConflictAction_SkipObject,
                        ArchestrA.Core.E_RESOLVE_VERSION_CONFLICT_ACTION.eResolveVersionConflictAction_AbortImport,
                        string.Empty, string.Empty, DateTime.Now.Millisecond,
                        packageManagerCallback,
                        operationStatusCallback);*/

            galaxyConfiguration.AddToolset("TestToolset", out var result);

            packageManagerCallback.WaitOnOperation();

            if (operationStatusCallback.TotalOperationSuccess)
                Assert.Pass();
            
            Assert.Fail();
        }

        [Test]
        public void Sample()
        {
            if (_galaxy.GetGalaxyConfiguration() is not IGalaxyConfiguration galaxyConfiguration)
                return;

            if (_galaxy.GetGalaxyConfiguration() is not IPackageManager packageManager)
                return;

            var id = packageManager.GetIDFromName2(ArchestrA.Core.Namespace.VisualElement, "$Symbol");

            using (var operationStatusCallback = new GObjectOperationStatusCallback())
            {
                using (var packageManagerCallback = new PackageManagerCallback())
                {
                    galaxyConfiguration.CreateObjectInFolder(id, EACTION.eNewInstance, 0, "TestGraphic",
                        ArchestrA.Core.E_RESOLVE_NAME_CONFLICT_ACTION.eResolveNameConflictAction_SkipObject,
                        ArchestrA.Core.E_RESOLVE_VERSION_CONFLICT_ACTION.eResolveVersionConflictAction_AbortImport,
                        string.Empty, string.Empty, DateTime.Now.Millisecond,
                        packageManagerCallback,
                        operationStatusCallback);

                    packageManagerCallback.WaitOnOperation();

                    if (operationStatusCallback.TotalOperationSuccess)
                        Assert.Pass();
                }
            }

            Assert.Fail();
        }
    }
}