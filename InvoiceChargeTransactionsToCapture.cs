using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingReportsGenerator
{
    public class InvoiceChargeTransactionsToCapture
    {
        public int TRANS_UNIQUE { get; set; }
        public string NAME { get; set; }
        public int LOAD_NO { get; set; }
        public int STOP_NO { get; set; }
        public double CHARGE_AMOUNT { get; set; }
        public string CHARGE_TYPE { get; set; }
        public string POST_DATE { get; set; }
        public string PROCESS_DATE { get; set; }
        public string CHARGE_DATE { get; set; }
        public double CHARGE_RATE { get; set; }
        public bool SPREAD_COST { get; set; }
        public bool EXPENSE_REIMBURSEMENT { get; set; }
        public bool ADJUSTMENT { get; set; }
        public bool MANUAL_TRANSACTION { get; set; }
        public int CO_ID { get; set; }
        public int SOURCE { get; set; }
        public int SOURCE_REFERENCE { get; set; }
        public int INVOICE_NUMBER { get; set; }
        public string CHARGE_CURRENCY { get; set; }
        public bool HELD { get; set; }
        public string GENERAL_LEDGER_ACCOUNT { get; set; }
        public long WW_LOAD_ID { get; set; }
        public string READY_TO_BE_CHARGED_DATE { get; set; }
        public string FIRST_STOP_CITY { get; set; }
        public string ALT_LOAD_ID { get; set; }
    }
}
