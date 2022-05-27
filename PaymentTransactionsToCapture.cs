using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingReportsGenerator
{
    public class PaymentTransactionsToCapture
    {
        public int TRANSACTION_ID { get; set; }
        public string NAME { get; set; }
        public int LOAD_NO { get; set; }
        public double PAYMENT_AMOUNT { get; set; }
        public string PAYMENT_TYPE { get; set; }
        public string POST_DATE { get; set; }
        public string PROCESS_DATE { get; set; }
        public string PAYMENT_DATE { get; set; }
        public double PAYMENT_RATE { get; set; }
        public bool ADJUSTMENT { get; set; }
        public bool MANUAL_TRANSACTION { get; set; }
        public int CO_ID { get; set; }
        public int SOURCE { get; set; }
        public int SOURCE_REFERENCE { get; set; }
        public int CHECK_NUMBER { get; set; }
        public string PAYMENT_CURRENCY { get; set; }
        public int PAYMENT_ID { get; set; }
        public string PO_NUMBER { get; set; }
        public string GENERAL_LEDGER_ACCOUNT { get; set; }
        public string READY_TO_BE_PAID_DATE { get; set; }
        public int WW_LOAD_ID { get; set; }
        public string FIRST_STOP_CITY { get; set; }
        public string ALT_LOAD_ID { get; set; }
    }
   
}
