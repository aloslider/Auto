class ErrorResponse(string error)
{
    public string Error { get; } = error;

    public static implicit operator ErrorResponse(string s) =>
        new(s);
}