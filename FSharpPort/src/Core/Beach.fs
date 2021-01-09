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

    let folder (avg, acc) c =
        let av' = av avg len

        match c with
        | SandQuality v -> (av' v, { acc with SandQuality = Some c })
        | BeachCleanliness v -> (av' v, { acc with BeachCleanliness = Some c })
        | BeautifulScenery v -> (av' v, { acc with BeautifulScenery = Some c })
        | CrowdFree v -> (av' v, { acc with CrowdFree = Some c })
        | Infrastructure v -> (av' v, { acc with Infrastructure = Some c })
        | WaterVisibility v -> (av' v, { acc with WaterVisibility = Some c })
        | LitterFree v -> (av' v, { acc with LitterFree = Some c })
        | FeetFriendlyBottom v -> (av' v, { acc with FeetFriendlyBottom = Some c })
        | SeaLifeDiversity v -> (av' v, { acc with SeaLifeDiversity = Some c })
        | CoralReef v -> (av' v, { acc with CoralReef = Some c })
        | Snorkeling v -> (av' v, { acc with Snorkeling = Some c })
        | Kayaking v -> (av' v, { acc with Kayaking = Some c })
        | Walking v -> (av' v, { acc with Walking = Some c })
        | Camping v -> (av' v, { acc with Camping = Some c })
        | LongTermStay v -> (av' v, { acc with LongTermStay = Some c })

    let (avg, t) =
        List.fold folder (None, Score.Default) criteria

    let result = { t with Average = avg }

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
