module BR.Core.Score

type V =
    | Average of float
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
        { Average: V option
          SandQuality: V option
          BeachCleanliness: V option
          BeautifulScenery: V option
          CrowdFree: V option
          Infrastructure: V option
          WaterVisibility: V option
          LitterFree: V option
          FeetFriendlyBottom: V option
          SeaLifeDiversity: V option
          CoralReef: V option
          Snorkeling: V option
          Kayaking: V option
          Walking: V option
          Camping: V option
          LongTermStay: V option }
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

let create scores =
    let av acc (len: int) (v: float) =
        match acc with
        | Some f -> Some((f + v) / (len |> float))
        | None _ -> Some v

    let average x =
        match x with
        | Some v -> Some(Average v)
        | None _ -> None

    let rec assign len scores t avg =
        let av2 = av avg len

        match scores with
        | [] -> { t with Average = average avg }
        | head :: tail ->
            let a2 = assign len tail
            match head with
            | Average _ -> a2 t avg
            | SandQuality v -> a2 { t with SandQuality = Some head } (av2 v)
            | BeachCleanliness v -> a2 { t with BeachCleanliness = Some head } (av2 v)
            | BeautifulScenery v -> a2 { t with BeautifulScenery = Some head } (av2 v)
            | CrowdFree v -> a2 { t with CrowdFree = Some head } (av2 v)
            | Infrastructure v -> a2 { t with Infrastructure = Some head } (av2 v)
            | WaterVisibility v -> a2 { t with WaterVisibility = Some head } (av2 v)
            | LitterFree v -> a2 { t with LitterFree = Some head } (av2 v)
            | FeetFriendlyBottom v -> a2 { t with FeetFriendlyBottom = Some head } (av2 v)
            | SeaLifeDiversity v -> a2 { t with SeaLifeDiversity = Some head } (av2 v)
            | CoralReef v -> a2 { t with CoralReef = Some head } (av2 v)
            | Snorkeling v -> a2 { t with Snorkeling = Some head } (av2 v)
            | Kayaking v -> a2 { t with Kayaking = Some head } (av2 v)
            | Walking v -> a2 { t with Walking = Some head } (av2 v)
            | Camping v -> a2 { t with Camping = Some head } (av2 v)
            | LongTermStay v -> a2 { t with LongTermStay = Some head } (av2 v)

    let result = assign (List.length scores) scores T.Default None
    result
