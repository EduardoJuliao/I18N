# I18N

This packages purpose is to handle I18N with json files.

## Example json file

For the localizer to work, the JSON resource file must be in the following format:

```json
[
   {
      "Key": "Name",
      "LocalizedValues":[
         {"en": "Name"},
         {"pt": "Nome"}
      ]
   },
   {
      "Key": "Age",
      "LocalizedValues":[
         {"en": "Age"},
         {"pt": "Idade"}
      ]
   }
]
```

Which must consist in one or more unique keys and its localized values.

## Creating a localizer

It can be used in Dependency Injection or can be created manually.

### Warning

The creating process is rather expensive as it'll load all keys/values into memory.

## Handling folders

When creating a Localizer, you can set new resource folder for it to find.

To do this, you need to pass a type and the root folder where the resources are located.

```csharp
var additional = new Dictionary<Type, string>
                  {
                     { typeof(MyClass), "My Resource Folder" },
                     { typeof(MyAnotherClass), "My Resource Folder/Even Handles sub folders" }
                  };

var withExternalSources = new JsonLocalizer(additionalPaths: additional);

```

This will tell the localizer to find the required folder in the passed assembly.

## Using

```csharp
// use it in DI as singleton
public void ConfigureServices(IServiceCollection services)
{
   // Other configurations ...
   services.AddSingleton<JsonLocalizer>();
}
```

To use in the application

```csharp

private readonly JsonLocalizer _localizer;

public class MySampleClass(JsonLocalizer localizer)
{
   _localizer = localizer;
}

public string GetLocalizedMessage()
{
   return _localizer["MyAppResource:MyKey"];
}

```

### Breakdown

To find the localized message, the `JsonLocalizer` uses 3 parameters, 2 mandatory and 1 optional, 
in the following format:

```csharp
"ResourceFileName:Key:Language"
```

#### Resource File Name

The resource file name MUST be unique, if the system finds a duplicate, will throw an exception.
Also, the name is everything before the first `'.'`.

Eg.:

```csharp

| File Name                  | Resource Name |
| -------------------------- | ------------- |
| MyResource.json            | MyResource    |
| MyApp.Resource.json        | MyApp         |
| MyApp-Errors.Resource.json | MyApp-Errors  |
| MyApp.Errors.Resource.json | MyApp         |

```

#### Key

The second parameter is the key. With it, the JsonLocalizer will try to find it inside the collection.

#### Culture

The last parameter is the culture, if not informed, will use the `CultureInfo.CurrentCulture` value.

#### Things to consider

At the moment, the `':'` char cannot be used in the file name, key and localized values key, as it relay on it to identify what is what.

## Next steps

Better configuration for Dependency Injection using Factory.
