namespace ModernWMS.Core.Models
{
    /// <summary>
    /// http response viewmodel
    /// </summary>
    public class ResultModel<T>
    {
        /// <summary>
        /// is request success
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// status code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// error message
        /// </summary>
        public string ErrorMessage { get; set; } = "";
        /// <summary>
        /// data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// success
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="errMsg">error message</param>
        /// <returns></returns>
        public static ResultModel<T> Success(T data, string errMsg = "")
        {
            if (data == null)
            {
                return Error("Some errors have occurred");
            }
            return new ResultModel<T> { Data = data, ErrorMessage = errMsg, IsSuccess = true, Code = 200 };
        }
        /// <summary>
        /// faild
        /// </summary>
        /// <param name="str">error message</param>
        /// <param name="code">status code</param>
        /// <param name="data">data</param>
        /// <returns></returns>
        public static ResultModel<T> Error(string str, int code = 400, T data = default)
        {
            return new ResultModel<T> { Data = data, ErrorMessage = str, IsSuccess = false, Code = code };
        }
    }
}
