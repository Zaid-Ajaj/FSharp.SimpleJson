# FSharp.SimpleJson [![Nuget](https://img.shields.io/nuget/v/FSharp.SimpleJson.svg?colorB=green)](https://www.nuget.org/packages/FSharp.SimpleJson) [![Build Status](https://travis-ci.org/Zaid-Ajaj/FSharp.SimpleJson.svg?branch=master)](https://travis-ci.org/Zaid-Ajaj/FSharp.SimpleJson)

A library for easily parsing, transforming and converting JSON in F#. Ported from [Fable.SimpleJson](https://github.com/Zaid-Ajaj/Fable.SimpleJson)

### Installation
Install from nuget using paket
```sh
paket add nuget Fable.SimpleJson --project path/to/YourProject.fsproj 
```
Make sure the references are added to your paket files
```sh
# paket.dependencies (solution-wide dependencies)
nuget FSharp.SimpleJson

# paket.refernces (project-specific dependencies)
FSharp.SimpleJson
```

### Using the library

JSON Parsing and Transformation API
```fs
open FSharp.SimpleJson 

// ... 

SimpleJson.parse : string -> Json
SimpleJson.tryParse : string -> Result<Json, string> 
SimpleJson.toString : Json -> string
SimpleJson.mapKeys : (f: string -> string) -> Json -> Json
SimpleJson.mapKeysByPath : (f: string list -> string option) -> Json -> Json
```