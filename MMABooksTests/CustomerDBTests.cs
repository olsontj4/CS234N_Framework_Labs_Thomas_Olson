using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    internal class CustomerDBTests
    {
        CustomerDB db;

        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Ricky House";
            p.Address = "102.3 FM";
            p.City = "New Vegas";
            p.State = "MI";
            p.ZipCode = "12345";
            db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerID);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }
        [Test]
        public void TestCreateMissingData()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "";
            p.Address = "";
            p.City = "";
            p.State = "";
            p.ZipCode = "";
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
        [Test]
        public void TestDelete()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }
        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }
        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, p.CustomerID);
            Assert.AreEqual("Molunguri, A", p.Name);
        }
        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }
        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "Vicky Strauss";
            p.Address = "99.1 FM";
            p.City = "New York City";
            p.State = "NY";
            p.ZipCode = "54321";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Vicky Strauss", p.Name);
        }
        [Test]
        public void TestUpdateFieldTooLong()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "Vicky Strauss";
            p.Address = "99.1 FM";
            p.City = "New York City";
            p.State = "NY";
            p.ZipCode = "543218888888888888888888";
            Assert.Throws<MySqlException>(() => db.Update(p));
            p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Molunguri, A", p.Name);
        }
    }
}