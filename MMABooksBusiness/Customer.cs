using System;

using MMABooksTools;
using MMABooksProps;
using MMABooksDB;

using System.Collections.Generic;
using System.Data;

namespace MMABooksBusiness
{
    public class Customer : BaseBusiness
    {
        public int CustomerID
        {
            get
            {
                return ((CustomerProps)mProps).CustomerID;
            }
        }

        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).Name;
            }
            set
            {
                if (!(value == ((CustomerProps)mProps).Name))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("Name", false);
                        ((CustomerProps)mProps).Name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Name must be no more than 100 characters long.");
                    }
                }
            }
        }
        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).Address;
            }
            set
            {
                if (!(value == ((CustomerProps)mProps).Address))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("Address", false);
                        ((CustomerProps)mProps).Address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("Address must be no more than 100 characters long.");
                    }
                }
            }
        }
        public string City
        {
            get
            {
                return ((CustomerProps)mProps).City;
            }
            set
            {
                if (!(value == ((CustomerProps)mProps).City))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("City", false);
                        ((CustomerProps)mProps).City = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("City must be no more than 100 characters long.");
                    }
                }
            }
        }
        public string State
        {
            get
            {
                return ((CustomerProps)mProps).State;
            }
            set
            {
                if (!(value == ((CustomerProps)mProps).State))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("State", false);
                        ((CustomerProps)mProps).State = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("State must be no more than 100 characters long.");
                    }
                }
            }
        }
        public string ZipCode
        {
            get
            {
                return ((CustomerProps)mProps).ZipCode;
            }
            set
            {
                if (!(value == ((CustomerProps)mProps).ZipCode))
                {
                    if (value.Trim().Length >= 1 && value.Trim().Length <= 100)
                    {
                        mRules.RuleBroken("ZipCode", false);
                        ((CustomerProps)mProps).ZipCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentOutOfRangeException("ZipCode must be no more than 100 characters long.");
                    }
                }
            }
        }
        public override object GetList()
        {
            List<Customer> Customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll();
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop);
                Customers.Add(c);
            }

            return Customers;
        }

        protected override void SetDefaultProperties()
        {

        }

        /// <summary>
        /// Sets required fields for a record.
        /// </summary>
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("Name", true);
            mRules.RuleBroken("Address", true);
            mRules.RuleBroken("City", true);
            mRules.RuleBroken("State", true);
            mRules.RuleBroken("ZipCode", true);
        }

        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            mdbReadable = new CustomerDB();
            mdbWriteable = new CustomerDB();
        }

        #region constructors
        /// <summary>
        /// Default constructor - gets the connection string - assumes a new record that is not in the database.
        /// </summary>
        public Customer() : base()
        {
        }

        // <summary>
        // Calls methods SetUp() and Load().
        // Use this constructor when the object is in the database AND the connection string is in a config file
        // </summary>
        // <param name="key">ID number of a record in the database.
        // Sent as an arg to Load() to set values of record to properties of an 
        // object.</param>
        public Customer(int key) : base(key)
        {
        }

        private Customer(CustomerProps props) : base(props)
        {
        }

        #endregion
    }
}
