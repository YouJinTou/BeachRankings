namespace BR.Core

module Score =
    let score s = 
        if s = Option.None then None
        elif s < 0.0 || s > 10.0 then None
        else Some s
    type Average = private Average of float
    type SandQuality = private SandQuality of float
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
    let create sq bc bs cf i wv lf ffb sld cr s k w c lts =
        {
            SandQuality = sq
            BeachCleanliness = bc
            BeautifulScenery = bs
            CrowdFree = cf
            Infrastructure = i
            WaterVisibility = wv
            LitterFree = lf
            FeetFriendlyBottom = ffb
            SeaLifeDiversity = sld
            CoralReef = cr
            Snorkeling = s
            Kayaking = k
            Walking = w
            Camping = c
            LongTermStay = lts
        }

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