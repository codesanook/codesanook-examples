using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CodeSanook.Examples.DotNetCore.Orm 
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbFactory _dbFactory;
        private static IDbContextTransaction   _transaction;
        private static System.Transactions.TransactionScope _transactionScope;
        public EFUnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public EFDbCotext dataContext
        {
            get { return _dbFactory.GetCurrentDataContext; }
        }

        public void Commit()
        {
            try
            {
                dataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string signature = $@"<br /><br /><p><b>Sent from</b> {ConstantHelper.UrlDomainName}</p>";
                var errorMessages = ex.EntityValidationErrors
                     .SelectMany(x => x.ValidationErrors)
                     .Select(x => x.ErrorMessage);

                var clientName = System.Net.Dns.GetHostEntry(ConfigurationManager.AppSettings["HostName"]).HostName;

                // Join the list to a single string.
                var fullErrorMessage = $"{String.Join("; ", errorMessages)} <br /><b>Inner Exception:</b> {ex.InnerException} <br /><b>ConnectionString:</b> {dataContext.Database.Connection.ConnectionString}";

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, $"<b>Client Name:</b> {clientName} <br /><b>The validation errors are:</b> ", fullErrorMessage);

                // email body
                var mailAlertBody = $"<b>Client Name:</b> {clientName} <br /><b>Error Message:</b> {errorMessages} <br /><b>ConnectionString:</b> {dataContext.Database.Connection.ConnectionString} <br /><b>TargetSite:</b> {ex.TargetSite}";
                mailAlertBody += signature;
                using (var db = new DbEntities())
                {
                    TblSystemLog_HistoryError newError = new TblSystemLog_HistoryError();
                    newError.Guid = Guid.NewGuid();
                    newError.ErrorDescription = exceptionMessage;
                    newError.FunctionName = ex.TargetSite.Name;
                    newError.PageName = "Commit data error.";
                    newError.DatetimeCreated = DateTime.UtcNow;
                    db.TblSystemLog_HistoryError.Add(newError);
                    db.SaveChanges();

                    SendEmailToAdmin(mailAlertBody);
                }

                throw ex;
            }
        }

        private void SendEmailToAdmin(string fullErrorMessage)
        {
            var emailToText = ConfigurationManager.AppSettings["EmailAlertSystemError"];
            var emailTo = emailToText.Split(',').ToList();

            List<MailProfile> mailTo = new List<MailProfile>();
            for (int i = 0; i < emailTo.Count(); i++)
            {
                MailProfile mail = new MailProfile()
                {
                    MailAddress = emailTo.ElementAt(i)
                };

                mailTo.Add(mail);
            }

            SendMailMessage message = new SendMailMessage();
            message.emailFrom = ConfigurationManager.AppSettings["From"];
            message.emailFromDisplayName = ConfigurationManager.AppSettings["FromName"];
            message.emailTo = mailTo;
            message.emailSubject = "Subject";
            message.emailBody = fullErrorMessage;

            int intSendMailType;
            intSendMailType = int.Parse(ConfigurationManager.AppSettings["SmtpType"]);
            switch (intSendMailType)
            {
                case 1:
                    message.sendBy = SendMailMessage.SendByEnum.BrinksSmtp;
                    message.smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                    break;
            }

            SendMail sendMail = new SendMail();
            sendMail.MailSend(message);
        }

        public async Task CommitAsync()
        {
            try
            {
                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                using (var db = new DbEntities())
                {
                    var newError = new ErrorObject();
                    newError.Guid = Guid.NewGuid();
                    newError.ErrorDescription = ex.ToString();
                    newError.FunctionName = ex.TargetSite.Name;
                    newError.PageName = "Commit data error.";
                    newError.DatetimeCreated = DateTime.UtcNow;
                    db.TblSystemLog_HistoryError.Add(newError);
                    db.SaveChanges();
                }

                throw ex;
            }
        }

        public DbContextTransaction BeginTransaction()
        {
            // dispose old transaction
            _dbFactory.ClearConnection(_transaction);

            // new transaction
            _transaction = dataContext.Database.BeginTransaction();

            return _transaction;
        }

        public void DebugLog()
        {
            dataContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public string GetConnectionString()
        {
            return dataContext.Database.Connection.ConnectionString;
        }

        public void Dispose()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public void ClearSession()
        {
            _dbFactory.ClearSession();
        }

        public System.Transactions.TransactionScope createTransactionScope()
        {
            disposeTransactionScope();
            _transactionScope = new TransactionScope();
            return _transactionScope;
        }

        public void completeTransactionScope()
        {
            if (_transactionScope != null)
            {
                _transactionScope.Complete();
                disposeTransactionScope();
            }
        }

        public void disposeTransactionScope()
        {
            if (_transactionScope != null)
            {
                _transactionScope.Dispose();
                _transactionScope = null;
            }
        }
    }
}
