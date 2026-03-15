public sealed class StringIdGenerator
{
    private int _next;

    public StringIdGenerator(int start = 1) => _next = start;

    public string Next(string prefix)
    {
        // z.B. bld_000123
        string id = $"{prefix}_{_next:000000}";
        _next++;
        return id;
    }

    // Beim Restore: wenn du schon "bld_000150" genutzt hast,
    // soll der Generator danach bei 151 weiter machen.
    public void ReserveNumericPart(int used)
    {
        if (used >= _next) _next = used + 1;
    }

    public int PeekNext() => _next;
}