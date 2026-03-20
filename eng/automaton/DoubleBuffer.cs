namespace misery.eng.automaton;

public class DoubleBuffer
{
    private bool _isExploitingBufferA = true;
    private readonly Grid _bufferA;
    private readonly Grid _bufferB;

    public DoubleBuffer(int rows, int columns)
    {
        _bufferA = new Grid(rows, columns);
        _bufferB = new Grid(rows, columns);
    }

    public Grid ReadBuffer => _isExploitingBufferA ? _bufferA : _bufferB;
    public Grid WriteBuffer => _isExploitingBufferA ? _bufferB: _bufferA;

    public void Swap() => _isExploitingBufferA = !_isExploitingBufferA;

    public void ForceState(int row, int column, State state)
    {
        if (!_bufferA.IsInside(row, column)) return;
        _bufferA.SetState(row, column, state);
        _bufferB.SetState(row, column, state);
    }
}
