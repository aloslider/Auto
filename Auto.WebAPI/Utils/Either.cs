readonly struct Either<TLeft, TRight>
{
    readonly TLeft _left;
    readonly TRight _right;
    readonly EitherState State;

    private Either(TLeft? left)
    {
        if (left is null)
            throw new NullReferenceException("Left value is null");

        _left = left;
        _right = default!;
        State = EitherState.Left;
    }

    private Either(TRight? right)
    {
        if (right is null)
            throw new NullReferenceException("Right value is null");

        _left = default!;
        _right = right;
        State = EitherState.Right;
    }

    public static implicit operator Either<TLeft, TRight>(EitherLeft<TLeft> left) =>
        new(left.Value);

    public static implicit operator Either<TLeft, TRight>(EitherRight<TRight> right) =>
        new(right.Value);

    public Ret Match<Ret>(Func<TRight, Ret> Right, Func<TLeft, Ret> Left) =>
        State switch
        {
            EitherState.Right => Right(_right),
            _ => Left(_left)
        };

    enum EitherState { Left, Right }
}

readonly struct EitherLeft<TLeft>
{
    internal readonly TLeft Value;

    EitherLeft(TLeft value)
    {
        if (value is null)
            throw new NullReferenceException("Left value is null");

        Value = value;
    }

    public static EitherLeft<TLeft> Left(TLeft value) =>
        new(value);
}

readonly struct EitherRight<TRight>
{
    internal readonly TRight Value;

    EitherRight(TRight value)
    {
        if (value is null)
            throw new NullReferenceException("Right value is null");

        Value = value;
    }

    public static EitherRight<TRight> Right(TRight value) =>
        new(value);
}