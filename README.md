# I18N

This packages purpose is to handle I18N with json files.

## Example json file

For the localizer to work, the JSON resource file must be in the following format:

```json
[
   {
      "Key": "Name",
      "LocalizedValues":[
         {"en": "Name"}
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
                     { typeof(MycLASS), "BrokenResources" }
                  };

var withExternalSources = new JsonLocalizer(additionalPaths: additional);

```

This will tell the localizer to find the required folder in the passed assembly.

## Next steps

Better configuration for Dependency Injection using Factory.
