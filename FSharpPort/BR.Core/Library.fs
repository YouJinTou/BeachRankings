namespace BR.Core

module Score =
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
        static member Default = { 
          Average = None
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
        let t = T.Default

        let av acc (len: int) (v: float) =
            match acc with
            | Some f -> Some ((f + v) / (len |> float))
            | None _ -> Some v
        let average x = 
            match x with
            | Some v -> Some (Average v)
            | None _ -> None

        let rec assign scores acc avg =
            let len = List.length scores
            let av2 = av avg len

            match scores with
            | [] -> { acc with Average = average avg }
            | head :: tail ->
                match head with
                | Average _ -> assign tail t avg
                | SandQuality v -> assign tail { t with SandQuality = Some head } (av2 v)
                | BeachCleanliness v -> assign tail { t with BeachCleanliness = Some head } (av2 v)
                | BeautifulScenery v -> assign tail { t with BeautifulScenery = Some head } (av2 v)
                | CrowdFree v -> assign tail { t with CrowdFree = Some head } (av2 v)
                | Infrastructure v -> assign tail { t with Infrastructure = Some head } (av2 v)
                | WaterVisibility v -> assign tail { t with WaterVisibility = Some head } (av2 v)
                | LitterFree v -> assign tail { t with LitterFree = Some head } (av2 v)
                | FeetFriendlyBottom v -> assign tail { t with FeetFriendlyBottom = Some head } (av2 v)
                | SeaLifeDiversity v -> assign tail { t with SeaLifeDiversity = Some head } (av2 v)
                | CoralReef v -> assign tail { t with CoralReef = Some head } (av2 v)
                | Snorkeling v -> assign tail { t with Snorkeling = Some head } (av2 v)
                | Kayaking v -> assign tail { t with Kayaking = Some head } (av2 v)
                | Walking v -> assign tail { t with Walking = Some head } (av2 v)
                | Camping v -> assign tail { t with Camping = Some head } (av2 v)
                | LongTermStay v -> assign tail { t with LongTermStay = Some head } (av2 v)

        let result = assign scores t None
        result

module Beach =
    open System

    type Place =
        { Continent: string
          WaterBody: string}
    type Name = Name of string
    type Coordinates = Coordinates of string
    type Beach = private {
        Id: Guid
        Name: Name
        Place: Place
        Coordinates: Coordinates
        Score: Score.T
        AddedBy: Guid
        CreatedAt: DateTime
    }
    let create name place coords score addedBy =
        {
            Id = Guid.NewGuid()
            Name = name
            Place = place
            Coordinates = coords
            Score = score
            AddedBy = addedBy
            CreatedAt = DateTime.UtcNow
        }

