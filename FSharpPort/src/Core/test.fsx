#r "nuget: FSharp.Data"
// #load "Funcs.fs"
#load "Place.fs"
// #load "Score.fs"
// #load "Beach.fs"

type InputXml = FSharp.Data.XmlProvider<"C:\Self\Programming\Projects\BeachRankings\FSharpPort\src\Core\seed.xml">

let seed =
    InputXml.Load "C:\Self\Programming\Projects\BeachRankings\FSharpPort\src\Core\seed.xml"

let appendWaterBody waterBody list =
    match waterBody with
    | Some v ->
        match (System.String.IsNullOrEmpty v) with
        | true -> list
        | false -> List.append list [ Place.WaterBody v ]
    | None -> list

let getWaterBodies places filter map wb =
    places
    |> Array.filter filter
    |> Array.toList
    |> List.collect map
    |> appendWaterBody wb
    |> List.distinct

let l4s =
    seed.Continents
    |> Array.collect (fun c -> c.Countries)
    |> Array.collect (fun co -> co.L1s)
    |> Array.collect (fun l1 -> l1.L2s)
    |> Array.collect (fun l2 -> l2.L3s)
    |> Array.collect (fun l3 -> l3.L4s)
    |> Array.map
        (fun l4 ->
            let continentId = Place.createContinentId l4.Continent

            let countryId =
                Place.createCountryId continentId l4.Country

            let l1Id = Place.createL1Id countryId l4.L1
            let l2Id = Place.createL2Id l1Id l4.L2
            let l3Id = Place.createL3Id l2Id l4.L3

            { Place.L4.Id = Place.createL4Id l3Id l4.Name
              Place.L4.Name = l4.Name
              Place.L4.ContinentId = continentId
              Place.L4.CountryId = countryId
              Place.L4.L1Id = l1Id
              Place.L4.L2Id = l2Id
              Place.L4.L3Id = l3Id
              Place.L4.WaterBodies = [ Place.WaterBody l4.WaterBody ] })

let l3s =
    seed.Continents
    |> Array.collect (fun c -> c.Countries)
    |> Array.collect (fun co -> co.L1s)
    |> Array.collect (fun l1 -> l1.L2s)
    |> Array.collect (fun l2 -> l2.L3s)
    |> Array.map
        (fun l3 ->
            let continentId = Place.createContinentId l3.Continent

            let countryId =
                Place.createCountryId continentId l3.Country

            let l1Id = Place.createL1Id countryId l3.L1
            let l2Id = Place.createL2Id l1Id l3.L2
            let l3Id = Place.createL3Id l2Id l3.Name

            { Place.L3.Id = l3Id
              Place.L3.Name = l3.Name
              Place.L3.ContinentId = continentId
              Place.L3.CountryId = countryId
              Place.L3.L1Id = l1Id
              Place.L3.L2Id = l2Id
              Place.L3.L4Ids =
                  l3.L4s
                  |> Array.map (fun l4 -> Place.createL4Id l3Id l4.Name)
                  |> Array.toList
              Place.L3.WaterBodies = getWaterBodies l4s (fun x -> x.L3Id = l3Id) (fun x -> x.WaterBodies) l3.WaterBody })

let l2s =
    seed.Continents
    |> Array.collect (fun c -> c.Countries)
    |> Array.collect (fun co -> co.L1s)
    |> Array.collect (fun l1 -> l1.L2s)
    |> Array.map
        (fun l2 ->
            let continentId = Place.createContinentId l2.Continent

            let countryId =
                Place.createCountryId continentId l2.Country

            let l1Id = Place.createL1Id countryId l2.L1
            let id = Place.createL2Id l1Id l2.Name

            { Place.L2.Id = id
              Place.L2.Name = l2.Name
              Place.L2.ContinentId = continentId
              Place.L2.CountryId = countryId
              Place.L2.L1Id = l1Id
              Place.L2.L3Ids =
                  l2.L3s
                  |> Array.map (fun l3 -> Place.createL3Id id l3.Name)
                  |> Array.toList
              Place.L2.WaterBodies = getWaterBodies l3s (fun x -> x.L2Id = id) (fun x -> x.WaterBodies) l2.WaterBody })

let l1s =
    seed.Continents
    |> Array.collect (fun c -> c.Countries)
    |> Array.collect (fun co -> co.L1s)
    |> Array.map
        (fun l1 ->
            let continentId = Place.createContinentId l1.Continent

            let countryId =
                Place.createCountryId continentId l1.Country

            let id = Place.createL1Id countryId l1.Name

            { Place.L1.Id = id
              Place.L1.Name = l1.Name
              Place.L1.ContinentId = continentId
              Place.L1.CountryId = countryId
              Place.L1.L2Ids =
                  l1.L2s
                  |> Array.map (fun l2 -> Place.createL2Id id l2.Name)
                  |> Array.toList
              Place.L1.WaterBodies = getWaterBodies l2s (fun x -> x.L1Id = id) (fun x -> x.WaterBodies) l1.WaterBody })

let countries =
    seed.Continents
    |> Array.collect (fun c -> c.Countries)
    |> Array.map
        (fun c ->
            let continentId = Place.createContinentId c.Continent
            let id = Place.createCountryId continentId c.Name

            { Place.Country.Id = id
              Place.Country.Name = c.Name
              Place.Country.ContinentId = continentId
              Place.Country.L1Ids =
                  c.L1s
                  |> Array.map (fun l1 -> Place.createL1Id id l1.Name)
                  |> Array.toList
              Place.Country.WaterBodies =
                  getWaterBodies l1s (fun x -> x.CountryId = id) (fun x -> x.WaterBodies) c.WaterBody })

let continents =
    seed.Continents
    |> Array.map
        (fun c ->
            let id = Place.createContinentId c.Name

            { Place.Continent.Id = id
              Place.Continent.Name = c.Name
              Place.Continent.CountryIds =
                  c.Countries
                  |> Array.map (fun c -> Place.createCountryId id c.Name)
                  |> Array.toList
              Place.Continent.WaterBodies =
                  getWaterBodies countries (fun x -> x.ContinentId = id) (fun x -> x.WaterBodies) None })

let waterBodies = 
printfn
    "%d"
    (continents.Length
     + countries.Length
     + l1s.Length
     + l2s.Length
     + l3s.Length
     + l4s.Length)
