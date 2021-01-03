namespace BR.Core

module Score =
    type Average = private Average of float
    type SandQuality = SandQuality of float
    type BeachCleanliness = BeachCleanliness of float
    type BeautifulScenery = BeautifulScenery of float
    type CrowdFree = CrowdFree of float
    type Infrastructure = Infrastructure of float
    type WaterVisibility = WaterVisibility of float
    type LitterFree = LitterFree of float
    type FeetFriendlyBottom = FeetFriendlyBottom of float
    type SeaLifeDiversity = SeaLifeDiversity of float
    type CoralReef = CoralReef of float
    type Snorkeling = Snorkeling of float
    type Kayaking = Kayaking of float
    type Walking = Walking of float
    type Camping = Camping of float
    type LongTermStay = LongTermStay of float
    type T = private {
        Average: Average option
        SandQuality: SandQuality option
        BeachCleanliness: BeachCleanliness option
        BeautifulScenery: BeautifulScenery option
        CrowdFree: CrowdFree option
        Infrastructure: Infrastructure option
        WaterVisibility: WaterVisibility option
        LitterFree: LitterFree option
        FeetFriendlyBottom: FeetFriendlyBottom option
        SeaLifeDiversity: SeaLifeDiversity option
        CoralReef: CoralReef option
        Snorkeling: Snorkeling option
        Kayaking: Kayaking option
        Walking: Walking option
        Camping: Camping option
        LongTermStay: LongTermStay option
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
        let rec assign scores =
            match scores with
            | [] -> t
            | head :: rest -> 
                match head with
                | SandQuality -> { t with SandQuality = head } :: assign rest 
                | BeachCleanliness -> { t with BeachCleanliness = head }  :: assign rest 
                | BeautifulScenery -> { t with BeautifulScenery = head }  :: assign rest 
                | CrowdFree -> { t with CrowdFree = head }  :: assign rest 
                | Infrastructure -> { t with Infrastructure = head }  :: assign rest 
                | WaterVisibility -> { t with WaterVisibility = head }  :: assign rest 
                | LitterFree -> { t with LitterFree = head }  :: assign rest 
                | FeetFriendlyBottom -> { t with FeetFriendlyBottom = head }  :: assign rest 
                | SeaLifeDiversity -> { t with SeaLifeDiversity = head }  :: assign rest 
                | CoralReef -> { t with CoralReef = head }  :: assign rest 
                | Snorkeling -> { t with Snorkeling = head }  :: assign rest 
                | Kayaking -> { t with Kayaking = head }  :: assign rest 
                | Walking -> { t with Walking = head }  :: assign rest 
                | Camping -> { t with Camping = head }  :: assign rest 
                | LongTermStay -> { t with LongTermStay = head }  :: assign rest 
        assign scores
        

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

module Beach = 
    open System

    type Name = Name of string
    type Coordinates = Coordinates of string
    type Beach = private {
        Id: Guid
        Name: Name
        Place: Place.T
        Coordinates: Coordinates
        Score: Score.T
        AddedBy: Guid
        CreatedAt: DateTime
    }
    let create name place coords score addedBy = 
        {
            Id = Guid.NewGuid()
            Place = place
            Coordinates = coords
            Score = score
            AddedBy = addedBy
            CreatedAt = DateTime.UtcNow()
        }