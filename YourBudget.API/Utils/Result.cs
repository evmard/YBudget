using System;
using System.Collections.Generic;
using System.Linq;

namespace YourBudget.API.Utils
{
    public class Result
    {
        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public Result() : this (true, null)
        {
        }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Error(string message)
        {
            return new Result(false, message);
        }

        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        public void AddValidationError(string fieldName, string message)
        {
            if (_validationErrors == null)
                _validationErrors = new List<FieldValidationError>();

            _validationErrors.Add(new FieldValidationError(fieldName, message));
            IsSuccess = false;
        }

        private List<FieldValidationError> _validationErrors;
        public IEnumerable<FieldValidationError> ValidationErrors { get { return _validationErrors ?? Enumerable.Empty<FieldValidationError>(); } }
    }

    public class FieldValidationError
    {
        public string FieldName { get; set; }
        public string Message { get; set; }

        public FieldValidationError(string fieldName, string message)
        {
            FieldName = fieldName;
            Message = message;
        }

        public FieldValidationError()
        {
        }
    }

    public class Result<TData> : Result
    {
        private Result(TData data, bool isSuccess, string message): base(isSuccess, message)
        {
            Data = data;
        }

        private Result(string message) : base(false, message)
        {
        }

        public Result(): base(true, null)
        {
        }

        public static Result<TData> Success(TData data)
        {
            return new Result<TData>(data, true, null);
        }

        public new static Result<TData> Error(string message)
        {
            return new Result<TData>(message);
        }

        public TData Data { get; set; }
    }
}