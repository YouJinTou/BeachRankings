module Score

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

type T =
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

type ValidCriteria = ValidCriteria of list<Criterion>

let validate criteria =
    let validate' score criterion =
        if score < 0. || score > 10. then
            Error((string criterion) + " must be between 0 and 10.")
        else
            Ok criterion

    let bind list validationResult =
        match validationResult with
        | Ok criterion -> criterion :: list |> Ok
        | Error e -> Error e

    let folder agg c =
        let aggregate list =
            let validate'' v c = validate' v c |> bind list

            match c with
            | SandQuality v -> validate'' v c
            | BeachCleanliness v -> validate'' v c
            | BeautifulScenery v -> validate'' v c
            | CrowdFree v -> validate'' v c
            | Infrastructure v -> validate'' v c
            | WaterVisibility v -> validate'' v c
            | LitterFree v -> validate'' v c
            | FeetFriendlyBottom v -> validate'' v c
            | SeaLifeDiversity v -> validate'' v c
            | CoralReef v -> validate'' v c
            | Snorkeling v -> validate'' v c
            | Kayaking v -> validate'' v c
            | Walking v -> validate'' v c
            | Camping v -> validate'' v c
            | LongTermStay v -> validate'' v c

        match agg with
        | Ok list -> aggregate list
        | Error e -> Error e

    match List.fold folder (Ok []) criteria with
    | Ok list -> Ok(ValidCriteria list)
    | Error e -> Error e

let create criteria =
    let (ValidCriteria validCriteria) = criteria

    let av currentAverage len v =
        match currentAverage with
        | Some f -> Some((f + v) / (float len))
        | None _ -> Some v

    let folder acc c =
        let av' =
            av acc.Average (List.length validCriteria)

        let setAvg score acc = { acc with Average = av' score }

        match c with
        | SandQuality v -> setAvg v { acc with SandQuality = Some c }
        | BeachCleanliness v -> setAvg v { acc with BeachCleanliness = Some c }
        | BeautifulScenery v -> setAvg v { acc with BeautifulScenery = Some c }
        | CrowdFree v -> setAvg v { acc with CrowdFree = Some c }
        | Infrastructure v -> setAvg v { acc with Infrastructure = Some c }
        | WaterVisibility v -> setAvg v { acc with WaterVisibility = Some c }
        | LitterFree v -> setAvg v { acc with LitterFree = Some c }
        | FeetFriendlyBottom v -> setAvg v { acc with FeetFriendlyBottom = Some c }
        | SeaLifeDiversity v -> setAvg v { acc with SeaLifeDiversity = Some c }
        | CoralReef v -> setAvg v { acc with CoralReef = Some c }
        | Snorkeling v -> setAvg v { acc with Snorkeling = Some c }
        | Kayaking v -> setAvg v { acc with Kayaking = Some c }
        | Walking v -> setAvg v { acc with Walking = Some c }
        | Camping v -> setAvg v { acc with Camping = Some c }
        | LongTermStay v -> setAvg v { acc with LongTermStay = Some c }

    let result = List.fold folder T.Default validCriteria

    result
