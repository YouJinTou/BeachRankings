module Commands

type AddBeach =
    { Name: string
      Description: string option
      Continent: string
      WaterBody: string
      Country: string option
      L1: string option
      L2: string option
      L3: string option
      L4: string option
      Coordinates: string option
      SandQuality: float option
      BeachCleanliness: float option
      BeautifulScenery: float option
      CrowdFree: float option
      Infrastructure: float option
      WaterVisibility: float option
      LitterFree: float option
      FeetFriendlyBottom: float option
      SeaLifeDiversity: float option
      CoralReef: float option
      Snorkeling: float option
      Kayaking: float option
      Walking: float option
      Camping: float option
      LongTermStay: float option
      AddedBy: System.Guid }

type Command = AddBeach of AddBeach
