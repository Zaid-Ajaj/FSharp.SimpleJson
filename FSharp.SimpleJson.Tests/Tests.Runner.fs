module Runner

open Expecto
open Expecto.Logging
open SimpleJson

let testConfig =  
    { Expecto.Tests.defaultConfig with 
        parallelWorkers = 1
        verbosity = LogLevel.Debug }

let pass() = Expect.equal true true ""
let fail() = Expect.equal false true ""

let simpleJsonTests = 
    testList "SimpleJson" [ 
        testCase "parse works" <| fun () ->
            "{ \"Name\" : \"John\", \"Age\" : 20, \"HasValue\": false, \"Values\":[false,null,5] }"
            |> SimpleJson.parse  
            |> fun object -> 
                match object with  
                | JObject map -> 
                    let values = List.choose (fun key -> Map.tryFind key map) [ "Name"; "Age"; "HasValue"; "Values" ]
                    match values with 
                    | [ JString "John"; 
                        JNumber 20.0; 
                        JBool false;
                        JArray [ JBool false; JNull; JNumber 5.0 ] ] -> pass()
                    | _ -> fail()
                | _ -> fail() 
    ]

[<EntryPoint>]
let main argv = runTests testConfig simpleJsonTests