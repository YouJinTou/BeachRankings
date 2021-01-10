module BR.Core.Funcs

open System
open System.Text.RegularExpressions

let ifTrueThen x =
    function
    | true -> Some x
    | false -> None

let (|NullOrEmpty|_|) =
    String.IsNullOrEmpty >> ifTrueThen NullOrEmpty

let (|WhiteSpace|_|) =
    String.IsNullOrWhiteSpace >> ifTrueThen WhiteSpace

let (|MaxLength|_|) len str =
    (String.length str) > len |> ifTrueThen MaxLength

let (|HasLetters|_|) str =
    Regex.IsMatch(str, "(?=.*[a-zA-Z])")
    |> ifTrueThen HasLetters
