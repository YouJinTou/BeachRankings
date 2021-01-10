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
    type T = T of string

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
        | NullOrEmpty -> Ok EmptyCoordinates
        | WhiteSpace -> Ok EmptyCoordinates
        | MaxLength 27 -> Error "Max coordinates length is 27, separator included."
        | CommaSeparated -> FullCoordinates(parse c) |> Ok
        | _ -> Error "Could not parse coordinates."

type AddedBy = AddedBy of Guid

type Place =
    { Continent: string
      WaterBody: string
      Country: string option
      L1: string option
      L2: string option
      L3: string option
      L4: string option }

type T =
    private
        { Id: Guid
          Name: Name.ValidName
          Place: Place
          Coordinates: Coordinates.Coords
          Score: Score.T
          AddedBy: AddedBy
          CreatedAt: DateTime }

let create name place coords score addedBy =
    { Id = Guid.NewGuid()
      Name = name
      Place = place
      Coordinates = coords
      Score = score
      AddedBy = addedBy
      CreatedAt = DateTime.UtcNow }
