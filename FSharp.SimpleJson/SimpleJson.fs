namespace SimpleJson 

open Newtonsoft
open Newtonsoft.Json.Linq
open System.Collections.Generic

module SimpleJson = 
    
    /// Parses the input string as structured JSON 
    let parse (input: string) = 
        let token = JToken.Parse(input)
        let rec fromJToken (token: JToken) = 
            match token.Type with 
            | JTokenType.Float -> JNumber (token.Value<float>())
            | JTokenType.Integer -> JNumber (token.Value<float>())
            | JTokenType.Boolean -> JBool (token.Value<bool>())
            | JTokenType.String -> JString (token.Value<string>())
            | JTokenType.Guid -> JString (token.Value<System.Guid>().ToString())
            | JTokenType.Null -> JNull 
            | JTokenType.Array -> 
                token.Values<JToken>()
                |> Seq.map fromJToken
                |> List.ofSeq 
                |> Json.JArray
            | JTokenType.Object -> 
                token.Value<IDictionary<string, JToken>>()   
                |> Seq.map (fun pair -> pair.Key, fromJToken pair.Value)
                |> List.ofSeq 
                |> Map.ofList 
                |> Json.JObject 
            | _ -> failwithf "JSON token type '%s' was not recognised" (token.Type.ToString()) 
        fromJToken token

    /// Tries to parse the input string as structured data 
    let tryParse input = 
        try Ok (parse input) 
        with | ex -> Error ex.Message

    /// Stringifies a Json object back to string representation
    let rec toString = function
        | JNull -> "null"
        | JBool true -> "true"
        | JBool false -> "false"
        | JNumber number -> string number
        | JString text -> sprintf "\"%s\"" text
        | JArray elements -> 
            elements
            |> List.map toString
            |> String.concat ","
            |> sprintf "[%s]"
        | JObject map -> 
            map 
            |> Map.toList
            |> List.map (fun (key,value) -> sprintf "\"%s\":%s" key (toString value))
            |> String.concat ","
            |> sprintf "{%s}"

    /// Transforms all keys of the objects within the Json structure
    let rec mapKeys f = function
        | JObject dictionary ->
            dictionary
            |> Map.toList
            |> List.map (fun (key, value) -> f key, mapKeys f value)
            |> Map.ofList
            |> Json.JObject
        | JArray values ->
            values
            |> List.map (mapKeys f)
            |> Json.JArray
        | otherJsonValue -> otherJsonValue

    /// Transforms keys of object selectively by path segments
    let mapKeysByPath f json =
        let rec mapKey xs = function
            | JObject dictionary ->
                dictionary
                |> Map.toList
                |> List.map (fun (key, value) ->
                    let keyPath = xs @ [key]
                    match f keyPath with
                    | Some nextKey -> nextKey, mapKey keyPath value
                    | None -> key, mapKey keyPath value)
                |> Map.ofList
                |> Json.JObject
            | JArray values ->
                values
                |> List.map (mapKey xs)
                |> Json.JArray
            | otherJsonValue -> otherJsonValue
        mapKey [] json