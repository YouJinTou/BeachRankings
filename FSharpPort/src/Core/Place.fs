module Place

type WaterBody = WaterBody of string

let waterBody waterBody =
    let (WaterBody value) = waterBody
    value

type ContinentId = private ContinentId of string
type CountryId = private CountryId of string
type L1Id = private L1Id of string
type L2Id = private L2Id of string
type L3Id = private L3Id of string
type L4Id = private L4Id of string

type Continent =
    { Id: ContinentId
      Name: string
      CountryIds: CountryId list
      WaterBodies: WaterBody list }

let createContinentId continent = ContinentId continent

let continentId continentId =
    let (ContinentId value) = continentId
    value

type Country =
    { Id: CountryId
      Name: string
      ContinentId: ContinentId
      L1Ids: L1Id list
      WaterBodies: WaterBody list }

let createCountryId continentId country =
    let (ContinentId x) = continentId
    CountryId $"%s{x}_%s{country}"

let countryId countryId =
    let (CountryId value) = countryId
    value

type L1 =
    { Id: L1Id
      Name: string
      ContinentId: ContinentId
      CountryId: CountryId
      L2Ids: L2Id list
      WaterBodies: WaterBody List }

let createL1Id countryId l1 =
    let (CountryId x) = countryId
    L1Id $"%s{x}_%s{l1}"

let l1Id l1Id =
    let (L1Id value) = l1Id
    value

type L2 =
    { Id: L2Id
      Name: string
      ContinentId: ContinentId
      CountryId: CountryId
      L1Id: L1Id
      L3Ids: L3Id list
      WaterBodies: WaterBody List }

let createL2Id l1Id l2 =
    let (L1Id x) = l1Id
    L2Id $"%s{x}_%s{l2}"

let l2Id l2Id =
    let (L2Id value) = l2Id
    value

type L3 =
    { Id: L3Id
      Name: string
      ContinentId: ContinentId
      CountryId: CountryId
      L1Id: L1Id
      L2Id: L2Id
      L4Ids: L4Id list
      WaterBodies: WaterBody List }

let createL3Id l2Id l3 =
    let (L2Id x) = l2Id
    L3Id $"%s{x}_%s{l3}"

let l3Id l3Id =
    let (L3Id value) = l3Id
    value

type L4 =
    { Id: L4Id
      Name: string
      ContinentId: ContinentId
      CountryId: CountryId
      L1Id: L1Id
      L2Id: L2Id
      L3Id: L3Id
      WaterBodies: WaterBody List }

let createL4Id l3Id l4 =
    let (L3Id x) = l3Id
    L4Id $"%s{x}_%s{l4}"

let l4Id l4Id =
    let (L4Id value) = l4Id
    value
