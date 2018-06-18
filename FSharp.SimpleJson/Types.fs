namespace SimpleJson 

open System
open FSharp.Reflection

[<AutoOpen>]
module Types = 

    /// A type representing Javascript Object Notation
    type Json = 
        | JNumber of float
        | JString of string
        | JBool of bool
        | JNull
        | JArray of Json list
        | JObject of Map<string, Json>

    /// A type that encodes type information which is easily serializable 
    type TypeInfo = 
        | Unit 
        | String  
        | Int32 
        | Bool   
        | Float  
        | Long 
        | DateTime 
        | BigInt 
        | Guid 
        | Object of Type
        | Option of TypeInfo 
        | List of TypeInfo 
        | Set of TypeInfo 
        | Array of TypeInfo
        | Tuple of TypeInfo [ ] 
        | Map of key:TypeInfo * value:TypeInfo 
        | Func of TypeInfo [ ] 
        | Record of (string  * TypeInfo) [ ] * Type  
        | Union of (string * UnionCaseInfo *  TypeInfo [ ]) [ ] * Type     