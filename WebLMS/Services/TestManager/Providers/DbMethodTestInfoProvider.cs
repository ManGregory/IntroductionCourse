using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Providers.Interfaces;
using WebLMS.Data;
using WebLMS.Models;

namespace WebLMS.TestManager.Providers
{
    public class DbMethodTestInfoProvider : ITestInfoProvider<CodingTest>
    {
        LMSDbContext _context;
        int _homeworkId;

        public DbMethodTestInfoProvider(LMSDbContext context, int homeworkId)
        {
            _context = context;
            _homeworkId = homeworkId;
        }

        public Func<CodingTest, ITestInfo> ConvertFunction { get; set; }

        public async Task<IEnumerable<ITestInfo>> GetMethodTestsAsync()
        {
            var codingTests = await _context.CodingTests
                .Where(codingTest => codingTest.CodingHomeworkId == _homeworkId && 
                    codingTest.Name != "Compilation" &&
                    codingTest.Name != "Timeout" &&
                    codingTest.Name != "UnknownException")
                .ToListAsync();
            return codingTests.Select(codingTest => ConvertFunction(codingTest));
        }
    }
}
