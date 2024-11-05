using Microsoft.AspNetCore.Mvc;
using TextDataProcessing.Helpers;
using TextDataProcessing.Models;

namespace TextDataProcessing.Controllers
{
   
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        [ProducesResponseType(typeof(ExpenseData), StatusCodes.Status200OK)]
        [HttpPost]
        [Route("~/expense/get")]
        //[Consumes("text/plain")]  // Tells the API to accept plain text
        public IActionResult ProcessExpense(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return BadRequest("Input text cannot be empty.");
            }

            var expenseData = ExpenseParser.ParseExpenseData(inputText);

            if (expenseData == null)
            {
                return BadRequest("Invalid format or missing required fields.");
            }

            return Ok(expenseData);
        }
    }

}

