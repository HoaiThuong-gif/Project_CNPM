using Project_CNPM.DTOs;

namespace Project_CNPM.Services
{
    public interface IUserPredictionService
    {
        Task<(bool IsSuccess, string Message, List<PredictResultDto>? Results)> PredictDrugsAsync(int userId, PredictRequestDto request);
        Task<(bool IsSuccess, string Message)> SubmitFeedbackAsync(int userId, FeedbackRequestDto request);
    }
}