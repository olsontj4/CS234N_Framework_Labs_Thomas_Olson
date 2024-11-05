using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using MMABooksProps;

using System.Data;

using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB
{
    public class ProductDB : DBBase, IReadDB, IWriteDB
    {
        public IBaseProps Create(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;
            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters.Add("productCode_p", DBDbType.VarChar);
            command.Parameters.Add("description_p", DBDbType.VarChar);
            command.Parameters.Add("unitPrice_p", DBDbType.Decimal);
            command.Parameters.Add("onHandQuantity_p", DBDbType.Int32);
            command.Parameters[0].Direction = ParameterDirection.Output;
            command.Parameters["productCode_p"].Value = props.ProductCode;
            command.Parameters["description_p"].Value = props.Description;
            command.Parameters["unitPrice_p"].Value = props.UnitPrice;
            command.Parameters["onHandQuantity_p"].Value = props.OnHandQuantity;
            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ProductID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
        public bool Delete(IBaseProps p)
        {
            ProductProps props = (ProductProps)p;
            int rowsAffected = 0;
            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters.Add("conCurrID", DBDbType.Int32);
            command.Parameters["prodID"].Value = props.ProductID;
            command.Parameters["conCurrID"].Value = props.ConcurrencyID;
            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
        public IBaseProps Retrieve(object key)
        {
            DBDataReader data = null;
            ProductProps props = new ProductProps();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("productCode_p", DBDbType.VarChar);
            command.Parameters["productCode_p"].Value = key.ToString();
            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        }
        public object RetrieveAll()
        {
            List<ProductProps> list = new List<ProductProps>();
            DBDataReader reader = null;
            ProductProps props;
            try
            {
                reader = RunProcedure("usp_ProductSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new ProductProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }
        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            ProductProps props = (ProductProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_ProductUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("prodID", DBDbType.Int32);
            command.Parameters.Add("productCode_p", DBDbType.VarChar);
            command.Parameters.Add("description_p", DBDbType.VarChar);
            command.Parameters.Add("unitPrice_p", DBDbType.Decimal);
            command.Parameters.Add("onHandQuantity_p", DBDbType.Int32);
            command.Parameters.Add("conCurrID", DBDbType.Int32);
            command.Parameters["prodID"].Value = props.ProductID;
            command.Parameters["productCode_p"].Value = props.ProductCode;
            command.Parameters["description_p"].Value = props.Description;
            command.Parameters["unitPrice_p"].Value = props.UnitPrice;
            command.Parameters["onHandQuantity_p"].Value = props.OnHandQuantity;
            command.Parameters["conCurrID"].Value = props.ConcurrencyID;
            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }
    }
}
