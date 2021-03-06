module Commands

type AddBeach =
    { Id: System.Guid
      Name: string
      Description: string option
      Continent: string
      WaterBody: string
      Country: string option
      L1: string option
      L2: string option
      L3: string option
      L4: string option
      Coordinates: string option
      AddedBy: System.Guid }

type UpdateBeach =
    { Id: System.Guid
      Name: string
      Description: string option
      Continent: string
      WaterBody: string
      Country: string option
      L1: string option
      L2: string option
      L3: string option
      L4: string option
      Coordinates: string option
      UpdatedBy: System.Guid }

type DeleteBeach =
    { Id: System.Guid
      InitiatedBy: System.Guid }

type Command =
    | AddBeach of AddBeach
    | UpdateBeach of UpdateBeach
    | DeleteBeach of DeleteBeach

type Event =
    | BeachAdded of AddBeach
    | BeachUpdated of UpdateBeach
    | BeachDeleted of DeleteBeach
