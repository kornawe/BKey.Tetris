namespace BKey.Tetris.Logic.Settings;
public interface ISettingProvider<T>
{
    public T Get();
    public T Set(T value);
}
