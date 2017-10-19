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
this.image.Bind<HomeViewModel,UIView>(this.ViewModel).Visible(vm => vm.IsActive);
this.toggleSwitch.Bind(this.ViewModel).On(vm => vm.IsActive);
this.slider.Bind(this.ViewModel).Value(vm => vm.Amount);
this.datePicker.Bind(this.ViewModel).Date(vm => vm.Birthday);
this.progressView.Bind(this.ViewModel).Progress(vm => vm.Amount);
this.activityIndicator.Bind(this.ViewModel).IsAnimating(vm => vm.IsLoading);
this.button.Bind(this.ViewModel.LoadCommand).TouchUpInside();
```

Value converters can also be used with an `IConverter<TSource,TTarget>` implementation, or a lambda expression :

```csharp
this.label.Bind(this.ViewModel).TextColor(vm => vm.IsValid, x => x ? UIColor.Green : UIColor.Red);
```

## Basic APIs

For more advanced options see : [./Sources/Wires/Bindings.cs](./Sources/Wires/Bindings.cs) or the provided extensions.

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