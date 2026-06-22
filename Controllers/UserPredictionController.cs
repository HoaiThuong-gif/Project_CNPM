using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_CNPM.DTOs;
using Project_CNPM.Services;
using System.Security.Claims;

namespace Project_CNPM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserPredictionController : ControllerBase
    {
        private readonly IUserPredictionService _predictionService;

        public UserPredictionController(IUserPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> Predict([FromBody] PredictRequestDto request)
        {
            int userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _predictionService.PredictDrugsAsync(userId, request);

            if (result.IsSuccess)
                return Ok(new { message = result.Message, data = result.Results });

            return BadRequest(new { message = result.Message });
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackRequestDto request)
        {
            int userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _predictionService.SubmitFeedbackAsync(userId, request);

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(new { message = result.Message });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            int userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _predictionService.GetUserHistoryAsync(userId);
            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpDelete("history/{historyId}")]
        public async Task<IActionResult> DeleteHistory(int historyId)
        {
            int userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _predictionService.DeleteHistoryAsync(userId, historyId);
            if (!result.IsSuccess) return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}