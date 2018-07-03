using System;
using System.Collections.Generic;
#if !NET20
using System.Linq;
#endif

namespace Mini.Foundation.Basic.CommonObj
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result Fail(Exception ex, Func<Exception, String> exFormater = null)
        {
            var message = String.Empty;
            if (ex != null)
            {
                if (exFormater != null)
                {
                    message = exFormater(ex);
                }
                else
                {
                    message = Utility.ExceptionHelper.FormatException(ex);
                }
            }
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T> Fail<T>(Exception ex, Func<Exception, String> exFormater = null)
        {
            var message = String.Empty;
            if (ex != null)
            {
                if (exFormater != null)
                {
                    message = exFormater(ex);
                }
                else
                {
                    message = Utility.ExceptionHelper.FormatException(ex);
                }
            }
            return new Result<T>(default(T), false, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

#if !NET20

        public static Result Combine(IEnumerable<Result> results)
        {
            foreach (Result result in results.Where(x => x != null))
            {
                if (result.IsFailure)
                    return result;
            }

            return Ok();
        }

        public static Result Combine(params Result[] results)
        {
            return Combine((IEnumerable<Result>)results);
        }

#endif

    }

    /// <summary>
    /// 如果执行结果非success，获取value的时候，会抛出异常
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new ResultFailedException(Error);

                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }

    /// <summary>
    /// 如果执行结果非success，获取value的时候，不会抛出异常，而是返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result2<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    return default(T);
                return _value;
            }
        }

        protected internal Result2(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
