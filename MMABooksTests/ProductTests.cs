using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    internal class ProductTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestNewProductConstructor()
        {
            Product p = new Product();
            Assert.AreEqual(string.Empty, p.ProductCode);
            Assert.AreEqual(string.Empty, p.Description);
            Assert.IsTrue(p.IsNew);
            Assert.IsFalse(p.IsValid);
        }
        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            Product p = new Product("A4CS");
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.IsTrue(p.ProductCode.Length > 0);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }
        [Test]
        public void TestSaveToDataStore()
        {
            Product p = new Product();
            p.Description = "Stuff.";
            p.ProductCode = "F0UR";
            p.UnitPrice = 5.0000m;
            p.OnHandQuantity = 0; //Can't make this zero for some reason.
            p.Save();
            Product s2 = new Product("F0UR");
            Assert.AreEqual(s2.Description, p.Description);
            Assert.AreEqual(s2.ProductCode, p.ProductCode);
        }
        [Test]
        public void TestUpdate()
        {
            Product p = new Product("A4CS");
            p.Description = "Edited description.";
            p.Save();
            Product s2 = new Product("A4CS");
            Assert.AreEqual(s2.Description, p.Description);
            Assert.AreEqual(s2.ProductCode, p.ProductCode);
        }
        [Test]
        public void TestDelete()
        {
            Product p = new Product("A4CS");
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product("A4CS"));
        }
        [Test]
        public void TestGetList()
        {
            Product s = new Product();
            List<Product> products = (List<Product>)s.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("DB1R", products[0].ProductCode);
            Assert.AreEqual("DB2 for the COBOL Programmer, Part 1 (2nd Edition)", products[0].Description);
        }
        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }
        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "??";
            Assert.Throws<Exception>(() => p.Save());
        }
        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Description = "Loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong description.");
        }
        [Test]
        public void TestConcurrencyIssue()
        {
            Product c1 = new Product("A4CS");
            Product c2 = new Product("A4CS");

            c1.Description = "Updated first";

            c1.Save();

            c2.Description = "Updated second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}
