using Auto.WebAPI.Utils;

static class Prelude
{
    public static Unit unit = new();

    public static EitherLeft<TLeft> Left<TLeft>(TLeft left) =>
        EitherLeft<TLeft>.Left(left);

    public static EitherRight<TRight> Right<TRight>(TRight right) =>
        EitherRight<TRight>.Right(right);
}
