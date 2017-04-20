﻿using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.Repositories
{
    public class PaymentRepositoryTests : IDisposable
    {
        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public PaymentRepositoryTests()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.Migrate();
            }
        }

        /// <summary>
        ///     Cleanup logic who is executed after executign every test.
        /// </summary>
        public void Dispose()
        {
            var path = Path.Combine(AppContext.BaseDirectory, DatabaseConstants.DB_NAME);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async void Add_AddedAndRead()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Note, loadedEntry.Note);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            // Act
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(3, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();
            testEntry.Id = 0;
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(2, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.NotNull(testEntry.Id);
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            var newValue = "newText";
            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            testEntry.Note = newValue;
            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Note);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            var idBeforeUpdate = testEntry.Id;
            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(idBeforeUpdate, testEntry.Id);
        }

        [Fact]
        public async void Update_NoNewEntryAdded()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);

            var testEntry = new PaymentEntity
            {
                Note = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            var testEntry = new PaymentEntity();
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Act
            repository.Delete(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(0, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            var testEntry = new PaymentEntity();

            // Act
            repository.Delete(testEntry);

            // Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await unitOfWork.Commit());
        }


        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var filterText = "Text";
            var repository = new PaymentRepository(factory);
            var testEntry1 = new PaymentEntity { Note = filterText };
            var testEntry2 = new PaymentEntity();
            repository.Add(testEntry1);
            repository.Add(testEntry2);
            await unitOfWork.Commit();

            // Act
            repository.Delete(x => x.Note == filterText);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var repository = new PaymentRepository(new DbFactory());

            // Act
            var emptyList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(emptyList);
            Assert.False(emptyList.Any());
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetMany(x => x.Note == "text").ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void GetMany_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            var filterText = "Text";
            repository.Add(new PaymentEntity { Note = filterText });
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetMany(x => x.Note == filterText).ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void Get_NothingMatched()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            await unitOfWork.Commit();

            // Act
            var result = await repository.Get(x => x.Note == "text");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new PaymentRepository(factory);
            var filterText = "Text";
            var testEntry = new PaymentEntity { Note = filterText };
            repository.Add(testEntry);
            repository.Add(new PaymentEntity());
            repository.Add(new PaymentEntity());
            await unitOfWork.Commit();

            // Act
            var result = await repository.Get(x => x.Note == filterText);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }
    }
}