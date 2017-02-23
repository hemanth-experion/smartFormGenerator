namespace Experion.Marina.Dto
{
    public class ResponseDto<TData> : MessageDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseDto{TData}"/> class.
        /// </summary>
        public ResponseDto()
        {
        }

        /// <summary>
        /// Gets or sets the data for the API Response.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TData Data { get; set; }
    }
}
