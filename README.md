![Logo](./Documentation/Logo.png)

Wires is a simple binding library for frameworks that doesn't have built-in binding mecanisms. Many choices have been made to have a restrictive base API. A wide set of extensions are also package for Xamarin.iOS and Xamarin.Android.

## Why ?

Several other solutions exists, but I've experienced a **lot** of memory issues with these : that's why I've decided to initiate my own binding library.

## Install

Available on NuGet

[![NuGet](https://img.shields.io/nuget/v/Wires.svg?label=NuGet)](https://www.nuget.org/packages/Wires/)

## Quickstart

### iOS

To bind data to components :

```csharp
this.ViewModel
		    .Bind(this.label)
		    	.Text(vm => vm.Title, Converters.Uppercase)
			.Bind(this.field)
		    	.Text(vm => vm.Title)
			.Bind(this.image)
		    	.ImageAsync(vm => vm.Illustration)
		    	.Alpha(vm => vm.Amount)
		    	.Visible(vm => vm.IsActive)
			.Bind(this.toggleSwitch)
		    	.On(vm => vm.IsActive)
			.Bind(this.slider)
		    	.Value(vm => vm.Amount)
			.Bind(this.datePicker)
		    	.Date(vm => vm.Birthday)
			.Bind(this.progressView)
		    	.Progress(vm => vm.Amount)
			.Bind(this.activityIndicator)
		    	.IsAnimating(vm => vm.IsLoading)
			.Bind(this.segmented)
		    	.Titles(vm => vm.Sections)
			.Bind(this.button)
		    	.TouchUpInside(vm => vm.LoadCommand);
```

Value converters can also be used with an `IConverter<TSource,TTarget>` implementation, or a lambda expression :

```csharp
this.ViewModel.Bind(this.label).TextColor(vm => vm.IsValid, x => x ? UIColor.Green : UIColor.Red);
```

## Bindings

### Build-in extensions

#### iOS

* **UIView**
  * `Visible` *bool*
  * `Hidden` *bool*
  * `TintColor` *UIColor*
  * `BackgroundColor` *UIColor*
  * `Alpha` *nfloat*
* **UIActivityIndicator**
  * `IsAnimating` *bool*
* **UIButton**
  * `TouchUpInside` *ICommand*
  * `Title` *string*
  * `Image` *UIImage*
* **UIDatePicker**
  * `Date` *DateTime*
* **UIProgressView**
  * `Progress` *double*
  * `ProgressTintColor` *UIColor*
  * `TrackTintColor` *UIColor*
* **UIImageView**
  * `Image` *UIImage*
  * `ImageAsync` *UIImage*
* **UILabel**
  * `Text` *string*
  * `TextColor` *UIColor*
* **UISegmentedControl**
  * `Titles` *string[]*
  * `Selected` *int*
* **UISlider**
  * `Value` *float*
  * `MaxValue` *float*
  * `MinValue` *float*
* **UIStepper**
  * `Value` *double*
  * `MaximumValue` *double*
  * `MinimumValue` *double*
* **UISwitch**
  * `On` *bool*
* **UITextField**
  * `Text` *string*
* **UIViewController**
  * `Title` *string*
  * `BackTitle` *string*
* **UIWebView / WKWebView**
  * `HtmlContent` *string*

### Basic APIs

**Wires** provides more basic APIs on which are based all the extensions.

```csharp
this.ViewModel.Bind(custom).Property(vm => vm.Source, x => x.Target);
this.ViewModel.Bind(custom).Property<TSourceType, TTargetType, EventArgs>(vm => vm.Source, x => x.Value, nameof(Custom.ValueChanged));
```

For more advanced options see `Binder<TSource,TTarget>` APIs, or simply take a look at provided extensions to create your own ones.

## Built-in converters

### Implicits

If no converter is given, an default one will be chosen from the registered ones (with `Converters.Register<TSource,TTarget>(converter)`).

* **Shared**
  * `<float, double>` : casting value
  * `<double, float>` : casting value
  * `<long, DateTime>` : from a millisecond timestamp to a datetime.
* **iOS**
  * `<int,nint>` : casting value
  * `<uint,nuint>` : casting value
  * `<float,nfloat>` : casting value
  * `<double,nfloat>` : casting value
  * `<int,nfloat>` : casting value
  * `<DateTime,NSDate>` : from managed type to native one
  * `<int,UIColor>` : an hexadecimal raw value from `0xAARRGGBB` to a native color
  * `<string,UIColor>` : an hexadecimal text value from `"#AARRGGBB"` to a native color
  * `<string,UIImage>` : from a bundle image name to an image instance

### Explicit

Specific converter can be used when binding, a several common converters are available.

* **Shared**
  * `Converters.Identity<T>()` :  creates a `IConverter<T,T>` that returns the given value in both ways.
  * `Converters.Invert` :  a `IConverter<bool,bool>` that inverts the given boolean value.
  * `Converters.Uppercase` :  a `IConverter<string,string>` that change the given string to uppercase.
  * `Converters.Lowercase` :  a `IConverter<string,string>` that change the given string to lowercase.
  * `new RelayConverter<TSource,TTarget>(...)` : an easy way to implement a converter from lambdas.
* **iOS**
  * `PlatformConverters.AsyncStringToCachedImage(TimeSpan expiration)` : at first request ,downloads image from http location and stores it into local storage. The next times (until the expiration date is reached), the cached image will be returned.

## Built-in sources

Wires provides also helpers for binding collection sources to `UITableView`*(iOS)*, `UICollectionView`*(iOS)* and `ListView`*(Android)*.

```csharp
this.ViewModel.Bind(this.tableView).Source<RedditViewModel, RedditViewModel.ItemViewModel, PostTableCell>(vm => vm.Simple, (post, index, cell) => cell.ViewModel = post, heightForItem: (c) => 88);
this.ViewModel.Bind(this.tableView).Source<RedditViewModel,string, RedditViewModel.ItemViewModel,PostTableHeader, PostTableCell>(vm => vm.Grouped, (section, index, cell) => cell.ViewModel = section, (post, index, cell) => cell.ViewModel = post, selectItemCommand, (headerIndex) => 68, (cellIndex) => 88);
```

```csharp
this.ViewModel.Bind(this.collectionView).Source<RedditViewModel, RedditViewModel.ItemViewModel, PostCollectionCell>(vm => vm.Simple, (post, index, cell) => cell.ViewModel = post);
this.ViewModel.Bind(this.collectionView).Source<RedditViewModel, string, RedditViewModel.ItemViewModel, PostCollectionHeader, PostCollectionCell>(vm => vm.Grouped, (section, index, cell) => cell.ViewModel = section, (post, index, cell) => cell.ViewModel = post, selectItemCommand);
```

Take a look at samples to see it in action.

## Unbinding

In most cases, you don't have to worry about unbinding because **Wires** purges all bindings regulary if sources of targets have been garbage collected.

But if you reuse a view, and want to update bindings you have to remove the previous bindings before. It is a common with recycling collection patterns (UITableViews, UICollectionViews, Adapters).

This is available through `Unbind(this TSsource, params object[] targets)` extension.

```csharp
this.viewmodel.Unbind(this.textfield, this.image, this.title)
```

## WeakEventHandlers

If you want to observe an event without keeping a strong reference to your subscriber, use a `WeakEventHandler` instead. You don't have to worry about unscription anymore in most cases!

```csharp
public override void ViewDidLoad()
{
	base.ViewDidLoad();
	this.ViewModel = new RedditViewModel();
	this.ViewModel.AddWeakHandler<PropertyChangedEventArgs>(nameof(INotifyPropertyChanged.PropertyChanged), this.OnViewModelPropertyChanged);
}

private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
{
	if(args == nameof(this.ViewModel.Title)
	{
	  // ...
	}
}
```

## Roadmap / Ideas

* Improve architecture
* Improve tests
* Android extensions
* Bindable adapters on Android
* Cleaner code
* More documentation
* Trottling functiunalities
* Command parameters

### Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

MIT © [Aloïs Deniel](http://aloisdeniel.github.io)