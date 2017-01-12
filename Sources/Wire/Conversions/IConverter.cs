namespace Wire
{
	public interface IConverter<TSource,TTarget>
	{
		TTarget Convert(TSource value);

		TSource ConvertBack(TTarget value);
	}
}
