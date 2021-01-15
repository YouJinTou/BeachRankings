module Beach

open Funcs
open System
open System.Text.RegularExpressions

module Name =
    type T = T of string

    type ValidName = private ValidName of string

    let (|HasBadCharacters|_|) name =
        Regex.IsMatch(name, "[^a-zA-Z0-9 -]")
        |> ifTrueThen HasBadCharacters

    let validate name =
        let (T n) = name

        match n with
        | NullOrEmpty -> Error "Name cannot be empty."
        | WhiteSpace -> Error "Name cannot be empty."
        | MaxLength 100 -> Error "Name cannot be more than 100 characters."
        | HasBadCharacters -> Error "Name can only contain letters, numbers, spaces, and hyphens."
        | n -> ValidName n |> Ok

module Coordinates =
    type T = T of string option

    type Coords =
        private
        | FullCoordinates of float * float
        | EmptyCoordinates

    let (|CommaSeparated|_|) coords =
        let pattern =
            "^([-+]?)([\d]{1,2})(((\.)(\d+)(,)))(\s*)(([-+]?)([\d]{1,3})((\.)(\d+))?)$"

        Regex.IsMatch(coords, pattern)
        |> ifTrueThen CommaSeparated

    let parse (coords: string) =
        coords.Split [| ',' |]
        |> fun tokens -> (float tokens.[0], float tokens.[1])

    let validate coords =
        let (T c) = coords

        match c with
        | None -> Ok EmptyCoordinates
        | Some c' ->
            match c' with
            | NullOrEmpty -> Ok EmptyCoordinates
            | WhiteSpace -> Ok EmptyCoordinates
            | MaxLength 27 -> Error "Max coordinates length is 27, separator included."
            | CommaSeparated -> FullCoordinates(parse c') |> Ok
            | _ -> Error "Could not parse coordinates."


module Place =
    type Continent = Continent of string
    type WaterBody = WaterBody of string
    type Country = Country of string option
    type L1 = L1 of string option
    type L2 = L2 of string option
    type L3 = L3 of string option
    type L4 = L4 of string option

    type T =
        { Continent: Continent
          WaterBody: WaterBody
          Country: Country
          L1: L1
          L2: L2
          L3: L3
          L4: L4 }

module Description =
    type T = T of string option

    type ValidDescription =
        private
        | ValidDescription of string
        | EmptyDescription

    let validate description =
        let (T d) = description

        match d with
        | None -> Ok EmptyDescription
        | Some c' ->
            match c' with
            | NullOrEmpty -> Ok EmptyDescription
            | WhiteSpace -> Ok EmptyDescription
            | MaxLength 500 -> Error "Max description length is 500 symbols."
            | desc -> ValidDescription desc |> Ok

type AddedBy = AddedBy of Guid

type T =
    private
        { Id: Guid
          Name: Name.ValidName
          Description: Description.ValidDescription
          Place: Place.T
          Coordinates: Coordinates.Coords
          Score: Score.T
          AddedBy: AddedBy
          CreatedAt: DateTime }

let create id name description place coords score addedBy =
    { Id = id
      Name = name
      Description = description
      Place = place
      Coordinates = coords
      Score = score
      AddedBy = addedBy
      CreatedAt = DateTime.UtcNow }
