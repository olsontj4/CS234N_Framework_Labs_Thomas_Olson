using NUnit.Framework;

using MMABooksBusiness;
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
    internal class CustomerTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no code
            Customer c = new Customer();
            Assert.AreEqual(string.Empty, c.Name);
            Assert.AreEqual(string.Empty, c.Address);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.Name = "Ricky House";
            c.Address = "102.3 FM";
            c.City = "New Vegas";
            c.State = "MI";
            c.ZipCode = "12345";
            c.Save();
            Customer s2 = new Customer(c.CustomerID);
            Assert.AreEqual(s2.Name, c.Name);
            Assert.AreEqual(s2.Address, c.Address);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer(1);
            c.Name = "Vicky Strauss";
            c.Address = "99.1 FM";
            c.City = "New York City";
            c.State = "NY";
            c.ZipCode = "54321";
            c.Save();
            Customer s2 = new Customer(1);
            Assert.AreEqual(s2.Name, c.Name);
            Assert.AreEqual(s2.Address, c.Address);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(1);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(1));
        }

        [Test]
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> Customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, Customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", Customers[0].Name);
            Assert.AreEqual("1414 S. Dairy Ashford", Customers[0].Address);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "??";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1);
            Customer c2 = new Customer(1);

            c1.Name = "Updated first";
            c1.Address = "1";
            c1.City = "1";
            c1.State = "NY";
            c1.ZipCode = "1";
            c1.Save();

            c2.Name = "Updated second";
            c2.Address = "2";
            c2.City = "2";
            c2.State = "NY";
            c2.ZipCode = "2";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}
