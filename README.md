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
this.label.Bind(this.ViewModel).Text(vm => vm.Title);
this.field.Bind(this.ViewModel).Text(vm => vm.Title);
this.image.Bind(this.ViewModel).Image(vm => vm.Illustration);
this.image.Bind(this.ViewModel).As<UIView>().Visible(vm => vm.IsActive);
this.toggleSwitch.Bind(this.ViewModel).On(vm => vm.IsActive);
this.slider.Bind(this.ViewModel).Value(vm => vm.Amount);
this.datePicker.Bind(this.ViewModel).Date(vm => vm.Birthday);
this.progressView.Bind(this.ViewModel).Progress(vm => vm.Amount);
this.activityIndicator.Bind(this.ViewModel).IsAnimating(vm => vm.IsLoading);
this.segmented.Bind(this.ViewModel).Titles(vm => vm.Sections);
this.button.Bind(this.ViewModel.LoadCommand).TouchUpInside();
```

Value converters can also be used with an `IConverter<TSource,TTarget>` implementation, or a lambda expression :

```csharp
this.label.Bind(this.ViewModel).TextColor(vm => vm.IsValid, x => x ? UIColor.Green : UIColor.Red);
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
  * `TouchUpInside` (command)
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

For more advanced options see : [./Sources/Wires/Bindings.cs](./Sources/Wires/Bindings.cs) or the provided extensions.

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

TODO

## Unbinding

In most cases, you don't have to worry about unbinding because **Wires** purges all bindings regulary if sources of targets have been garbage collected.

But if you reuse a view, and want to update bindings you have to remove the previous bindings before. It is a common with recycling collection patterns (UITableViews, UICollectionViews, Adapters).

This is available through `Unbind(this TSsource, params object[] targets)` extension.

```csharp
this.viewmodel.Unbind(this.textfield, this.image, this.title)
```

## Roadmap / Ideas

* Improve architecture
* Improve tests
* MORE extensions
* Bindable adapters on Android
* Grouped collection sources

### Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

MIT © [Aloïs Deniel](http://aloisdeniel.github.io)