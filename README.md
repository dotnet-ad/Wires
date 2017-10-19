![Logo](./Documentation/Logo.png)

[![NuGet](https://img.shields.io/nuget/v/Wires.svg?label=NuGet)](https://www.nuget.org/packages/Wires/) [![Donate](https://img.shields.io/badge/donate-paypal-yellow.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ZJZKXPPGBKKAY&lc=US&item_name=GitHub&item_number=0000001&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted)

**Wires** is a simple *binding* library for frameworks that doesn't have built-in mecanisms. Many choices have been made to have a restrictive base API. A wide set of extensions are also packaged for Xamarin.iOS and Xamarin.Android.

## Why ?

Several other solutions exist, but I've experienced a **lot** of memory issues with these : that's why I've decided to initiate my own binding library.

## Install

Available on [NuGet](https://www.nuget.org/packages/Wires/).

## Quickstart

### iOS

To bind data to components with extensions, simply use those fluent APIs :

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
this.ViewModel
		.Bind(this.label)
			.TextColor(vm => vm.IsValid, new RelayConverter<bool,UIColor>(x => x ? UIColor.Green : UIColor.Red));
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

You also observe a property with `ObserveProperty` : the given action will be invoked and again each time the property changes.

```csharp
this.ViewModel.Bind(this.label).ObserveProperty(vm => vm.Title, (vm,label,title) => { label.Text = title; });
```
If you wish to bind a sub-property, use the `SubBind`.

```csharp
this.ViewModel.Bind(this.label)
			     .Text(vm => vm.Title)
			     .SubBind(vm => vm.ExecuteCommand, sub => sub.Visible(c => c.IsExecuting);
```


For more advanced options see `Binder<TSource,TTarget>` APIs, or simply take a look at provided extensions to create your own ones.

## Built-in converters

**Wires** relies on [Transmute](https://github.com/aloisdeniel/Transmute) converters for converting values beetween sources and targets.

## Built-in sources

Wires provides also common helpers for binding simple collection sources to `UITableView`*(iOS)*, `UICollectionView`*(iOS)* and `RecycleView`*(Android)*.

You can first describe your `CollectionSource<TViewModel>` from your shared code.

```csharp
var collection = new CollectionSource<RedditViewModel>(this);
collection.WithSections<TSection,TItem>("cell", "header", vm => vm.Items, (item) => item.Status, null);
```

```csharp
var collection = new CollectionSource<RedditViewModel>(this)
collection.WithSection()
			  .WithHeader("header", vm => "Section 1")
			  .WithCells("cell", vm => vm.Items1 );
		    .WithSection()
			  .WithHeader("header", vm => "Section 2")
			  .WithCell("cell", vm => vm.Item21 )
			  .WithCell("cell", vm => vm.Item22 )
			  .WithCells("cell", vm => vm.Items23to26 )
			  .WithCell("cell", vm => vm.Item27 )
			  .WithFooter("footer", vm => "End");
```

And then bind it like any other property with the view extensions on iOS.

```csharp
this.ViewModel
			.Bind(this.tableView)
				.Source(vm => vm.Items, (vm,v,c) =>
					{
						c.RegisterCellView<PostTableCell>("cell", 44);
						c.RegisterHeaderView<PostTableHeader>("header", 88);
					});
```

Its the same for Android!

```csharp
this.ViewModel
			.Bind(this.recyclerview)
				.Source(vm => vm.Items, (vm,v,c) =>
					{
						c.RegisterCellView<PostCellViewHolder>("cell", 44);
						c.RegisterHeaderView<PostHeaderViewHolder>("header", 88);
					});
```

Be sure that your `UITableViewCell` and `RecyclerView.ViewHolder` are implementing `Wires.IView` and update the view on `ViewModel` setter view. Your `RecyclerView.ViewHolder` should also have only one constructor with a `ViewGroup` as only input parameter.

Take a look at samples to see it in action.

## Unbinding

In most cases, you don't have to worry about unbinding because **Wires** purges all bindings regulary if sources of targets have been garbage collected.

But if you reuse a view, and want to update bindings you have to remove the previous bindings before. It is a common with recycling collection patterns (UITableViews, UICollectionViews, Adapters).

This is available through `Unbind(this TSsource, params object[] targets)` extension.

```csharp
this.viewmodel.Unbind(label, this.image, this.title);
```

You can also use `Rebind` method to unbind just before binding.

```csharp
this.ViewModel.Rebind(this.label).Text(v => v.Title);
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
* Dynamic sources with collection changes
* Cleaner code
* More documentation
* Trottling functiunalities
* Command parameters

### Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

MIT © [Aloïs Deniel](http://aloisdeniel.github.io)
