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
    internal class ProductDBTests
    {
        ProductDB db;
        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve("A4CS");
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
        }
        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }
        [Test]
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve("A4CS");
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve("A4CS"));
        }
        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            ProductProps p = (ProductProps)db.Retrieve("A4CS");
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }
        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve("A4CS");
            p.Description = "Very descriptive description";
            p.UnitPrice = 0.01m;
            p.OnHandQuantity = 10;
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve("A4CS");
            Assert.AreEqual("Very descriptive description", p.Description);
        }
        [Test]
        public void TestUpdateFieldTooLong()
        {
            ProductProps p = (ProductProps)db.Retrieve("A4CS");
            p.Description = "Oregon is the state where Crater Lake National Park is.";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }
        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "??";
            p.Description = "Where am I";
            p.UnitPrice = 0.0100m;
            p.OnHandQuantity = 10;
            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductCode);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }
        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "A4CS";
            p.Description = "Murach's ASP.NET 4 Web Programming with C# 2010";
            p.UnitPrice = 56.5000m;
            p.OnHandQuantity = 4637;
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
    }
}
