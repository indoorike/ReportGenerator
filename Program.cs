
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AccountingReportsGenerator
{
    public class Program
    {
        //TO DO: Add in args so that when more reports are added to this process, we can pass in arguments to specify what reports are created. 
        static void Main()
        {
            //Building the configuration to read appsettings and initializing Serilog.
            IConfiguration config = Configuration().Build();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"{config.GetSection("FilePaths").GetSection("LoggingPath").Value}\\log_.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            DataAccess dataAccess = new DataAccess(config.GetConnectionString("threepl"));

            //Checks to see if test mode is on, grabs the active and archive file paths from appsettings.
            bool testArchiveDelete = bool.Parse(config.GetSection("TestArchiveDelete").Value.ToLower());
            string activeFilesPath = config.GetSection("FilePaths").GetSection("CurrentFiles").Value;
            string archivedFilesPath = config.GetSection("FilePaths").GetSection("ArchivedFiles").Value;

            try
            {
                //Grabbing all PaymentTransactionsToCapture from the database using a stored procedure.
                Log.Information("Getting payment transactions data.");
                List<PaymentTransactionsToCapture> paymentTransactions = dataAccess.GetPaymentTransactions();
                Log.Information("^Success.");


                //Grabbing all InvoiceChargeTransactionsToCapture from the database using a stored procedure.
                Log.Information("Getting data for invoice charge transactions to capture.");
                List<InvoiceChargeTransactionsToCapture> invoiceChargeTransactionsToCaptures = dataAccess.GetInvoiceChargeTransactionsToCapture();
                Log.Information("^Success.");

                //Placing the stored procedure results inside of a dynamic array. This allows us to dynamically create the excels later. 
                dynamic[] reportTypes = new dynamic[]
                {
                    paymentTransactions,
                    invoiceChargeTransactionsToCaptures
                };

                //The purpose of the accounting excel class is to handle all of the file shuffling and path creation. 
                AccountingExcel accountingExcel = new AccountingExcel(activeFilesPath, archivedFilesPath);

                //Begins the excel generation process by moving all files currently located in the active path to the archive path. 
                Log.Information($"Archiving Current Files From {activeFilesPath} >>> To {archivedFilesPath}");
                accountingExcel.ArchiveCurrentFiles();
                Log.Information("^Success.");

                //Creating the excels. Takes in a dynamic array. When more reports are needed to be created, add them to the dynamic array above.
                Log.Information($"Placing New Files in ${activeFilesPath}");
                accountingExcel.CreateExcels(reportTypes);
                Log.Information("^Success.");

                //Checking the archive folder for any files older than 30 days, and deleting them
                Log.Information($"Deleting files older than 30 days from {archivedFilesPath}");
                Log.Information($"^Success. {accountingExcel.DeleteOldArchives(testArchiveDelete)} Files Were Deleted.");

            }

            catch (Exception ex)
            {
                Log.Error($"Issue with report generation process... \n{ex.Message}\n{ex.StackTrace}\nEnding Process...");
                return;
            }
        }

        static private IConfigurationBuilder Configuration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}
