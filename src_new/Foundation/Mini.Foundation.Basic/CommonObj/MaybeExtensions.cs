#if !NET20

namespace Mini.Foundation.Basic.CommonObj
{
    public static class Maybe
    {
        public static Maybe<T> Empty<T>() where T : class
        {
            return new Maybe<T>();
        }

        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage) where T : class
        {
            if (maybe.HasNoValue)
                return Result.Fail<T>(errorMessage);

            return Result.Ok(maybe.Value);
        }
    }
}

#endif
