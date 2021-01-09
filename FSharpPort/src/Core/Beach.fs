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
    let av currentAverage len v =
        match currentAverage with
        | Some f -> Some((f + v) / (float len))
        | None _ -> Some v

    let len = List.length criteria

    let folder acc c =
        let av' = av acc.Average len

        match c with
        | SandQuality v -> { acc with SandQuality = Some c; Average = av' v }
        | BeachCleanliness v ->  { acc with BeachCleanliness = Some c; Average = av' v }
        | BeautifulScenery v ->  { acc with BeautifulScenery = Some c; Average = av' v }
        | CrowdFree v ->  { acc with CrowdFree = Some c; Average = av' v }
        | Infrastructure v ->  { acc with Infrastructure = Some c; Average = av' v }
        | WaterVisibility v ->  { acc with WaterVisibility = Some c; Average = av' v }
        | LitterFree v ->  { acc with LitterFree = Some c; Average = av' v }
        | FeetFriendlyBottom v ->  { acc with FeetFriendlyBottom = Some c; Average = av' v }
        | SeaLifeDiversity v ->  { acc with SeaLifeDiversity = Some c; Average = av' v }
        | CoralReef v ->  { acc with CoralReef = Some c; Average = av' v }
        | Snorkeling v ->  { acc with Snorkeling = Some c; Average = av' v }
        | Kayaking v ->  { acc with Kayaking = Some c; Average = av' v }
        | Walking v ->  { acc with Walking = Some c; Average = av' v }
        | Camping v ->  { acc with Camping = Some c; Average = av' v }
        | LongTermStay v ->  { acc with LongTermStay = Some c; Average = av' v }

    let result =
        List.fold folder Score.Default criteria

    result

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
