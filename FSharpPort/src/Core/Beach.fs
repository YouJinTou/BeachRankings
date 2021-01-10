module Beach

open Funcs

open System

module Name =
    open System.Text.RegularExpressions

    type T = T of string

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
        | n -> T n |> Ok

type Coordinates = Coordinates of string
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
          Name: Name.T
          Place: Place
          Coordinates: Coordinates
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
