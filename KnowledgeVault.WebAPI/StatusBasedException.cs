﻿using System.Net;

namespace KnowledgeVault.WebAPI
{
    public class StatusBasedException : Exception
    {
        public StatusBasedException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public StatusBasedException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
