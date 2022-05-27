using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingReportsGenerator
{
    public class DataAccess
    {
        private readonly string _connectionstring;
        public DataAccess(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public void returnUsers()
        {
            using SqlConnection connection = new SqlConnection(_connectionstring);



            connection.Query("select * from Users");
        }

        public List<PaymentTransactionsToCapture> GetPaymentTransactions()
        {
            using SqlConnection connection = new SqlConnection(_connectionstring);

            return connection.Query<PaymentTransactionsToCapture>("EXEC [dbo].[rptGetPaymentTransactionsToCaptureForConsole]").ToList();
        }

        public List<InvoiceChargeTransactionsToCapture> GetInvoiceChargeTransactionsToCapture()
        {
            using SqlConnection connection = new SqlConnection(_connectionstring);

            return connection.Query<InvoiceChargeTransactionsToCapture>("EXEC [dbo].[rptGetInvoiceChargeTransactionsToCaptureForConsole]").ToList();
        }

    }
}
