namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;
	using WebKit;

	public static partial class UIExtensions
	{
		#region Html (UI) property

		public static Binder<TSource, UIWebView> Html<TSource, TPropertyType>(this Binder<TSource, UIWebView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			Action<UIWebView, string> setter = (b, v) => b.LoadHtmlString(v, null);
			Func<UIWebView, string> getter = (b) => b.EvaluateJavascript("document.documentElement.outerHTML");
			return binder.Property(property, getter, setter, converter);
		}

		#endregion

		#region Html (WK) property

		public static Binder<TSource, WKWebView> Html<TSource, TPropertyType>(this Binder<TSource, WKWebView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			Action<WKWebView, string> setter = (b, v) => b.LoadHtmlString(v, null);
			Func<WKWebView, string> getter = (b) => { throw new InvalidOperationException("No available way to get HTML from WKWebView"); } ;
			return binder.Property(property, getter, setter, converter);
		}

		#endregion
	}
}
