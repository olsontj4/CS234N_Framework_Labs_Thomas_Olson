using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MMABooksProps;

namespace MMABooksTests
{
    internal class ProductPropsTests
    {
        ProductProps props;
        [SetUp]
        public void Setup()
        {
            props = new ProductProps();
            props.ProductID = 0;
            props.ProductCode = "Ricky House";
            props.Description = "102.3 FM";
            props.UnitPrice = 0.0100m;
            props.OnHandQuantity = 10;
        }
        [Test]
        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.Description));
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
        }
        [Test]
        public void TestSetState()
        {
            string jsonString = props.GetState();
            ProductProps newProps = new ProductProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductID, newProps.ProductID);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
        [Test]
        public void TestClone()
        {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductID, newProps.ProductID);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}
