using RedAndWhite.Infrastructure.Enums;

namespace RedAndWhite.Model.Shared
{
    public class ResultDTO<T>
    {
        public ResultDTO() { }

        public ResultDTO(ResultStatusEnum resultStatus, string? message)
        {
            ResultStatus = resultStatus;
            Message = message;
        }

        public ResultStatusEnum ResultStatus { get; set; } = ResultStatusEnum.Success;
        public string? Message { get; set; }
        public T? Object { get; set; }
        public List<T>? ObjectList { get; set; }
    }
}