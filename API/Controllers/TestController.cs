using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class TestController : MainApiController
    {
        private readonly ITestService _testService;
        private readonly ITransactionService _transactionService;

        public TestController(ITestService testService, ITransactionService transactionService)
        {
            _testService = testService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //await _testService.InsertData();
            //await _testService.DummyData1();
            //await _testService.DummyData2();
            //await _testService.AddNewRoles();
            //await _testService.AddNewUser();
            //await _transactionService.FinancialTransaction();
            await _testService.CreateAndroidAndWebClient();
            return Ok("hello world");
        }
    }
}
