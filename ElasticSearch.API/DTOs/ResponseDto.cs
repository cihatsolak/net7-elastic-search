﻿namespace ElasticSearch.API.DTOs
{
    public record ResponseDto<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public HttpStatusCode Status { get; set; }

        //Static Factory Method
        public static ResponseDto<T> Success(T data, HttpStatusCode status)
        {
            return new ResponseDto<T>
            {
                Data = data,
                Status = status
            };
        }

        public static ResponseDto<T> Fail(List<string> erros, HttpStatusCode status)
        {
            return new ResponseDto<T>
            {
                Errors = erros,
                Status = status
            };
        }
        public static ResponseDto<T> Fail(string error, HttpStatusCode status)
        {
            return new ResponseDto<T>
            {
                Errors = new List<string> { error },
                Status = status
            };
        }
    }
}
