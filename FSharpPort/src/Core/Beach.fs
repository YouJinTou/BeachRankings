module BR.Core.Beach

open System

type Name = Name of string
type Coordinates = Coordinates of string

type Place =
    { Continent: string
      WaterBody: string }

type T =
    private
        { Id: Guid
          Name: Name
          Place: Place
          Coordinates: Coordinates
          Score: Score.T
          AddedBy: Guid
          CreatedAt: DateTime }

let create name place coords score addedBy =
    { Id = Guid.NewGuid()
      Name = name
      Place = place
      Coordinates = coords
      Score = score
      AddedBy = addedBy
      CreatedAt = DateTime.UtcNow }
