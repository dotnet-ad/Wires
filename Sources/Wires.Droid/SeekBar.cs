namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Widget;
	using Transmute;

	public static partial class UIExtensions
	{
		private static ITwoWayConverter<float, int> PercentToAmountConverter = new TwoWayConverter<float, int>(new RelayConverter<float, int>(x => (int)(x * 100.0f)), new RelayConverter<int, float>(x => x / 100.0f));

		#region Progress property

		public static Binder<TSource, SeekBar> Progress<TSource, TPropertyType>(this Binder<TSource, SeekBar> binder, Expression<Func<TSource, TPropertyType>> property, ITwoWayConverter<TPropertyType, float> converter = null)
			where TSource : class
		{
			converter = converter ?? Transmute.Transmuter.Default.GetTwoWayConverter<TPropertyType, float>();
			var finalconverter = converter.Chain(PercentToAmountConverter);
			return binder.Property<TPropertyType, int, SeekBar.ProgressChangedEventArgs>(property, b => b.Progress, nameof(SeekBar.ProgressChanged), finalconverter);
		}

		#endregion
	}
}