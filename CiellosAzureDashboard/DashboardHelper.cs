using CiellosAzureDashboard.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Pages
{
    public static class DashboardHelper
    {
        private static byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return salt;
        }
        public static string CalculateHash(string input)
        {
            var salt = GenerateSalt(16);

            var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);
            var retString = $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
            retString = retString.Replace("=", "").Replace(".", "").Replace("/", "").Replace("\\", "").Replace(",", "").Replace(":", "").Replace(";", "").Replace("+", "").Replace("-", "").Replace("*", "");
            return retString;
        }

        public static void CleanLogs(CADContext _context)
        {
            var settings = _context.Settings.FirstOrDefault();

            var vmInLogTable = _context.Logs.GroupBy(l => new { l.vmname, l.resourcegroup }).ToList();
            foreach (var vm in vmInLogTable)
            {
                var logs_count = _context.Logs.Where(l => l.vmname == vm.Key.vmname && l.resourcegroup == vm.Key.resourcegroup).Count();
                if (logs_count > settings.MaxNumEventsLogStorePerVM)
                {
                    var vmLastLogRecord = _context.Logs.Where(l => l.vmname == vm.Key.vmname && l.resourcegroup == vm.Key.resourcegroup).OrderByDescending(u => u.timestamp).Take(settings.MaxNumEventsLogStorePerVM).Last();
                    var logsForRemove = _context.Logs.Where(l => l.vmname == vm.Key.vmname && l.resourcegroup == vm.Key.resourcegroup && l.timestamp < vmLastLogRecord.timestamp).ToList();
                    _context.Logs.RemoveRange(logsForRemove);
                }
            }
            _context.SaveChanges();

            var errorLogsToRemove = _context.Logs.Where(l => string.IsNullOrEmpty(l.vmname)).ToList();
            _context.Logs.RemoveRange(errorLogsToRemove);
            _context.SaveChanges();
        }
    }
}
