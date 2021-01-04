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
    type T = private {
        Average: V option
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
        LongTermStay: V option
    }
    let empty = {
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
        LongTermStay = None
    }
    let create scores =
        let t = empty
        let av c (l: int) (v: float) =
            match c with
                | Some currVal -> (currVal + v) / (l |> float)
                | None _ -> v
        let rec assign scores acc avg =
            let len = List.length scores
            let av2 = av avg len
            match scores with
            | [] -> { acc with Average = avg }
            | head :: tail -> 
                match head with
                    | Average _ -> assign tail t avg
                    | SandQuality v -> assign tail { t with SandQuality = Some head } (av2 v)
                    | BeachCleanliness _ -> assign tail { t with BeachCleanliness = Some head } 
                    | BeautifulScenery _ -> assign tail { t with BeautifulScenery = Some head } 
                    | CrowdFree _ -> assign tail { t with CrowdFree = Some head } 
                    | Infrastructure _ -> assign tail { t with Infrastructure = Some head } 
                    | WaterVisibility _ -> assign tail { t with WaterVisibility = Some head } 
                    | LitterFree _ -> assign tail { t with LitterFree = Some head } 
                    | FeetFriendlyBottom _ -> assign tail { t with FeetFriendlyBottom = Some head } 
                    | SeaLifeDiversity _ -> assign tail { t with SeaLifeDiversity = Some head } 
                    | CoralReef _ -> assign tail { t with CoralReef = Some head } 
                    | Snorkeling _ -> assign tail { t with Snorkeling = Some head } 
                    | Kayaking _ -> assign tail { t with Kayaking = Some head } 
                    | Walking _ -> assign tail { t with Walking = Some head } 
                    | Camping _ -> assign tail { t with Camping = Some head } 
                    | LongTermStay _ -> assign tail { t with LongTermStay = Some head } 
        let result = assign scores t None
        result

module Place = 
    type Continent = Continent of string
    type WaterBody = WaterBody of string
    type Country = Country of string option
    type L1 = L1 of string option
    type L2 = L2 of string option
    type L3 = L3 of string option
    type L4 = L4 of string option
    type T = {
        Continent: Continent
        WaterBody: WaterBody
        Country: Country
        L1: L1
        L2: L2
        L3: L3
        L4: L4
    }

// module Beach = 
//     open System

//     type Name = Name of string
//     type Coordinates = Coordinates of string
//     type Beach = private {
//         Id: Guid
//         Name: Name
//         Place: Place.T
//         Coordinates: Coordinates
//         Score: Score.T
//         AddedBy: Guid
//         CreatedAt: DateTime
//     }
//     let create name place coords score addedBy = 
//         {
//             Id = Guid.NewGuid()
//             Place = place
//             Coordinates = coords
//             Score = score
//             AddedBy = addedBy
//             CreatedAt = DateTime.UtcNow()
//         }