module BR.Core.Beach

open System

type Criterion =
    | SandQuality of float
    | BeachCleanliness of float
    | BeautifulScenery of float
    | CrowdFree of float
    | Infrastructure of float
    | WaterVisibility of float
    | LitterFree of float
    | FeetFriendlyBottom of float
    | SeaLifeDiversity of float
    | CoralReef of float
    | Snorkeling of float
    | Kayaking of float
    | Walking of float
    | Camping of float
    | LongTermStay of float

type Score =
    private
        { Average: float option
          SandQuality: Criterion option
          BeachCleanliness: Criterion option
          BeautifulScenery: Criterion option
          CrowdFree: Criterion option
          Infrastructure: Criterion option
          WaterVisibility: Criterion option
          LitterFree: Criterion option
          FeetFriendlyBottom: Criterion option
          SeaLifeDiversity: Criterion option
          CoralReef: Criterion option
          Snorkeling: Criterion option
          Kayaking: Criterion option
          Walking: Criterion option
          Camping: Criterion option
          LongTermStay: Criterion option }
    static member Default =
        { Average = None
          SandQuality = None
          BeachCleanliness = None
          BeautifulScenery = None
          CrowdFree = None
          Infrastructure = None
          WaterVisibility = None
          LitterFree = None
          FeetFriendlyBottom = None
          SeaLifeDiversity = None
          CoralReef = None
          Snorkeling = None
          Kayaking = None
          Walking = None
          Camping = None
          LongTermStay = None }

let createScore criteria =
    let validate score =
        if score < 0. || score > 10. then
            Error "Score must be between 0 and 10."
        else
            Ok score

    let bind continueWith score =
        match score with
        | Ok s -> continueWith s
        | Error s -> Error s

    let av currentAverage len v =
        match currentAverage with
        | Some f -> Some((f + v) / (float len))
        | None _ -> Some v

    let folder acc c =
        let aggregate acc =
            let av' = av acc.Average (List.length criteria)

            let update acc =
                fun score -> Ok { acc with Average = av' score }

            let vbu score acc = validate score |> bind (update acc)

            match c with
            | SandQuality v -> vbu v { acc with SandQuality = Some c }
            | BeachCleanliness v -> vbu v { acc with BeachCleanliness = Some c }
            | BeautifulScenery v -> vbu v { acc with BeautifulScenery = Some c }
            | CrowdFree v -> vbu v { acc with CrowdFree = Some c }
            | Infrastructure v -> vbu v { acc with Infrastructure = Some c }
            | WaterVisibility v -> vbu v { acc with WaterVisibility = Some c }
            | LitterFree v -> vbu v { acc with LitterFree = Some c }
            | FeetFriendlyBottom v -> vbu v { acc with FeetFriendlyBottom = Some c }
            | SeaLifeDiversity v -> vbu v { acc with SeaLifeDiversity = Some c }
            | CoralReef v -> vbu v { acc with CoralReef = Some c }
            | Snorkeling v -> vbu v { acc with Snorkeling = Some c }
            | Kayaking v -> vbu v { acc with Kayaking = Some c }
            | Walking v -> vbu v { acc with Walking = Some c }
            | Camping v -> vbu v { acc with Camping = Some c }
            | LongTermStay v -> vbu v { acc with LongTermStay = Some c }

        match acc with
        | Error e -> Error e
        | Ok acc -> aggregate acc

    let result =
        List.fold folder (Ok Score.Default) criteria

    result

type Name = Name of string
type Coordinates = Coordinates of string

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
          Name: Name
          Place: Place
          Coordinates: Coordinates
          Score: Score
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
