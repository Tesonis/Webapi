using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Webapi.Models
{
    internal class DataSource
    {
        private IBM.Data.DB2.iSeries.iDB2Connection cn = null/* TODO Change to default(_) if this is not a reference type */;


        public DataSource(string server, string username, string password)
        {
            OpenConnection("DataSource=" + server + "; UserID=" + username + "; Password=" + password + ";");
        }
        // Public ReadOnly Property ProductionServer() As String
        // Get
        // Return _productionServer
        // End Get
        // End Property
        // Public ReadOnly Property TestServer() As String
        // Get
        // Return _testServer
        // End Get
        // End Property
        public void ExecuteCommand(string commandText, ref int rowsAffected, ref IBM.Data.DB2.iSeries.iDB2Transaction transaction/* TODO Change to default(_) if this is not a reference type */)
        {
            IBM.Data.DB2.iSeries.iDB2Command cmd = new IBM.Data.DB2.iSeries.iDB2Command();

            // Make sure that we have a connnection to the database
            if (cn == null)
                return;

            try
            {
                {
                    var withBlock = cmd;
                    // .CommandTimeout
                    withBlock.CommandType = CommandType.Text;
                    withBlock.CommandText = commandText;
                    withBlock.Connection = cn;
                    if (transaction != null)
                        withBlock.Transaction = transaction;
                    rowsAffected = withBlock.ExecuteNonQuery();
                }
            }
            finally
            {
                // Release resources
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
        }
        public void ExecuteCommand(string commandText, List<IBM.Data.DB2.iSeries.iDB2Parameter> parameters, ref IBM.Data.DB2.iSeries.iDB2Transaction transaction/* TODO Change to default(_) if this is not a reference type */)
        {
            IBM.Data.DB2.iSeries.iDB2Command cmd = new IBM.Data.DB2.iSeries.iDB2Command();

            // Make sure that we have a connnection to the database
            if (cn == null)
                return;

            try
            {
                {
                    var withBlock = cmd;
                    // .CommandTimeout
                    withBlock.CommandType = CommandType.Text;
                    withBlock.CommandText = commandText;
                    withBlock.Connection = cn;
                    if (transaction != null)
                        withBlock.Transaction = transaction;

                    // Add any parameters
                    foreach (IBM.Data.DB2.iSeries.iDB2Parameter parameter in parameters)
                        // .Parameters.Add(New IBM.Data.DB2.iSeries.iDB2Parameter(parameter.ParameterName, parameter.iDB2DbType, parameter.Size)).Direction = parameter.Direction
                        withBlock.Parameters.Add(parameter).Direction = parameter.Direction;
                    withBlock.ExecuteNonQuery();
                }
            }
            finally
            {
                // Release resources
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
        }
        public System.Data.DataSet GetDataSet(string commandText/*, ref IBM.Data.DB2.iSeries.iDB2Transaction transaction*//* TODO Change to default(_) if this is not a reference type */)
        {
            IBM.Data.DB2.iSeries.iDB2DataAdapter da = null/* TODO Change to default(_) if this is not a reference type */;
            System.Data.DataSet ds = null;
            IBM.Data.DB2.iSeries.iDB2Transaction transaction = null;
            // Make sure that we have a connnection to the database
            if (cn == null)
                return null;

            try
            {
                if ((transaction == null))
                    // 'Set the connection string
                    // cn = New SqlConnection(pdesdal.GetDSN())

                    // 'Open the connection
                    // cn.Open()

                    // Execute the query with the connection
                    da = new IBM.Data.DB2.iSeries.iDB2DataAdapter(commandText, cn);
                else
                    // Execute the query with the connection of the existing transaction
                    da = new IBM.Data.DB2.iSeries.iDB2DataAdapter(commandText, transaction.Connection);

                // Get the data set
                ds = new System.Data.DataSet();
                da.Fill(ds);

                // Return the dataset
                return ds;
            }
            finally
            {
                // Release resources
                if (!(da == null))
                {
                    da.Dispose();
                    da = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
        }
        //public IBM.Data.DB2.iSeries.iDB2DataReader GetDataReader(string commandText, ref IBM.Data.DB2.iSeries.iDB2Transaction transaction/* TODO Change to default(_) if this is not a reference type */)
        //{
        //    IBM.Data.DB2.iSeries.iDB2Command cmd = new IBM.Data.DB2.iSeries.iDB2Command();

        //    // Make sure that we have a connnection to the database
        //    if (cn == null)
        //        return null/* TODO Change to default(_) if this is not a reference type */;

        //    try
        //    {
        //        {
        //            var withBlock = cmd;
        //            // .CommandTimeout
        //            withBlock.CommandType = CommandType.Text;
        //            withBlock.CommandText = commandText;
        //            withBlock.Connection = cn;
        //            if (transaction != null)
        //                withBlock.Transaction = transaction;

        //            return withBlock.ExecuteReader();
        //        }
        //    }
        //    finally
        //    {
        //        // Release resources
        //        if (cmd != null)
        //        {
        //            cmd.Dispose();
        //            cmd = null/* TODO Change to default(_) if this is not a reference type */;
        //        }
        //    }
        //}
        // Public Function GetDataSet(ByVal storeProcedure As String,
        // ByVal parameters As List(Of String),
        // Optional ByRef transaction As IBM.Data.DB2.iSeries.iDB2Transaction = Nothing) As IBM.Data.DB2.iSeries.iDB2DataReader

        // Dim cmd As New IBM.Data.DB2.iSeries.iDB2Command

        // 'Make sure that we have a connnection to the database
        // If cn Is Nothing Then Return Nothing

        // Try
        // With cmd
        // '.CommandTimeout
        // .CommandType = CommandType.StoredProcedure
        // .CommandText = storeProcedure
        // .Connection = cn
        // If transaction IsNot Nothing Then .Transaction = transaction

        // For Each parameter As String In parameters
        // Dim param As New IBM.Data.DB2.iSeries.iDB2Parameter()
        // param = .Parameters.Add("IN_JSON_DATA", IBM.Data.DB2.iSeries.iDB2DbType.iDB2VarChar, 32000)
        // param.Direction = ParameterDirection.Input

        // .Parameters.Add(param)

        // param = Nothing
        // Next

        // Return .ExecuteReader()
        // End With

        // Finally
        // 'Release resources
        // If cmd IsNot Nothing Then
        // cmd.Dispose()
        // cmd = Nothing
        // End If

        // End Try

        // End Function

        private IBM.Data.DB2.iSeries.iDB2Connection OpenConnection(string connectionString)
        {
            cn = new IBM.Data.DB2.iSeries.iDB2Connection()
            {
                ConnectionString = connectionString
            };

            cn.Open();

            return cn;
        }

        private void CloseConnection()
        {
            try
            {
                if (cn != null)
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        // cn.Close()
                        cn.Dispose();

                        cn = null;
                    }
                }
            }
            finally
            {
            }
        }
        ~DataSource()
        {
            CloseConnection();
        }
    }
}