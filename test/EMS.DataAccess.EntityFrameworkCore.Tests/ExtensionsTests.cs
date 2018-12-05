using System;
using EMS.DataAccess.EntityFrameworkCore.Tests.Contexts;
using EMS.DataAccess.EntityFrameworkCore.Tests.Helpers;
using EMS.DataAccess.EntityFrameworkCore.Tests.Models;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace EMS.DataAccess.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        private SqliteConnection _dstConnection;

        [SetUp]
        public void Setup()
        {
            DbConnectionHelpers.SeedContext<DestinationDbContext>(out _dstConnection);
        }

        [TearDown]
        public void TearDown()
        {
            _dstConnection.Close();
        }

        [Test]
        public void InexistentEntity_Exists_ReturnsFalse()
        {
            var dbContext = new DestinationDbContext(_dstConnection);
            {
                var dstEntity = new DestinationUser
                {
                    EmailAddress = "pesho@pesho.org",
                    SamAccountName = "pesho"
                };

                Assert.That(dbContext.EntityExists(dstEntity), Is.False);
            }
        }

        [Test]
        public void FakeEntity_Exists_ReturnsFalse()
        {
            var dbContext = new DestinationDbContext(_dstConnection);
            {
                var dstEntity = new DestinationUser
                {
                    Id = Guid.NewGuid(),
                    EmailAddress = "pesho@pesho.org",
                    SamAccountName = "pesho"
                };

                Assert.That(dbContext.EntityExists(dstEntity), Is.False);
            }
        }


        [Test]
        public void RealEntity_Exists_ReturnsTrue()
        {
            var dbContext = new DestinationDbContext(_dstConnection);
            {
                var dstEntity = new DestinationUser
                {
                    EmailAddress = "pesho@pesho.org",
                    SamAccountName = "pesho"
                };

                dbContext.Add(dstEntity);
                dbContext.SaveChanges();

                Assert.That(dbContext.EntityExists(dstEntity), Is.True);
            }
        }
    }
}