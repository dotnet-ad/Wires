![Logo](./Documentation/Logo.png)

Wires is a simple binding library for frameworks that doesn't have built-in binding mecanisms. Many choices have been made to have a restrictive base API. A wide set of extensions are also package for Xamarin.iOS and Xamarin.Android.

## Why ?

Several other solutions exists, but I've experienced a **lot** of memory issues with those because they are mainly based on lambda expressions. That's why Wire keeps basic concepts at its core and limits usag of lambda expressions.

## Install

Available on NuGet

[![NuGet](https://img.shields.io/nuget/v/Wires.svg?label=NuGet)](https://www.nuget.org/packages/Wires/)

## Quickstart

### iOS

To bind data to components :

```csharp
viewmodel.BindHidden(view,nameof(viewmodel.IsHidden);
viewmodel.BindText(label,nameof(viewmodel.Title));
viewmodel.BindTextColor(label,nameof(viewmodel.ForegroundColor));
viewmodel.BindText(field,nameof(viewmodel.ForegroundColor));
viewmodel.UpdateCommand.Bind(button));
```

Value converters can also be used with an `IConverter<TSource,TTarget>` implementation, or a lambda expression :

```csharp
viewmodel.BindHidden(view,nameof(viewmodel.IsVisible), Converters.Invert);
viewmodel.BindTextColor<bool>(label,nameof(viewmodel.ForegroundColor), x => x ? UIColor.Green : UIColor.Red);
```

## Basic APIs

If included binding extensions don't feet your needs, you can use the basic APIs :

```csharp
viewmodel.BindOneWay(nameof(viewmodel.SourceProperty), target, nameof(target.TargetProperty), converter);
viewmodel.BindTwoWay<Target, TProperty, string, EventArgs>((nameof(viewmodel.SourceProperty), target, nameof(target.TargetProperty), converter);
```

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